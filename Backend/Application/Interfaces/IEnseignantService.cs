using Application.DTOs;

namespace Application.Interfaces;

public interface IEnseignantService
{
    Task<IEnumerable<EnseignantDto>> GetAllEnseignantsAsync();
    Task<EnseignantDto> CreateEnseignantAsync(EnseignantCreateDto request);
}