namespace Application.Interfaces;

public interface IKeycloakAdminService
{
    /// <summary>
    /// Crée un utilisateur dans Keycloak
    /// </summary>
    /// <param name="email">Email de l'utilisateur</param>
    /// <param name="firstName">Prénom</param>
    /// <param name="lastName">Nom de famille</param>
    /// <returns>ID de l'utilisateur créé dans Keycloak</returns>
    Task<string> CreateUserAsync(string email, string firstName, string lastName);

    /// <summary>
    /// Définit le mot de passe d'un utilisateur Keycloak
    /// </summary>
    /// <param name="userId">ID Keycloak de l'utilisateur</param>
    /// <param name="password">Mot de passe à définir</param>
    Task SetUserPasswordAsync(string userId, string password);

    /// <summary>
    /// Supprime un utilisateur de Keycloak
    /// </summary>
    /// <param name="userId">ID Keycloak de l'utilisateur</param>
    Task DeleteUserAsync(string userId);
}
