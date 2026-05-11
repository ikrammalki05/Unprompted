using Domain.Entities;

namespace Application.Interfaces;

public interface IEtudiantRepository
{
    Task<Etudiant?> GetByIdAsync(int id);
    Task<Etudiant?> GetByUtilisateurIdAsync(int idUtilisateur);
    Task<Etudiant?> GetByCodeApogeeAsync(string codeApogee);
    Task<IEnumerable<Etudiant>> GetAllAsync();
    Task<Etudiant> AddAsync(Etudiant etudiant);
    Task UpdateAsync(Etudiant etudiant);
    Task DeleteAsync(int id);
}
