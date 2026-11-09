using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class EnseignantService : IEnseignantService
{
    private readonly IEnseignantRepository _enseignantRepo;
    private readonly IUtilisateurRepository _utilisateurRepo;
    private readonly IProjetRepository _projetRepo;
    private readonly IKeycloakAdminService _keycloakService;
    private readonly ILogger<EnseignantService> _logger;

    public EnseignantService(
        IEnseignantRepository enseignantRepo,
        IUtilisateurRepository utilisateurRepo,
        IProjetRepository projetRepo,
        IKeycloakAdminService keycloakService,
        ILogger<EnseignantService> logger)
    {
        _enseignantRepo = enseignantRepo;
        _utilisateurRepo = utilisateurRepo;
        _projetRepo = projetRepo;
        _keycloakService = keycloakService;
        _logger = logger;
    }

    public async Task<IEnumerable<EnseignantDto>> GetAllEnseignantsAsync()
    {
        var enseignants = await _enseignantRepo.GetAllAsync();

        return enseignants.Select(e => new EnseignantDto
        {
            Id = e.IdEnseignant,
            NomComplet = $"{e.IdUtilisateurNavigation?.Prenom} {e.IdUtilisateurNavigation?.Nom}",
            Email = e.IdUtilisateurNavigation?.Email ?? "Email inconnu",
            Specialite = e.Specialite ?? "Non spécifiée",
            Statut = e.IdUtilisateurNavigation?.Statut ?? "Inactif",
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

        // 1. Créer l'utilisateur dans Keycloak d'abord
        string keycloakUserId = string.Empty;
        try
        {
            keycloakUserId = await _keycloakService.CreateUserAsync(request.Email, request.Prenom, request.Nom);

            // Mot de passe temporaire : Prenom + Specialite (ex: Ahmed123Informatique)
            var tempPassword = $"{request.Prenom}Enseignant2024!";
            await _keycloakService.SetUserPasswordAsync(keycloakUserId, tempPassword);

            _logger.LogInformation($"Enseignant créé dans Keycloak avec ID: {keycloakUserId}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Échec de la création dans Keycloak: {ex.Message}");
            throw new Exception($"Impossible de créer l'utilisateur dans Keycloak: {ex.Message}");
        }

        // 2. Créer l'Utilisateur en base de données
        Domain.Entities.Utilisateur nouvelUtilisateur;
        try
        {
            nouvelUtilisateur = new Domain.Entities.Utilisateur
            {
                Nom = request.Nom,
                Prenom = request.Prenom,
                Email = request.Email,
                Statut = "Actif"
            };
            await _utilisateurRepo.AddAsync(nouvelUtilisateur);
        }
        catch (Exception ex)
        {
            // Rollback : supprimer l'utilisateur Keycloak si la BD échoue
            _logger.LogError($"Échec de la création en BD, rollback Keycloak: {ex.Message}");
            await _keycloakService.DeleteUserAsync(keycloakUserId);
            throw;
        }

        // 3. Créer le profil Enseignant lié
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

    public async Task UpdateEnseignantAsync(int id, EnseignantCreateDto request)
    {
        var enseignant = await _enseignantRepo.GetByIdAsync(id);
        if (enseignant == null)
            throw new ArgumentException($"Enseignant avec l'id {id} introuvable.");

        var utilisateur = await _utilisateurRepo.GetByIdAsync(enseignant.IdUtilisateur);
        if (utilisateur == null)
            throw new ArgumentException("Utilisateur introuvable.");

        utilisateur.Nom = request.Nom;
        utilisateur.Prenom = request.Prenom;
        utilisateur.Email = request.Email;
        enseignant.Specialite = request.Specialite;

        await _utilisateurRepo.UpdateAsync(utilisateur);
        await _enseignantRepo.UpdateAsync(enseignant);
    }

    public async Task DeleteEnseignantAsync(int id)
    {
        var enseignant = await _enseignantRepo.GetByIdAsync(id);
        if (enseignant == null)
            throw new ArgumentException($"Enseignant avec l'id {id} introuvable.");

        await _enseignantRepo.DeleteAsync(id);
    }

    public async Task<StatistiquesEnseignantDto> GetStatistiquesAsync(int idEnseignant)
    {
        var projets = await _projetRepo.GetByEnseignantIdAsync(idEnseignant);
        var projetCount = projets.Count();

        var etudiants = new HashSet<int>();
        foreach (var projet in projets)
        {
            if (projet.Groupes != null)
            {
                foreach (var groupe in projet.Groupes)
                {
                    // Les groupes contiennent les étudiants
                }
            }
        }

        return new StatistiquesEnseignantDto
        {
            ProjetsSupervisés = projetCount,
            ÉtudiantsActifs = etudiants.Count,
            ÉvaluationsEnAttente = 0,
            ActivitésRécentes = 0
        };
    }
}
