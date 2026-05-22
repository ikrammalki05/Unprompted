using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Application.Interfaces;
using Unprompted.Services.Projet.Domain.Entities;
using Unprompted.Services.Projet.Infrastructure.Data;

namespace Unprompted.Services.Projet.Infrastructure.Repositories;

public class AffectationRepository : IAffectationRepository
{
    private readonly ProjetDbContext _context;

    public AffectationRepository(ProjetDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Affectation>> GetByGroupeIdAsync(int groupeId)
    {
        return await _context.Affectations
            .Where(a => a.IdGroupe == groupeId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Affectation>> GetByEtudiantIdAsync(int etudiantId)
    {
        return await _context.Affectations
            .Where(a => a.IdEtudiant == etudiantId)
            .ToListAsync();
    }

    public async Task AddAsync(Affectation affectation)
    {
        await _context.Affectations.AddAsync(affectation);
    }

    public void Remove(Affectation affectation)
    {
        _context.Affectations.Remove(affectation);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync()) > 0;
    }
}