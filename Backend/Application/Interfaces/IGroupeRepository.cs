using Domain.Entities;

namespace Application.Interfaces;

public interface IGroupeRepository
{
    Task<IEnumerable<Groupe>> GetAllAsync();
    Task<Groupe?> GetByIdAsync(int id);
    Task AddAsync(Groupe groupe);
    Task UpdateAsync(Groupe groupe);
    Task DeleteAsync(int id);
    Task AddAffectationAsync(Affectation affectation);
    Task RemoveAffectationAsync(int idEtudiant, int idGroupe);
    Task RemoveAllAffectationsForGroupeAsync(int idGroupe);
}
