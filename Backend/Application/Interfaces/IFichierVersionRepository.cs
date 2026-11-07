using Domain.Entities;

namespace Application.Interfaces;

public interface IFichierVersionRepository
{
    Task AddAsync(FichierVersion version);
    Task<IEnumerable<FichierVersion>> GetByFichierIdAsync(int fichierId);
}