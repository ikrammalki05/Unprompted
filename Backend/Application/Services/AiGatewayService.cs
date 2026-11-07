using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AiGatewayService : IAiGatewayService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IConfigurationIumService _configService;
    private readonly IProjetService _projetService;
    private readonly ILogger<AiGatewayService> _logger;

    public AiGatewayService(
        HttpClient httpClient,
        IConfiguration configuration,
        IConfigurationIumService configService,
        IProjetService projetService,
        ILogger<AiGatewayService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _configService = configService;
        _projetService = projetService;
        _logger = logger;
    }

    public string ResolveAccessLevel(ConfigurationIumDto? config)
    {
        if (config == null) return "restreint";
        if (config.GenerationCodeAutorisee == false) return "restreint";
        if ((config.QuotaRequetes ?? 1000) <= 50) return "limité";
        return "illimité";
    }

    public async Task<AiAskResponseDto> AskAsync(int idProjet, int idEtudiant, string message)
    {
        var config = await _configService.GetConfigByProjetAsync(idProjet);
        var accessLevel = ResolveAccessLevel(config);

        var baseUrl = _configuration["AiService:BaseUrl"]?.TrimEnd('/');
        var apiKey = _configuration["AiService:ApiKey"] ?? "un_mot_de_passe_super_secret_pfe_2026";

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            var fallback = BuildOfflineResponse(message, accessLevel);
            await PersistPrompt(idProjet, idEtudiant, message, fallback);
            return fallback;
        }

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/ask");
            request.Headers.TryAddWithoutValidation("X-Service-API-Key", apiKey);
            request.Content = new StringContent(JsonSerializer.Serialize(new
            {
                student_id = idEtudiant,
                project_id = idProjet,
                message,
                access_level = accessLevel
            }), Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("AI /ask failed: {Status} {Body}", response.StatusCode, body);
                var offline = BuildOfflineResponse(message, accessLevel);
                offline.Status = "degraded";
                await PersistPrompt(idProjet, idEtudiant, message, offline);
                return offline;
            }

            using var doc = JsonDocument.Parse(body);
            var root = doc.RootElement;
            var result = new AiAskResponseDto
            {
                Response = root.TryGetProperty("response", out var r) ? r.GetString() ?? "" : body,
                TokensUsed = root.TryGetProperty("tokens_used", out var t) ? t.GetInt32() : 0,
                DependencyScore = root.TryGetProperty("dependency_score", out var d) ? d.GetDouble() : 0,
                Status = root.TryGetProperty("status", out var s) ? s.GetString() ?? "success" : "success",
                AccessLevel = accessLevel
            };

            await PersistPrompt(idProjet, idEtudiant, message, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI gateway ask failed");
            var offline = BuildOfflineResponse(message, accessLevel);
            offline.Status = "degraded";
            await PersistPrompt(idProjet, idEtudiant, message, offline);
            return offline;
        }
    }

    public async Task UploadCahierAsync(int idProjet, Stream fileStream, string fileName, string contentType)
    {
        await using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms);
        await _projetService.SaveCahierAsync(idProjet, ms.ToArray());

        var baseUrl = _configuration["AiService:BaseUrl"]?.TrimEnd('/');
        var apiKey = _configuration["AiService:ApiKey"] ?? "un_mot_de_passe_super_secret_pfe_2026";
        if (string.IsNullOrWhiteSpace(baseUrl)) return;

        try
        {
            ms.Position = 0;
            using var form = new MultipartFormDataContent();
            form.Add(new StringContent(idProjet.ToString()), "project_id");
            var fileContent = new ByteArrayContent(ms.ToArray());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(
                string.IsNullOrWhiteSpace(contentType) ? "application/pdf" : contentType);
            form.Add(fileContent, "file", fileName);

            using var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/upload");
            request.Headers.TryAddWithoutValidation("X-Service-API-Key", apiKey);
            request.Content = form;

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("AI /upload failed: {Status} {Body}", response.StatusCode, body);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI upload failed for project {ProjectId}", idProjet);
        }
    }

    private async Task PersistPrompt(int idProjet, int idEtudiant, string message, AiAskResponseDto result)
    {
        await _projetService.CreatePromptAsync(idProjet, new PromptCreateDto
        {
            Contenu = $"[Q] {message}\n[R] {result.Response}",
            IdEtudiant = idEtudiant,
            NbTokensEntree = Math.Max(1, message.Length / 4),
            NbTokensSortie = result.TokensUsed
        });
    }

    private static AiAskResponseDto BuildOfflineResponse(string message, string accessLevel)
    {
        var reply = accessLevel switch
        {
            "restreint" =>
                "Service RAG indisponible pour le moment. En mode restreint, je ne peux répondre qu'à partir du cahier des charges indexé. Relancez le service AI (port 8000) après upload du PDF.",
            "limité" =>
                $"Service RAG hors-ligne (mode limité). Votre question « {Truncate(message, 120)} » a été enregistrée. Relancez AI pour une réponse contextualisée.",
            _ =>
                $"Service RAG hors-ligne (mode illimité). Question enregistrée : « {Truncate(message, 120)} ». Démarrez le service Python AI pour activer la génération complète."
        };

        return new AiAskResponseDto
        {
            Response = reply,
            TokensUsed = 0,
            DependencyScore = 0,
            Status = "offline",
            AccessLevel = accessLevel
        };
    }

    private static string Truncate(string value, int max)
        => value.Length <= max ? value : value[..max] + "…";
}
