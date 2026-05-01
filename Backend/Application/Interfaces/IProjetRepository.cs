using Domain.Entities;

namespace Application.Interfaces;
<<<<<<< HEAD

public interface IProjetRepository
{
    Task<IEnumerable<Projet>> GetAllAsync();
    Task<IEnumerable<Projet>> GetByEnseignantIdAsync(int idEnseignant);
    Task<Projet?> GetByIdAsync(int id);
    Task AddAsync(Projet projet);
=======
public interface IProjetRepository
{
    Task<IEnumerable<Projet>> GetAllAsync();
    Task<Projet?> GetByIdAsync(int id);
    Task <IEnumerable<Projet>>GetByEnseignantIdAsync(int idEnseignant);
    Task AddAsync(Projet projet);
    Task UpdateAsync(Projet projet);
    Task DeleteAsync(int id);
    Task<IEnumerable<Contribution>> GetContributionsByProjectIdAsync(int idProjet);
    Task AddContributionAsync(Contribution contribution);
    Task<int> CountAsync();
    Task AssignProjectToGroupAsync(int idProjet, int idGroupe);

>>>>>>> origin/feature/fix-keycloak
}