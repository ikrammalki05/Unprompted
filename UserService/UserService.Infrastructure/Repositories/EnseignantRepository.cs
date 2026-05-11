using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EnseignantRepository : IEnseignantRepository
{
    private readonly UserDbContext _context;

    public EnseignantRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<Enseignant?> GetByIdAsync(int id)
        => await _context.Enseignants
            .Include(e => e.IdUtilisateurNavigation)
            .FirstOrDefaultAsync(e => e.IdEnseignant == id);

    public async Task<Enseignant?> GetByUtilisateurIdAsync(int idUtilisateur)
        => await _context.Enseignants
            .Include(e => e.IdUtilisateurNavigation)
            .FirstOrDefaultAsync(e => e.IdUtilisateur == idUtilisateur);

    public async Task<IEnumerable<Enseignant>> GetAllAsync()
        => await _context.Enseignants
            .Include(e => e.IdUtilisateurNavigation)
            .ToListAsync();

    public async Task<Enseignant> AddAsync(Enseignant enseignant)
    {
        _context.Enseignants.Add(enseignant);
        await _context.SaveChangesAsync();
        return enseignant;
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
