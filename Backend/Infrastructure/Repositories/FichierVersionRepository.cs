using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FichierVersionRepository : IFichierVersionRepository
{
    private readonly AppDbContext _context;

    public FichierVersionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FichierVersion version)
    {
        await _context.FichierVersions.AddAsync(version);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<FichierVersion>> GetByFichierIdAsync(int fichierId)
    {
        return await _context.FichierVersions
            .Where(v => v.IdFichier == fichierId)
            .OrderByDescending(v => v.CreatedAt)
            .ToListAsync();
    }
}