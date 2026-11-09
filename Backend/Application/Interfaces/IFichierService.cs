using Application.DTOs;

namespace Application.Interfaces;

public interface IFichierService
{
    Task<FichierDto> CreateAsync(FichierCreateDto dto, string userId);
    Task<FichierDto?> GetByIdAsync(int id);
    Task UpdateAsync(int id, FichierUpdateDto dto, string userId);
    Task DeleteAsync(int id);
    Task AutosaveAsync(int fichierId, string contenu, string userId);
    Task<IEnumerable<FichierVersionDto>> GetVersionsAsync(int fichierId);
    Task RestoreVersionAsync(int fichierId, int versionId, string userId);
    Task<FichierDto> RenameAsync(int id, FichierRenameDto dto);
}