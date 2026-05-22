using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Application.Interfaces;
using Unprompted.Services.Projet.Domain.Entities;
using Unprompted.Services.Projet.Infrastructure.Data;

namespace Unprompted.Services.Projet.Infrastructure.Repositories;

public class GroupeRepository : IGroupeRepository
{
    private readonly ProjetDbContext _context;

    public GroupeRepository(ProjetDbContext context)
    {
        _context = context;
    }

    public async Task<Groupe?> GetByIdAsync(int id)
    {
        return await _context.Groupes
            .Include(g => g.Affectations)
            .FirstOrDefaultAsync(g => g.IdGroupe == id);
    }

    public async Task<IEnumerable<Groupe>> GetByProjetIdAsync(int projetId)
    {
        return await _context.Groupes
            .Where(g => g.IdProjet == projetId)
            .Include(g => g.Affectations)
            .ToListAsync();
    }

    public async Task AddAsync(Groupe groupe)
    {
        await _context.Groupes.AddAsync(groupe);
    }

    public void Delete(Groupe groupe)
    {
        _context.Groupes.Remove(groupe);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync()) > 0;
    }
}