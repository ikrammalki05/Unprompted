using Domain.Entities;

namespace Application.Interfaces;

public interface IContributionRepository
{
    Task<IEnumerable<Contribution>> GetByProjetIdAsync(int idProjet);
}