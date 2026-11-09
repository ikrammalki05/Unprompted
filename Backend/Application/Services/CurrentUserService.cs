using System.Security.Claims;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IUtilisateurRepository _utilisateurRepo;
    private readonly IEtudiantRepository _etudiantRepo;
    private readonly IEnseignantRepository _enseignantRepo;
    private readonly IAdminRepository _adminRepo;

    public CurrentUserService(
        IUtilisateurRepository utilisateurRepo,
        IEtudiantRepository etudiantRepo,
        IEnseignantRepository enseignantRepo,
        IAdminRepository adminRepo)
    {
        _utilisateurRepo = utilisateurRepo;
        _etudiantRepo = etudiantRepo;
        _enseignantRepo = enseignantRepo;
        _adminRepo = adminRepo;
    }

    public string? GetEmail(ClaimsPrincipal user)
    {
        return user.FindFirst("email")?.Value
            ?? user.FindFirst(ClaimTypes.Email)?.Value
            ?? user.FindFirst("preferred_username")?.Value;
    }

    public string? GetSubject(ClaimsPrincipal user)
        => user.FindFirst("sub")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public async Task<Utilisateur?> GetUtilisateurAsync(ClaimsPrincipal user)
    {
        var email = GetEmail(user);
        if (string.IsNullOrWhiteSpace(email)) return null;
        return await _utilisateurRepo.GetByEmailAsync(email);
    }

    public async Task<Etudiant?> GetOrCreateEtudiantAsync(ClaimsPrincipal user)
    {
        var email = GetEmail(user);
        if (string.IsNullOrWhiteSpace(email)) return null;

        var utilisateur = await _utilisateurRepo.GetByEmailAsync(email);
        if (utilisateur == null)
        {
            var given = user.FindFirst("given_name")?.Value
                ?? user.FindFirst(ClaimTypes.GivenName)?.Value
                ?? "Etudiant";
            var family = user.FindFirst("family_name")?.Value
                ?? user.FindFirst(ClaimTypes.Surname)?.Value
                ?? user.Identity?.Name
                ?? "User";

            utilisateur = new Utilisateur
            {
                Nom = family,
                Prenom = given,
                Email = email,
                Statut = "Actif",
                DateCreation = DateTime.UtcNow
            };
            await _utilisateurRepo.AddAsync(utilisateur);
        }

        var all = await _etudiantRepo.GetAllAsync();
        var etudiant = all.FirstOrDefault(e => e.IdUtilisateur == utilisateur.IdUtilisateur);
        if (etudiant != null) return etudiant;

        etudiant = new Etudiant
        {
            IdUtilisateur = utilisateur.IdUtilisateur,
            CodeApogee = $"AUTO-{utilisateur.IdUtilisateur}",
            Niveau = "Non spécifié",
            Filiere = "Non spécifiée"
        };
        await _etudiantRepo.AddAsync(etudiant);
        return await _etudiantRepo.GetByIdAsync(etudiant.IdEtudiant);
    }

    public async Task<Enseignant?> GetOrCreateEnseignantAsync(ClaimsPrincipal user)
    {
        var email = GetEmail(user);
        if (string.IsNullOrWhiteSpace(email)) return null;

        var utilisateur = await _utilisateurRepo.GetByEmailAsync(email);
        if (utilisateur == null)
        {
            var given = user.FindFirst("given_name")?.Value ?? "Enseignant";
            var family = user.FindFirst("family_name")?.Value
                ?? user.Identity?.Name
                ?? "User";

            utilisateur = new Utilisateur
            {
                Nom = family,
                Prenom = given,
                Email = email,
                Statut = "Actif",
                DateCreation = DateTime.UtcNow
            };
            await _utilisateurRepo.AddAsync(utilisateur);
        }

        var all = await _enseignantRepo.GetAllAsync();
        var enseignant = all.FirstOrDefault(e => e.IdUtilisateur == utilisateur.IdUtilisateur);
        if (enseignant != null) return enseignant;

        enseignant = new Enseignant
        {
            IdUtilisateur = utilisateur.IdUtilisateur,
            Specialite = "Informatique",
            Departement = "Génie Informatique"
        };
        await _enseignantRepo.AddAsync(enseignant);
        return await _enseignantRepo.GetByIdAsync(enseignant.IdEnseignant);
    }

    public async Task<Admin?> GetOrCreateAdminAsync(ClaimsPrincipal user)
    {
        var email = GetEmail(user);
        if (string.IsNullOrWhiteSpace(email)) return null;

        var utilisateur = await _utilisateurRepo.GetByEmailAsync(email);
        if (utilisateur == null)
        {
            utilisateur = new Utilisateur
            {
                Nom = user.FindFirst("family_name")?.Value ?? "Admin",
                Prenom = user.FindFirst("given_name")?.Value ?? "Admin",
                Email = email,
                Statut = "Actif",
                DateCreation = DateTime.UtcNow
            };
            await _utilisateurRepo.AddAsync(utilisateur);
        }

        var existing = await _adminRepo.GetByUtilisateurIdAsync(utilisateur.IdUtilisateur);
        if (existing != null) return existing;

        var admin = new Admin { IdUtilisateur = utilisateur.IdUtilisateur };
        await _adminRepo.AddAsync(admin);
        return await _adminRepo.GetByUtilisateurIdAsync(utilisateur.IdUtilisateur);
    }
}
