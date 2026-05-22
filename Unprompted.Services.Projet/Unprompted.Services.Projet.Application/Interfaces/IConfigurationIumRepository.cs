using System.Threading.Tasks;
using Unprompted.Services.Projet.Domain.Entities;

namespace Unprompted.Services.Projet.Application.Interfaces;

public interface IConfigurationIumRepository
{
    Task<ConfigurationIum?> GetByProjetIdAsync(int projetId);
}