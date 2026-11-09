using Application.DTOs;

namespace Application.Interfaces;

public interface IAiGatewayService
{
    Task<AiAskResponseDto> AskAsync(int idProjet, int idEtudiant, string message);
    Task UploadCahierAsync(int idProjet, Stream fileStream, string fileName, string contentType);
    string ResolveAccessLevel(ConfigurationIumDto? config);
}
