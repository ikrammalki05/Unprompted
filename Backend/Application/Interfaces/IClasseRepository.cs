// Application/Interfaces/IClasseRepository.cs
namespace Application.Interfaces;
using Domain.Entities;
public interface IClasseRepository
{
    Task<IEnumerable<Groupe>> GetAllAsync();
    Task<Groupe?> GetByIdAsync(int id);
    Task<int> CountAsync();
    Task AddAsync(Groupe groupe);
    Task UpdateAsync(Groupe groupe);
    Task DeleteAsync(int id);
}