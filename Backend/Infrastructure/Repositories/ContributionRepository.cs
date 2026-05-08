using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ContributionRepository : IContributionRepository
{
    private readonly AppDbContext _context;

    public ContributionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contribution>> GetByProjetIdAsync(int idProjet)
    {
        return await _context.Contributions
            .Include(c => c.IdEtudiantNavigation)
                .ThenInclude(e => e!.IdUtilisateurNavigation)
            .Where(c => c.IdProjet == idProjet)
            .ToListAsync();
    }

    public async Task<IEnumerable<Contribution>> GetByEtudiantIdAsync(int idEtudiant)
    {
        return await _context.Contributions
            .Include(c => c.IdProjetNavigation) // On charge le projet pour avoir son Titre
            .Where(c => c.IdEtudiant == idEtudiant)
            .OrderByDescending(c => c.DateCommit)
            .ToListAsync();
    }
}