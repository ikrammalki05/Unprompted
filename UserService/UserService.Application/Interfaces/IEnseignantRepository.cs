using Domain.Entities;

namespace Application.Interfaces;

public interface IEnseignantRepository
{
    Task<Enseignant?> GetByIdAsync(int id);
    Task<Enseignant?> GetByUtilisateurIdAsync(int idUtilisateur);
    Task<IEnumerable<Enseignant>> GetAllAsync();
    Task<Enseignant> AddAsync(Enseignant enseignant);
    Task UpdateAsync(Enseignant enseignant);
    Task DeleteAsync(int id);
}
