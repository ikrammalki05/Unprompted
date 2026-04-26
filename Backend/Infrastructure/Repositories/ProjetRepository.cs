using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProjetRepository : IProjetRepository
{
    private readonly AppDbContext _context;

    public ProjetRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Projet>> GetAllAsync()
    {
        return await _context.Projets
            .Include(p => p.IdEnseignantNavigation)
                .ThenInclude(e => e!.IdUtilisateurNavigation) // Pour récupérer le nom du prof
            .ToListAsync();
    }

    public async Task<IEnumerable<Projet>> GetByEnseignantIdAsync(int idEnseignant)
    {
        return await _context.Projets
            .Include(p => p.IdEnseignantNavigation)
                .ThenInclude(e => e!.IdUtilisateurNavigation)
            .Where(p => p.IdEnseignant == idEnseignant)
            .ToListAsync();
    }

    public async Task<Projet?> GetByIdAsync(int id)
    {
        return await _context.Projets
            .Include(p => p.IdEnseignantNavigation)
            .FirstOrDefaultAsync(p => p.IdProjet == id);
    }

    public async Task AddAsync(Projet projet)
    {
        await _context.Projets.AddAsync(projet);
        await _context.SaveChangesAsync();
    }
}