using Application.DTOs;

namespace Application.Interfaces;

public interface IEnseignantService
{
    Task<IEnumerable<EnseignantDto>> GetAllEnseignantsAsync();
}