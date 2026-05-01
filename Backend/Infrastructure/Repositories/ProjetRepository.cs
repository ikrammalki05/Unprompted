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
        => await _context.Projets
            .Include(p => p.IdEnseignantNavigation)
                .ThenInclude(e => e.IdUtilisateurNavigation)
            .ToListAsync();

    public async Task<Projet?> GetByIdAsync(int id)
        => await _context.Projets
            .Include(p => p.IdEnseignantNavigation)
                .ThenInclude(e => e.IdUtilisateurNavigation)
            .Include(p => p.Groupes)
            .FirstOrDefaultAsync(p => p.IdProjet == id);

    public async Task<IEnumerable<Projet>> GetByEnseignantIdAsync(int idEnseignant)
        => await _context.Projets
            .Where(p => p.IdEnseignant == idEnseignant)
            .Include(p => p.Groupes)
            .ToListAsync();

    public async Task AddAsync(Projet projet)
    {
        await _context.Projets.AddAsync(projet);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Projet projet)
    {
        _context.Projets.Update(projet);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var projet = await GetByIdAsync(id);
        if (projet != null)
        {
            _context.Projets.Remove(projet);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Contribution>> GetContributionsByProjectIdAsync(int idProjet)
        => await _context.Contributions
            .Where(c => c.IdProjet == idProjet)
            .OrderByDescending(c => c.DateCommit)
            .ToListAsync();

    public async Task AddContributionAsync(Contribution contribution)
    {
        await _context.Contributions.AddAsync(contribution);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync()
        => await _context.Projets.CountAsync();

    public async Task AssignProjectToGroupAsync(int idProjet, int idGroupe)
    {
        var groupe = await _context.Groupes.FindAsync(idGroupe);
        if (groupe != null)
        {
            groupe.IdProjet = idProjet;
            await _context.SaveChangesAsync();
        }
    }
}