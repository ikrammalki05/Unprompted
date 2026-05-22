using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Application.Interfaces;
using Unprompted.Services.Projet.Domain.Entities;
using Unprompted.Services.Projet.Infrastructure.Data;

namespace Unprompted.Services.Projet.Infrastructure.Repositories;

public class ProjetRepository : IProjetRepository
{
    private readonly ProjetDbContext _context;

    public ProjetRepository(ProjetDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Projet?> GetByIdAsync(int id)
    {
        return await _context.Projets
            .Include(p => p.ConfigurationIum)
            .Include(p => p.Groupes)
            .FirstOrDefaultAsync(p => p.IdProjet == id);
    }

    public async Task<IEnumerable<Domain.Entities.Projet>> GetAllAsync()
    {
        return await _context.Projets.ToListAsync();
    }

    public async Task<IEnumerable<Domain.Entities.Projet>> GetByEnseignantIdAsync(int enseignantId)
    {
        return await _context.Projets
            .Where(p => p.IdEnseignant == enseignantId)
            .ToListAsync();
    }

    public async Task AddAsync(Domain.Entities.Projet projet)
    {
        await _context.Projets.AddAsync(projet);
    }

    public void Update(Domain.Entities.Projet projet)
    {
        _context.Projets.Update(projet);
    }

    public void Delete(Domain.Entities.Projet projet)
    {
        _context.Projets.Remove(projet);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync()) > 0;
    }
}