using Domain.Entities;

namespace Application.Interfaces;
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
    Task AddPromptAsync(Prompt prompt);
    Task AddAffectationAsync(Affectation affectation);
    Task<int> CountAsync();
    Task AssignProjectToGroupAsync(int idProjet, int idGroupe);
    Task<IEnumerable<Projet>> GetByEtudiantIdAsync(int idEtudiant);
    Task<IEnumerable<Prompt>> GetPromptsByProjectAndEtudiantAsync(int idProjet, int idEtudiant);
    Task<IEnumerable<Contribution>> GetContributionsByProjectAndEtudiantAsync(int idProjet, int idEtudiant);
}