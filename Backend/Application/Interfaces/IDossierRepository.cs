using Domain.Entities;

namespace Application.Interfaces;

public interface IDossierRepository
{
    Task<IEnumerable<Dossier>> GetByProjectIdAsync(int projectId);
    Task<Dossier?> GetByIdAsync(int id);

    Task<bool> ExistsAsync(string nom, int? parentId, int projectId);

    Task AddAsync(Dossier dossier);
    Task DeleteAsync(int id);
    Task UpdateAsync(Dossier dossier);
}