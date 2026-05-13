// Application/Interfaces/IEnseignantRepository.cs
namespace Application.Interfaces;
using Domain.Entities;
public interface IEnseignantRepository
{
    Task<IEnumerable<Enseignant>> GetAllAsync();
    Task<Enseignant?> GetByIdAsync(int id);
    Task<int> CountAsync();
    Task<Enseignant?> GetByEmailAsync(string email);
    Task AddAsync(Enseignant enseignant);
    Task UpdateAsync(Enseignant enseignant);
    Task DeleteAsync(int id);
}