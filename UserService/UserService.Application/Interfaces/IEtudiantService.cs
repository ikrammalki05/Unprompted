using Application.DTOs;

namespace Application.Interfaces;

public interface IEtudiantService
{
    Task<EtudiantDto?> GetEtudiantByIdAsync(int id);
    Task<IEnumerable<EtudiantDto>> GetAllEtudiantsAsync();
    Task<EtudiantDto> CreateEtudiantAsync(EtudiantCreateDto request); 
    Task DeleteEtudiantAsync(int id);
}
