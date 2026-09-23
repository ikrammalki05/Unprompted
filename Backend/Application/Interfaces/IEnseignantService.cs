using Application.DTOs;

namespace Application.Interfaces;

public interface IEnseignantService
{
    Task<IEnumerable<EnseignantDto>> GetAllEnseignantsAsync();
    Task<EnseignantDto> CreateEnseignantAsync(EnseignantCreateDto request);
    Task UpdateEnseignantAsync(int id, EnseignantCreateDto request);
    Task DeleteEnseignantAsync(int id);
    Task<StatistiquesEnseignantDto> GetStatistiquesAsync(int idEnseignant);
    Task<EnseignantDto?> GetEnseignantByEmailAsync(string email);
    Task UpdateProfilByEmailAsync(string email, EnseignantCreateDto request);
    Task<EnseignantDto> EnsureEnseignantExistsAsync(string email, string firstName, string lastName);
}