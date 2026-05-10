using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ConfigurationIumRepository : IConfigurationIumRepository
{
    private readonly AppDbContext _context;

    public ConfigurationIumRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ConfigurationIum?> GetByProjetIdAsync(int idProjet)
    {
        return await _context.ConfigurationIa
            .FirstOrDefaultAsync(c => c.IdProjet == idProjet);
    }

    public async Task AddAsync(ConfigurationIum config)
    {
        await _context.ConfigurationIa.AddAsync(config);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ConfigurationIum config)
    {
        _context.ConfigurationIa.Update(config);
        await _context.SaveChangesAsync();
    }
}