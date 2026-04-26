using Domain.Entities;

namespace Application.Interfaces;

public interface IProjetRepository
{
    Task<IEnumerable<Projet>> GetAllAsync();
    Task<IEnumerable<Projet>> GetByEnseignantIdAsync(int idEnseignant);
    Task<Projet?> GetByIdAsync(int id);
    Task AddAsync(Projet projet);
}