using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EnseignantRepository : IEnseignantRepository
{
    private readonly AppDbContext _context;

    public EnseignantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Enseignant>> GetAllAsync()
    {
        return await _context.Enseignants
            .Include(e => e.IdUtilisateurNavigation)
            .ToListAsync();
    }

    public async Task<Enseignant?> GetByIdAsync(int id)
    {
        return await _context.Enseignants
            .Include(e => e.IdUtilisateurNavigation)
            .FirstOrDefaultAsync(e => e.IdEnseignant == id);
    }

    public async Task<IEnumerable<Enseignant>> GetBySpecialiteAsync(string specialite)
    {
        return await _context.Enseignants
            .Include(e => e.IdUtilisateurNavigation)
            .Where(e => e.Specialite == specialite)
            .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Enseignants.CountAsync();
    }

    public async Task AddAsync(Enseignant enseignant)
    {
        await _context.Enseignants.AddAsync(enseignant);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Enseignant enseignant)
    {
        _context.Enseignants.Update(enseignant);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var enseignant = await GetByIdAsync(id);
        if (enseignant != null)
        {
            _context.Enseignants.Remove(enseignant);
            await _context.SaveChangesAsync();
        }
    }
}