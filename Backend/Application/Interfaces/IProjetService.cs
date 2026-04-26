using Application.DTOs;

namespace Application.Interfaces;

public interface IProjetService
{
    Task<IEnumerable<ProjetDto>> GetAllProjetsAsync();
    Task<IEnumerable<ProjetDto>> GetProjetsByEnseignantAsync(int idEnseignant);
    Task<ProjetDto> CreateProjetAsync(ProjetCreateDto request);
}