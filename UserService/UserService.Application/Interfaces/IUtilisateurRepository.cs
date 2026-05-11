using Domain.Entities;

namespace Application.Interfaces;

public interface IUtilisateurRepository
{
    Task<Utilisateur?> GetByIdAsync(int id);
    Task<Utilisateur?> GetByEmailAsync(string email);
    Task<IEnumerable<Utilisateur>> GetAllAsync();
    Task<Utilisateur> AddAsync(Utilisateur utilisateur);
    Task UpdateAsync(Utilisateur utilisateur);
    Task DeleteAsync(int id);
}
