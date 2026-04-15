using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class ClasseService : IClasseService
{
    private readonly IClasseRepository _classeRepo;

    public ClasseService(IClasseRepository classeRepo)
    {
        _classeRepo = classeRepo;
    }

    public async Task<IEnumerable<ClasseDto>> GetAllClassesAsync()
    {
        // Attention : GetAllAsync retourne des "Groupe" selon IClasseRepository
        var groupes = await _classeRepo.GetAllAsync();

        return groupes.Select(g => 
        {
            // On cherche s'il y a un enseignant affecté à ce groupe pour le mettre en référent
            var affectationEnseignant = g.Affectations?.FirstOrDefault(a => a.IdEnseignant != null);
            var nomEnseignant = affectationEnseignant?.IdEnseignantNavigation?.IdUtilisateurNavigation != null
                ? $"{affectationEnseignant.IdEnseignantNavigation.IdUtilisateurNavigation.Prenom} {affectationEnseignant.IdEnseignantNavigation.IdUtilisateurNavigation.Nom}"
                : "Aucun";

            return new ClasseDto
            {
                Id = g.IdGroupe,
                NomClasse = g.NomGroupe,
                AnneeAcademique = "2023 - 2024", // Valeur par défaut pour correspondre à la maquette
                // On compte le nombre d'étudiants distincts affectés à ce groupe
                EffectifActuel = g.Affectations?.Where(a => a.IdEtudiant != 0).Select(a => a.IdEtudiant).Distinct().Count() ?? 0,
                EffectifMax = 40, // Valeur par défaut de la maquette
                EnseignantReferent = nomEnseignant
            };
        });
    }
}