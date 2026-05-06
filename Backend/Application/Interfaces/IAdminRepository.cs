// Application/Interfaces/IAdminRepository.cs
namespace Application.Interfaces;
using Domain.Entities;
public interface IAdminRepository
{
    Task<Admin?> GetByIdAsync(int id);
    Task<Admin?> GetByUtilisateurIdAsync(int idUtilisateur);
    Task AddAsync(Admin admin);
    Task UpdateAsync(Admin admin);
}