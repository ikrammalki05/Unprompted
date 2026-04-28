using Application.DTOs;

namespace Application.Interfaces;

public interface IEtudiantService
{
    Task<IEnumerable<EtudiantDto>> GetAllEtudiantsAsync();
    Task<EtudiantDto> CreateEtudiantAsync(EtudiantCreateDto request);
    Task UpdateEtudiantAsync(int id, EtudiantCreateDto request);
    Task DeleteEtudiantAsync(int id);
}