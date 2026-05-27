using Domain.Entities;

namespace Application.Interfaces;

public interface IConfigurationIumRepository
{
    Task<ConfigurationIum?> GetByProjetIdAsync(int idProjet);
    Task AddAsync(ConfigurationIum config);
    Task UpdateAsync(ConfigurationIum config);
}