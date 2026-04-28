using Application.DTOs;
using Application.Interfaces;
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

    public async Task<EtudiantDto> CreateEtudiantAsync(EtudiantCreateDto request)
    {
        // 1. Vérifier si l'email existe déjà
        var existingUser = await _utilisateurRepo.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new Exception("Un utilisateur avec cet email existe déjà.");

        // 2. Créer l'utilisateur dans Keycloak d'abord
        string keycloakUserId = string.Empty;
        try
        {
            keycloakUserId = await _keycloakService.CreateUserAsync(request.Email, request.Prenom, request.Nom);

            // Mot de passe temporaire : Prenom + CodeApogee (ex: Mohamed12345)
            var tempPassword = $"{request.Prenom}{request.CodeApogee}";
            await _keycloakService.SetUserPasswordAsync(keycloakUserId, tempPassword);

            _logger.LogInformation($"Etudiant créé dans Keycloak avec ID: {keycloakUserId}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Échec de la création dans Keycloak: {ex.Message}");
            throw new Exception($"Impossible de créer l'utilisateur dans Keycloak: {ex.Message}");
        }

        // 3. Créer l'Utilisateur en base de données
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

        // 4. Créer le profil Etudiant lié
        var nouvelEtudiant = new Domain.Entities.Etudiant
        {
            IdUtilisateur = nouvelUtilisateur.IdUtilisateur,
            CodeApogee = request.CodeApogee
        };
        await _etudiantRepo.AddAsync(nouvelEtudiant);

        // 5. Retourner le DTO pour le Frontend
        return new EtudiantDto
        {
            Id = nouvelEtudiant.IdEtudiant,
            NomComplet = $"{nouvelUtilisateur.Prenom} {nouvelUtilisateur.Nom}",
            Email = nouvelUtilisateur.Email,
            CodeApogee = nouvelEtudiant.CodeApogee,
            Statut = nouvelUtilisateur.Statut,
            ClasseNom = "Non assigné"
        };
    }


    public async Task<IEnumerable<EtudiantDto>> GetAllEtudiantsAsync()
    {
        var etudiants = await _etudiantRepo.GetAllAsync();
        
        return etudiants.Select(e => new EtudiantDto
        {
            Id = e.IdEtudiant,
            CodeApogee = e.CodeApogee,
            NomComplet = $"{e.IdUtilisateurNavigation?.Prenom} {e.IdUtilisateurNavigation?.Nom}",
            Email = e.IdUtilisateurNavigation?.Email ?? "Email inconnu",
            Statut = e.IdUtilisateurNavigation?.Statut ?? "Inactif",
            ClasseNom = e.Affectations?.FirstOrDefault()?.IdGroupeNavigation?.NomGroupe ?? "Non assigné"
        });
    }

    public async Task UpdateEtudiantAsync(int id, EtudiantCreateDto request)
    {
        var etudiant = await _etudiantRepo.GetByIdAsync(id);
        if (etudiant == null)
            throw new ArgumentException($"Etudiant avec l'id {id} introuvable.");

        var utilisateur = await _utilisateurRepo.GetByIdAsync(etudiant.IdUtilisateur);
        if (utilisateur == null)
            throw new ArgumentException("Utilisateur introuvable.");

        utilisateur.Nom = request.Nom;
        utilisateur.Prenom = request.Prenom;
        utilisateur.Email = request.Email;
        etudiant.CodeApogee = request.CodeApogee;

        await _utilisateurRepo.UpdateAsync(utilisateur);
        await _etudiantRepo.UpdateAsync(etudiant);
    }

    public async Task DeleteEtudiantAsync(int id)
    {
        var etudiant = await _etudiantRepo.GetByIdAsync(id);
        if (etudiant == null)
            throw new ArgumentException($"Etudiant avec l'id {id} introuvable.");

        await _etudiantRepo.DeleteAsync(id);
    }
}