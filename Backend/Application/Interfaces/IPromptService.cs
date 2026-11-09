using Application.DTOs;

namespace Application.Interfaces;

public interface IPromptService
{
    Task<bool> LogInteractionAsync(PromptLogRequestDto request);
    Task<IEnumerable<PromptDto>> GetHistoriqueProjetAsync(int idProjet);
}