using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DossierRepository : IDossierRepository
{
    private readonly AppDbContext _context;

    public DossierRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Dossier>> GetByProjectIdAsync(int projectId)
    {
        return await _context.Dossiers
            .Include(d => d.Fichiers)
            .Include(d => d.Dossiersfils)
                .ThenInclude(sd => sd.Fichiers)
            .Where(d => d.IdProjet == projectId)
            .ToListAsync();
    }

    public async Task<Dossier?> GetByIdAsync(int id)
    {
        return await _context.Dossiers
            .Include(d => d.Dossiersfils)
                .ThenInclude(sd => sd.Fichiers)
            .Include(d => d.Fichiers)
            .FirstOrDefaultAsync(d => d.IdDossier == id);
    }

    public async Task<bool> ExistsAsync(string nom, int? parentId, int projectId)
    {
        return await _context.Dossiers.AnyAsync(d =>
            d.Nom == nom &&
            d.DossierParentId == parentId &&
            d.IdProjet == projectId);
    }

    public async Task AddAsync(Dossier dossier)
    {
        await _context.Dossiers.AddAsync(dossier);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var dossier = await GetByIdAsync(id);
        if (dossier != null)
        {
            _context.Dossiers.Remove(dossier);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Dossier dossier)
    {
        _context.Dossiers.Update(dossier);
        await _context.SaveChangesAsync();
    }
}