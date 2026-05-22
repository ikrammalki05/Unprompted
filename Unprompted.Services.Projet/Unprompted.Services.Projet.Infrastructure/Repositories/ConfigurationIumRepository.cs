using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Application.Interfaces;
using Unprompted.Services.Projet.Domain.Entities;
using Unprompted.Services.Projet.Infrastructure.Data;

namespace Unprompted.Services.Projet.Infrastructure.Repositories;

public class ConfigurationIumRepository : IConfigurationIumRepository
{
    private readonly ProjetDbContext _context;

    public ConfigurationIumRepository(ProjetDbContext context)
    {
        _context = context;
    }

    public async Task<ConfigurationIum?> GetByProjetIdAsync(int projetId)
    {
        return await _context.ConfigurationsIum
            .FirstOrDefaultAsync(c => c.IdProjet == projetId);
    }
}