using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class EnseignantService : IEnseignantService
{
    private readonly IEnseignantRepository _enseignantRepo;

    public EnseignantService(IEnseignantRepository enseignantRepo)
    {
        _enseignantRepo = enseignantRepo;
    }

    public async Task<IEnumerable<EnseignantDto>> GetAllEnseignantsAsync()
    {
        var enseignants = await _enseignantRepo.GetAllAsync();

        return enseignants.Select(e => new EnseignantDto
        {
            Id = e.IdEnseignant,
            // Navigation vers la table Utilisateur pour récupérer le nom et l'email
            NomComplet = $"{e.IdUtilisateurNavigation?.Prenom} {e.IdUtilisateurNavigation?.Nom}",
            Email = e.IdUtilisateurNavigation?.Email ?? "Email inconnu",
            Specialite = e.Specialite ?? "Non spécifiée",
            Statut = e.IdUtilisateurNavigation?.Statut ?? "Inactif",
            
            // On récupère la liste de toutes les classes (Groupes) où il est affecté
            ClassesAssignees = e.Affectations != null 
                ? e.Affectations.Select(a => a.IdGroupeNavigation?.NomGroupe ?? "").Where(n => !string.IsNullOrEmpty(n)).ToList()
                : new List<string>()
        });
    }
}