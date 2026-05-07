using Application.DTOs;

namespace Application.Interfaces;

public interface IDossierService
{
    Task<DossierDto> CreateAsync(DossierCreateDto dto);
    Task DeleteAsync(int id);
    Task<DossierDto?> GetByIdAsync(int id);
    Task<IEnumerable<DossierDto>> GetByProjetIdAsync(int projetId);
    Task<DossierDto> RenameAsync(int id, DossierRenameDto dto);
}