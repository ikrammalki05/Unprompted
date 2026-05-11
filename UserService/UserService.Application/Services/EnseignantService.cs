using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class EnseignantService : IEnseignantService
{
    private readonly IEnseignantRepository _enseignantRepo;
    private readonly IUtilisateurRepository _utilisateurRepo;
    private readonly IKeycloakAdminService _keycloakService;
    private readonly ILogger<EnseignantService> _logger;

    public EnseignantService(
        IEnseignantRepository enseignantRepo,
        IUtilisateurRepository utilisateurRepo,
        IKeycloakAdminService keycloakService,
        ILogger<EnseignantService> logger)
    {
        _enseignantRepo = enseignantRepo;
        _utilisateurRepo = utilisateurRepo;
        _keycloakService = keycloakService;
        _logger = logger;
    }

    public async Task<EnseignantDto?> GetEnseignantByIdAsync(int id)
    {
        var e = await _enseignantRepo.GetByIdAsync(id);
        if (e == null) return null;

        return new EnseignantDto
        {
            IdEnseignant = e.IdEnseignant,
            Nom = e.IdUtilisateurNavigation.Nom,
            Prenom = e.IdUtilisateurNavigation.Prenom,
            Email = e.IdUtilisateurNavigation.Email,
            Specialite = e.Specialite,
            Departement = e.Departement,
            Statut = e.IdUtilisateurNavigation.Statut
        };
    }

    public async Task<IEnumerable<EnseignantDto>> GetAllEnseignantsAsync()
    {
        var enseignants = await _enseignantRepo.GetAllAsync();
        return enseignants.Select(e => new EnseignantDto
        {
            IdEnseignant = e.IdEnseignant,
            Nom = e.IdUtilisateurNavigation.Nom,
            Prenom = e.IdUtilisateurNavigation.Prenom,
            Email = e.IdUtilisateurNavigation.Email,
            Specialite = e.Specialite,
            Departement = e.Departement,
            Statut = e.IdUtilisateurNavigation.Statut
        });
    }

    public async Task<EnseignantDto> CreateEnseignantAsync(EnseignantCreateDto request)
    {
        // 1. Créer dans Keycloak
        string keycloakUserId;
        string tempPassword = "Password123!"; 
        try
        {
            keycloakUserId = await _keycloakService.CreateUserAsync(request.Email, request.Prenom, request.Nom);
            await _keycloakService.SetUserPasswordAsync(keycloakUserId, tempPassword);
            _logger.LogInformation($"Enseignant créé dans Keycloak avec ID: {keycloakUserId}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Échec de la création dans Keycloak: {ex.Message}");
            throw new Exception($"Impossible de créer l'utilisateur dans Keycloak: {ex.Message}");
        }

        try
        {
            // 2. Créer l'utilisateur en BD
            var utilisateur = new Utilisateur
            {
                Nom = request.Nom,
                Prenom = request.Prenom,
                Email = request.Email,
                Statut = "Actif"
            };
            await _utilisateurRepo.AddAsync(utilisateur);

            // 3. Créer l'enseignant
            var enseignant = new Enseignant
            {
                Specialite = request.Specialite,
                Departement = request.Departement,
                IdUtilisateur = utilisateur.IdUtilisateur
            };
            await _enseignantRepo.AddAsync(enseignant);

            return new EnseignantDto
            {
                IdEnseignant = enseignant.IdEnseignant,
                Nom = utilisateur.Nom,
                Prenom = utilisateur.Prenom,
                Email = utilisateur.Email,
                Specialite = enseignant.Specialite,
                Departement = enseignant.Departement,
                Statut = utilisateur.Statut
            };
        }
        catch (Exception ex)
        {
            // Rollback Keycloak si erreur BD
            await _keycloakService.DeleteUserAsync(keycloakUserId);
            throw new Exception($"Erreur BD, rollback Keycloak effectué : {ex.Message}");
        }
    }

    public async Task DeleteEnseignantAsync(int id)
    {
        await _enseignantRepo.DeleteAsync(id);
    }
}
