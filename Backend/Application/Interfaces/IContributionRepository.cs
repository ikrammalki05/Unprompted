using Domain.Entities;

namespace Application.Interfaces;

public interface IContributionRepository
{
    Task<IEnumerable<Contribution>> GetByProjetIdAsync(int idProjet);
    Task<IEnumerable<Contribution>> GetByEtudiantIdAsync(int idEtudiant);
}