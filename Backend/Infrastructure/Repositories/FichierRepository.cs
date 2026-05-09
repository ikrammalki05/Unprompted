// Infrastructure/Repositories/FichierRepository.cs

using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FichierRepository : IFichierRepository
{
    private readonly AppDbContext _context;

    public FichierRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Fichier>> GetByProjectIdAsync(int projectId)
    {
        return await _context.Fichiers
            .Where(f => f.IdProjet == projectId)
            .ToListAsync();
    }

    public async Task<Fichier?> GetByIdAsync(int id)
    {
        return await _context.Fichiers
            .FirstOrDefaultAsync(f => f.IdFichier == id);
    }

    public async Task<bool> ExistsAsync(string nom, int? dossierId, int projectId)
    {
        return await _context.Fichiers.AnyAsync(f =>
            f.Nom == nom &&
            f.IdDossier == dossierId &&
            f.IdProjet == projectId);
    }

    public async Task AddAsync(Fichier fichier)
    {
        await _context.Fichiers.AddAsync(fichier);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Fichier fichier)
    {
        _context.Fichiers.Update(fichier);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var fichier = await GetByIdAsync(id);
        if (fichier != null)
        {
            _context.Fichiers.Remove(fichier);
            await _context.SaveChangesAsync();
        }
    }
}