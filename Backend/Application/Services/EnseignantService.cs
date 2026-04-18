using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class EnseignantService : IEnseignantService
{
    private readonly IEnseignantRepository _enseignantRepo;
    private readonly IUtilisateurRepository _utilisateurRepo;

    public EnseignantService(IEnseignantRepository enseignantRepo, IUtilisateurRepository utilisateurRepo)
    {
        _enseignantRepo = enseignantRepo;
        _utilisateurRepo = utilisateurRepo;
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

    public async Task<EnseignantDto> CreateEnseignantAsync(EnseignantCreateDto request)
    {
        var existingUser = await _utilisateurRepo.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new Exception("Un utilisateur avec cet email existe déjà.");

        var nouvelUtilisateur = new Domain.Entities.Utilisateur
        {
            Nom = request.Nom,
            Prenom = request.Prenom,
            Email = request.Email,
            MotDePasse = request.MotDePasse,
            Statut = "Actif"
        };
        await _utilisateurRepo.AddAsync(nouvelUtilisateur);

        var nouvelEnseignant = new Domain.Entities.Enseignant
        {
            IdUtilisateur = nouvelUtilisateur.IdUtilisateur,
            Specialite = request.Specialite
        };
        await _enseignantRepo.AddAsync(nouvelEnseignant);

        return new EnseignantDto
        {
            Id = nouvelEnseignant.IdEnseignant,
            NomComplet = $"{nouvelUtilisateur.Prenom} {nouvelUtilisateur.Nom}",
            Email = nouvelUtilisateur.Email,
            Specialite = nouvelEnseignant.Specialite,
            Statut = nouvelUtilisateur.Statut,
            ClassesAssignees = new List<string>()
        };
    }
    
}