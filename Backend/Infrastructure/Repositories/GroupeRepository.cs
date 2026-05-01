using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GroupeRepository : IGroupeRepository
{
    private readonly AppDbContext _context;

    public GroupeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Groupe>> GetAllAsync()
        => await _context.Groupes
            .Include(g => g.Affectations)
                .ThenInclude(a => a.IdEtudiantNavigation)
                    .ThenInclude(e => e.IdUtilisateurNavigation)
            .Include(g => g.Affectations)
                .ThenInclude(a => a.IdRoleNavigation)
            .ToListAsync();

    public async Task<Groupe?> GetByIdAsync(int id)
        => await _context.Groupes
            .Include(g => g.Affectations)
                .ThenInclude(a => a.IdEtudiantNavigation)
                    .ThenInclude(e => e.IdUtilisateurNavigation)
            .Include(g => g.Affectations)
                .ThenInclude(a => a.IdRoleNavigation)
            .FirstOrDefaultAsync(g => g.IdGroupe == id);

    public async Task AddAsync(Groupe groupe)
    {
        await _context.Groupes.AddAsync(groupe);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Groupe groupe)
    {
        _context.Groupes.Update(groupe);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var groupe = await _context.Groupes.FindAsync(id);
        if (groupe != null)
        {
            _context.Groupes.Remove(groupe);
            await _context.SaveChangesAsync();
        }
    }

    public async Task AddAffectationAsync(Affectation affectation)
    {
        await _context.Affectations.AddAsync(affectation);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAffectationAsync(int idEtudiant, int idGroupe)
    {
        var affectation = await _context.Affectations
            .FirstOrDefaultAsync(a => a.IdEtudiant == idEtudiant && a.IdGroupe == idGroupe);
        if (affectation != null)
        {
            _context.Affectations.Remove(affectation);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveAllAffectationsForGroupeAsync(int idGroupe)
    {
        var affectations = await _context.Affectations
            .Where(a => a.IdGroupe == idGroupe)
            .ToListAsync();
        
        if (affectations.Any())
        {
            _context.Affectations.RemoveRange(affectations);
            await _context.SaveChangesAsync();
        }
    }
}
