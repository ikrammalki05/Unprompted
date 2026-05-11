using Application.DTOs;

namespace Application.Interfaces;

public interface IEnseignantService
{
    Task<EnseignantDto?> GetEnseignantByIdAsync(int id);
    Task<IEnumerable<EnseignantDto>> GetAllEnseignantsAsync();
    Task<EnseignantDto> CreateEnseignantAsync(EnseignantCreateDto request);
    Task DeleteEnseignantAsync(int id);
}
