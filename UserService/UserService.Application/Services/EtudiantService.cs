using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class EtudiantService : IEtudiantService
{
    private readonly IEtudiantRepository _etudiantRepo;
    private readonly IUtilisateurRepository _utilisateurRepo;
    private readonly IKeycloakAdminService _keycloakService;
    private readonly ILogger<EtudiantService> _logger;

    public EtudiantService(
        IEtudiantRepository etudiantRepo,
        IUtilisateurRepository utilisateurRepo,
        IKeycloakAdminService keycloakService,
        ILogger<EtudiantService> logger)
    {
        _etudiantRepo = etudiantRepo;
        _utilisateurRepo = utilisateurRepo;
        _keycloakService = keycloakService;
        _logger = logger;
    }

    public async Task<EtudiantDto?> GetEtudiantByIdAsync(int id)
    {
        var e = await _etudiantRepo.GetByIdAsync(id);
        if (e == null) return null;

        return new EtudiantDto
        {
            IdEtudiant = e.IdEtudiant,
            CodeApogee = e.CodeApogee,
            Nom = e.IdUtilisateurNavigation.Nom,
            Prenom = e.IdUtilisateurNavigation.Prenom,
            Email = e.IdUtilisateurNavigation.Email,
            Niveau = e.Niveau,
            Filiere = e.Filiere,
            Statut = e.IdUtilisateurNavigation.Statut
        };
    }

    public async Task<IEnumerable<EtudiantDto>> GetAllEtudiantsAsync()
    {
        var etudiants = await _etudiantRepo.GetAllAsync();
        return etudiants.Select(e => new EtudiantDto
        {
            IdEtudiant = e.IdEtudiant,
            CodeApogee = e.CodeApogee,
            Nom = e.IdUtilisateurNavigation.Nom,
            Prenom = e.IdUtilisateurNavigation.Prenom,
            Email = e.IdUtilisateurNavigation.Email,
            Niveau = e.Niveau,
            Filiere = e.Filiere,
            Statut = e.IdUtilisateurNavigation.Statut
        });
    }

    public async Task<EtudiantDto> CreateEtudiantAsync(EtudiantCreateDto request)
    {
        // 1. Créer dans Keycloak
        string keycloakUserId;
        string tempPassword = "Password123!"; // À envoyer par email idéalement
        try
        {
            keycloakUserId = await _keycloakService.CreateUserAsync(request.Email, request.Prenom, request.Nom);
            await _keycloakService.SetUserPasswordAsync(keycloakUserId, tempPassword);
            _logger.LogInformation($"Etudiant créé dans Keycloak avec ID: {keycloakUserId}");
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

            // 3. Créer l'étudiant
            var etudiant = new Etudiant
            {
                CodeApogee = request.CodeApogee,
                Niveau = request.Niveau,
                Filiere = request.Filiere,
                IdUtilisateur = utilisateur.IdUtilisateur
            };
            await _etudiantRepo.AddAsync(etudiant);

            return new EtudiantDto
            {
                IdEtudiant = etudiant.IdEtudiant,
                CodeApogee = etudiant.CodeApogee,
                Nom = utilisateur.Nom,
                Prenom = utilisateur.Prenom,
                Email = utilisateur.Email,
                Niveau = etudiant.Niveau,
                Filiere = etudiant.Filiere,
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

    public async Task DeleteEtudiantAsync(int id)
    {
        await _etudiantRepo.DeleteAsync(id);
        // Note: idéalement, il faudrait aussi récupérer l'email pour le supprimer de Keycloak.
    }
}
