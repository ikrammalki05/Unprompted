using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class GroupeService : IGroupeService
{
    private readonly AppDbContext _context;

    // Injection du DbContext fourni par l'architecte
    public GroupeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GroupeDto>> GetAllGroupesAsync()
    {
        return await _context.Groupes
            .Include(g => g.IdProjetNavigation) // On charge le projet lié
            .Select(g => new GroupeDto
            {
                IdGroupe = g.IdGroupe,
                NomGroupe = g.NomGroupe,
                IdProjet = g.IdProjet,
                NomProjet = g.IdProjetNavigation.Titre
            })
            .ToListAsync();
    }

    public async Task<GroupeDto?> GetGroupeByIdAsync(int id)
    {
        return await _context.Groupes
            .Include(g => g.IdProjetNavigation)
            .Where(g => g.IdGroupe == id)
            .Select(g => new GroupeDto
            {
                IdGroupe = g.IdGroupe,
                NomGroupe = g.NomGroupe,
                IdProjet = g.IdProjet,
                NomProjet = g.IdProjetNavigation.Titre
            })
            .FirstOrDefaultAsync();
    }
}