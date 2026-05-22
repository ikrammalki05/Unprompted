using System.Collections.Generic;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Domain.Entities;

namespace Unprompted.Services.Projet.Application.Interfaces;

public interface IProjetRepository
{
    Task<Domain.Entities.Projet?> GetByIdAsync(int id);
    Task<IEnumerable<Domain.Entities.Projet>> GetAllAsync();
    Task<IEnumerable<Domain.Entities.Projet>> GetByEnseignantIdAsync(int enseignantId);
    Task AddAsync(Domain.Entities.Projet projet);
    void Update(Domain.Entities.Projet projet);
    void Delete(Domain.Entities.Projet projet);
    Task<bool> SaveChangesAsync();
}