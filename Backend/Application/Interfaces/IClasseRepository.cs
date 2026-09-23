// Application/Interfaces/IClasseRepository.cs
namespace Application.Interfaces;
using Domain.Entities;
public interface IClasseRepository
{
    Task<IEnumerable<Classe>> GetAllAsync();
    Task<Classe?> GetByIdAsync(int id);
    Task<int> CountAsync();
    Task AddAsync(Classe classe);
    Task UpdateAsync(Classe classe);
    Task DeleteAsync(int id);
}

