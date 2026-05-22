using System.Collections.Generic;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Domain.Entities;

namespace Unprompted.Services.Projet.Application.Interfaces;

public interface IAffectationRepository
{
    Task<IEnumerable<Affectation>> GetByGroupeIdAsync(int groupeId);
    Task<IEnumerable<Affectation>> GetByEtudiantIdAsync(int etudiantId);
    Task AddAsync(Affectation affectation);
    void Remove(Affectation affectation);
    Task<bool> SaveChangesAsync();
}