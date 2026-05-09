using System.Text;
using System.Text.Json;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services;

/// <summary>
/// Service pour gérer les utilisateurs dans Keycloak via l'Admin API
/// </summary>
public class KeycloakAdminService : IKeycloakAdminService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<KeycloakAdminService> _logger;
    private string _adminToken = string.Empty;
    private DateTime _tokenExpiry = DateTime.MinValue;

    public KeycloakAdminService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<KeycloakAdminService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Obtient un token d'accès Admin de Keycloak (avec cache)
    /// </summary>
    private async Task<string> GetAdminTokenAsync()
    {
        // Retourner le token en cache s'il n'a pas expiré
        if (!string.IsNullOrEmpty(_adminToken) && DateTime.UtcNow < _tokenExpiry)
            return _adminToken;

        // admin-cli s'authentifie via le realm MASTER avec username/password
        var keycloakBaseUrl = _configuration["Keycloak:AdminUrl"] ?? "http://localhost:8080";
        var tokenUrl = $"{keycloakBaseUrl.TrimEnd('/')}/realms/master/protocol/openid-connect/token";
        
        var formData = new Dictionary<string, string>
        {
            { "client_id", "admin-cli" },
            { "username", _configuration["Keycloak:Admin:Username"] ?? "admin" },
            { "password", _configuration["Keycloak:Admin:Password"] ?? "admin" },
            { "grant_type", "password" }
        };

        var content = new FormUrlEncodedContent(formData);
        
        try
        {
            var response = await _httpClient.PostAsync(tokenUrl, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Keycloak token error: {response.StatusCode} - {error}");
                throw new Exception($"Failed to get Keycloak admin token: {response.StatusCode}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var token = JsonDocument.Parse(jsonResponse);
            _adminToken = token.RootElement.GetProperty("access_token").GetString() ?? "";
            var expiresIn = token.RootElement.GetProperty("expires_in").GetInt32();
            
            _tokenExpiry = DateTime.UtcNow.AddSeconds(expiresIn - 30);

            _logger.LogInformation("Keycloak admin token obtained successfully via master realm");
            return _adminToken;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting Keycloak token: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Crée un nouvel utilisateur dans Keycloak
    /// </summary>
    public async Task<string> CreateUserAsync(string email, string firstName, string lastName)
    {
        var token = await GetAdminTokenAsync();
        var realmName = _configuration["Keycloak:RealmName"] ?? "unprompted";
        var adminUrl = _configuration["Keycloak:AdminUrl"];

        var createUserUrl = $"{adminUrl}/admin/realms/{realmName}/users";

        // Générer un username unique basé sur l'email
        var username = email.Split('@')[0] + "_" + Guid.NewGuid().ToString().Substring(0, 8);

        var userPayload = new
        {
            email = email,
            firstName = firstName,
            lastName = lastName,
            enabled = true,
            username = username
        };

        var json = JsonSerializer.Serialize(userPayload);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, createUserUrl);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = httpContent;

        try
        {
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Keycloak create user error: {response.StatusCode} - {error}");
                throw new Exception($"Failed to create user in Keycloak: {response.StatusCode} - {error}");
            }

            // Keycloak retourne l'URL de l'utilisateur créé dans le header Location
            var locationHeader = response.Headers.Location?.ToString() ?? "";
            var userId = locationHeader.Split('/').Last();
            
            _logger.LogInformation($"User created in Keycloak with ID: {userId}, username: {username}");
            return userId;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating user in Keycloak: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Définit le mot de passe d'un utilisateur dans Keycloak
    /// </summary>
    public async Task SetUserPasswordAsync(string userId, string password)
    {
        var token = await GetAdminTokenAsync();
        var realmName = _configuration["Keycloak:RealmName"] ?? "unprompted";
        var adminUrl = _configuration["Keycloak:AdminUrl"];

        var setPasswordUrl = $"{adminUrl}/admin/realms/{realmName}/users/{userId}/reset-password";

        var passwordPayload = new
        {
            type = "password",
            value = password,
            temporary = true
        };

        var json = JsonSerializer.Serialize(passwordPayload);
        var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Put, setPasswordUrl);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = httpContent;

        try
        {
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Keycloak set password error: {response.StatusCode} - {error}");
                throw new Exception($"Failed to set password in Keycloak: {response.StatusCode} - {error}");
            }

            _logger.LogInformation($"Password set for user {userId} in Keycloak");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error setting password in Keycloak: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Supprime un utilisateur de Keycloak
    /// </summary>
    public async Task DeleteUserAsync(string userId)
    {
        var token = await GetAdminTokenAsync();
        var realmName = _configuration["Keycloak:RealmName"] ?? "unprompted";
        var adminUrl = _configuration["Keycloak:AdminUrl"];

        var deleteUserUrl = $"{adminUrl}/admin/realms/{realmName}/users/{userId}";

        var request = new HttpRequestMessage(HttpMethod.Delete, deleteUserUrl);
        request.Headers.Add("Authorization", $"Bearer {token}");

        try
        {
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Keycloak delete user error: {response.StatusCode} - {error}");
                throw new Exception($"Failed to delete user in Keycloak: {response.StatusCode} - {error}");
            }

            _logger.LogInformation($"User {userId} deleted from Keycloak");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting user from Keycloak: {ex.Message}");
            throw;
        }
    }
}
