using Domain.Entities;

namespace Application.Interfaces;

public interface IClasseRepository
{
    Task<Classe?> GetByIdAsync(int id);
    Task<IEnumerable<Classe>> GetAllAsync();
    Task<Classe> AddAsync(Classe classe);
    Task UpdateAsync(Classe classe);
    Task DeleteAsync(int id);
    Task AddEnseignantToClasseAsync(int idClasse, int idEnseignant);
}
