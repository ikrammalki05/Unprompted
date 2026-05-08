using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EtudiantRepository : IEtudiantRepository
{
    private readonly AppDbContext _context;

    public EtudiantRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Etudiant>> GetAllAsync()
    {
        return await _context.Etudiants
            .Include(e => e.IdUtilisateurNavigation)
            .ToListAsync();
    }

    public async Task<Etudiant?> GetByIdAsync(int id)
    {
        return await _context.Etudiants
            .Include(e => e.IdUtilisateurNavigation)
            .FirstOrDefaultAsync(e => e.IdEtudiant == id);
    }

    public async Task<IEnumerable<Etudiant>> GetByFiliereAsync(string filiere)
    {
        return await _context.Etudiants
            .Include(e => e.IdUtilisateurNavigation)
            .Where(e => e.Filiere == filiere)
            .ToListAsync();
    }

    public async Task<IEnumerable<Etudiant>> GetByStatutAsync(string statut)
    {
        return await _context.Etudiants
            .Include(e => e.IdUtilisateurNavigation)
            .Where(e => e.IdUtilisateurNavigation.Statut == statut)
            .ToListAsync();
    }

    public async Task<int> CountAsync()
    {
        return await _context.Etudiants.CountAsync();
    }

    public async Task AddAsync(Etudiant etudiant)
    {
        await _context.Etudiants.AddAsync(etudiant);
        await _context.SaveChangesAsync();
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