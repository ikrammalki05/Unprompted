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

    public async Task<EnseignantDto?> GetEnseignantByEmailAsync(string email)
    {
        var enseignants = await _enseignantRepo.GetAllAsync();
        var e = enseignants.FirstOrDefault(x => x.IdUtilisateurNavigation?.Email?.ToLower() == email.ToLower());

        if (e == null) return null;

        return new EnseignantDto
        {
            Id = e.IdEnseignant,
            NomComplet = $"{e.IdUtilisateurNavigation?.Prenom} {e.IdUtilisateurNavigation?.Nom}",
            Email = e.IdUtilisateurNavigation?.Email ?? "Email inconnu",
            Specialite = e.Specialite ?? "Non spécifiée",
            Statut = e.IdUtilisateurNavigation?.Statut ?? "Inactif"
        };
    }

    public async Task UpdateProfilByEmailAsync(string email, EnseignantCreateDto request)
    {
        var utilisateur = await _utilisateurRepo.GetByEmailAsync(email);
        if (utilisateur == null) throw new Exception("Utilisateur introuvable.");

        var enseignant = await _enseignantRepo.GetByEmailAsync(email);
        if (enseignant == null) throw new Exception("Profil enseignant introuvable.");

        await UpdateEnseignantAsync(enseignant.IdEnseignant, request);
    }

    public async Task<EnseignantDto> EnsureEnseignantExistsAsync(string email, string firstName, string lastName)
    {
        var existing = await GetEnseignantByEmailAsync(email);
        if (existing != null) return existing;

        _logger.LogInformation($"Création automatique du profil enseignant pour {email}");

        // 1. Vérifier si l'utilisateur existe déjà
        var utilisateur = await _utilisateurRepo.GetByEmailAsync(email);
        if (utilisateur == null)
        {
            utilisateur = new Domain.Entities.Utilisateur
            {
                Nom = lastName,
                Prenom = firstName,
                Email = email,
                Statut = "Actif"
            };
            await _utilisateurRepo.AddAsync(utilisateur);
        }

        // 2. Créer le profil Enseignant
        var enseignant = new Domain.Entities.Enseignant
        {
            IdUtilisateur = utilisateur.IdUtilisateur,
            Specialite = "Auto-généré"
        };
        await _enseignantRepo.AddAsync(enseignant);

        return new EnseignantDto
        {
            Id = enseignant.IdEnseignant,
            NomComplet = $"{utilisateur.Prenom} {utilisateur.Nom}",
            Email = utilisateur.Email,
            Specialite = enseignant.Specialite,
            Statut = utilisateur.Statut
        };
    }
}
