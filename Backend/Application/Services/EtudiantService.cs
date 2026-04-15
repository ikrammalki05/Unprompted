using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class EtudiantService : IEtudiantService
{
    private readonly IEtudiantRepository _etudiantRepo;
    private readonly IUtilisateurRepository _utilisateurRepo;

   public EtudiantService(IEtudiantRepository etudiantRepo, IUtilisateurRepository utilisateurRepo)
    {
        _etudiantRepo = etudiantRepo;
        _utilisateurRepo = utilisateurRepo;
    }

    public async Task<IEnumerable<EtudiantDto>> GetAllEtudiantsAsync()
    {
        var etudiants = await _etudiantRepo.GetAllAsync();
        
        // On transforme (Map) l'entité de la BDD vers le DTO attendu par Figma
        return etudiants.Select(e => new EtudiantDto
        {
            Id = e.IdEtudiant,
            CodeApogee = e.CodeApogee,
            // On suppose que le Repository inclut la navigation vers Utilisateur
            NomComplet = $"{e.IdUtilisateurNavigation?.Prenom} {e.IdUtilisateurNavigation?.Nom}",
            Email = e.IdUtilisateurNavigation?.Email ?? "Email inconnu",
            Statut = e.IdUtilisateurNavigation?.Statut ?? "Inactif",
            // On récupère le nom du premier groupe (classe) auquel il est affecté
            ClasseNom = e.Affectations?.FirstOrDefault()?.IdGroupeNavigation?.NomGroupe ?? "Non assigné"
        });
    }
}