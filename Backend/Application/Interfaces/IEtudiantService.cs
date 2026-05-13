using Application.DTOs;

namespace Application.Interfaces;

public interface IEtudiantService
{
    Task<IEnumerable<EtudiantDto>> GetAllEtudiantsAsync();
    Task<EtudiantDto> CreateEtudiantAsync(EtudiantCreateDto request);
    Task<EtudiantProfilDto?> GetProfilEtudiantAsync(int idEtudiant);
    Task<EtudiantProfilDto?> GetProfilByEmailAsync(string email);
    Task UpdateEtudiantAsync(int id, EtudiantCreateDto request);
    Task UpdateProfilByEmailAsync(string email, EtudiantCreateDto request);
    Task<HistoriqueEtudiantDto?> GetHistoriqueAsync(int idEtudiant);
    Task DeleteEtudiantAsync(int id);
}