// Application/Interfaces/IUtilisateurRepository.cs
namespace Application.Interfaces;
using Domain.Entities;
public interface IUtilisateurRepository
{
    Task<IEnumerable<Utilisateur>> GetAllAsync();
    Task<Utilisateur?> GetByIdAsync(int id);
    Task<Utilisateur?> GetByEmailAsync(string email);
    Task AddAsync(Utilisateur utilisateur);
    Task UpdateAsync(Utilisateur utilisateur);
    Task DeleteAsync(int id);
}