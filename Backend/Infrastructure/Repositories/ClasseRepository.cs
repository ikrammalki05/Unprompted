using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClasseRepository : IClasseRepository
{
    private readonly AppDbContext _context;

    public ClasseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Groupe>> GetAllAsync()
    {
        return await _context.Groupes
            .Include(g => g.IdProjetNavigation)
            .ToListAsync();
    }

    public async Task<Groupe?> GetByIdAsync(int id)
    {
        return await _context.Groupes
            .Include(g => g.IdProjetNavigation)
            .Include(g => g.Affectations)
            .FirstOrDefaultAsync(g => g.IdGroupe == id);
    }

    public async Task<IEnumerable<Groupe>> GetByProjetAsync(int idProjet)
    {
        return await _context.Groupes
            .Where(g => g.IdProjet == idProjet)
            .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Groupes.CountAsync();
    }

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
        var groupe = await GetByIdAsync(id);
        if (groupe != null)
        {
            _context.Groupes.Remove(groupe);
            await _context.SaveChangesAsync();
        }
    }
}