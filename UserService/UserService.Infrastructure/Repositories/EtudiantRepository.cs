using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EtudiantRepository : IEtudiantRepository
{
    private readonly UserDbContext _context;

    public EtudiantRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<Etudiant?> GetByIdAsync(int id)
        => await _context.Etudiants
            .Include(e => e.IdUtilisateurNavigation)
            .FirstOrDefaultAsync(e => e.IdEtudiant == id);

    public async Task<Etudiant?> GetByUtilisateurIdAsync(int idUtilisateur)
        => await _context.Etudiants
            .Include(e => e.IdUtilisateurNavigation)
            .FirstOrDefaultAsync(e => e.IdUtilisateur == idUtilisateur);

    public async Task<Etudiant?> GetByCodeApogeeAsync(string codeApogee)
        => await _context.Etudiants
            .Include(e => e.IdUtilisateurNavigation)
            .FirstOrDefaultAsync(e => e.CodeApogee == codeApogee);

    public async Task<IEnumerable<Etudiant>> GetAllAsync()
        => await _context.Etudiants
            .Include(e => e.IdUtilisateurNavigation)
            .ToListAsync();

    public async Task<Etudiant> AddAsync(Etudiant etudiant)
    {
        _context.Etudiants.Add(etudiant);
        await _context.SaveChangesAsync();
        return etudiant;
    }

    public async Task UpdateAsync(Etudiant etudiant)
    {
        _context.Etudiants.Update(etudiant);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var etudiant = await GetByIdAsync(id);
        if (etudiant != null)
        {
            _context.Etudiants.Remove(etudiant);
            await _context.SaveChangesAsync();
        }
    }
}
