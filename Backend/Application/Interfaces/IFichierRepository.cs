using Domain.Entities;

namespace Application.Interfaces;

public interface IFichierRepository
{
    Task<IEnumerable<Fichier>> GetByProjectIdAsync(int projectId);
    Task<Fichier?> GetByIdAsync(int id);
    Task<bool> ExistsAsync(string nom, int? dossierId, int projectId);

    Task AddAsync(Fichier fichier);
    Task UpdateAsync(Fichier fichier);
    Task DeleteAsync(int id);
}