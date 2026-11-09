using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces;

public interface ICurrentUserService
{
    string? GetEmail(ClaimsPrincipal user);
    string? GetSubject(ClaimsPrincipal user);
    Task<Utilisateur?> GetUtilisateurAsync(ClaimsPrincipal user);
    Task<Etudiant?> GetOrCreateEtudiantAsync(ClaimsPrincipal user);
    Task<Enseignant?> GetOrCreateEnseignantAsync(ClaimsPrincipal user);
    Task<Admin?> GetOrCreateAdminAsync(ClaimsPrincipal user);
}
