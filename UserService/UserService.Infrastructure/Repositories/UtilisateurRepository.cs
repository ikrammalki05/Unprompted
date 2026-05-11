using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UtilisateurRepository : IUtilisateurRepository
{
    private readonly UserDbContext _context;

    public UtilisateurRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<Utilisateur?> GetByIdAsync(int id)
        => await _context.Utilisateurs.FindAsync(id);

    public async Task<Utilisateur?> GetByEmailAsync(string email)
        => await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<IEnumerable<Utilisateur>> GetAllAsync()
        => await _context.Utilisateurs.ToListAsync();

    public async Task<Utilisateur> AddAsync(Utilisateur utilisateur)
    {
        _context.Utilisateurs.Add(utilisateur);
        await _context.SaveChangesAsync();
        return utilisateur;
    }

    public async Task UpdateAsync(Utilisateur utilisateur)
    {
        _context.Utilisateurs.Update(utilisateur);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var utilisateur = await GetByIdAsync(id);
        if (utilisateur != null)
        {
            _context.Utilisateurs.Remove(utilisateur);
            await _context.SaveChangesAsync();
        }
    }
}
