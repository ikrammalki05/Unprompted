using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly AppDbContext _context;

    public AdminRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Admin?> GetByIdAsync(int id)
    {
        return await _context.Admins
            .Include(a => a.IdUtilisateurNavigation)
            .FirstOrDefaultAsync(a => a.IdAdmin == id);
    }

    public async Task<Admin?> GetByUtilisateurIdAsync(int idUtilisateur)
    {
        return await _context.Admins
            .Include(a => a.IdUtilisateurNavigation)
            .FirstOrDefaultAsync(a => a.IdUtilisateur == idUtilisateur);
    }

    public async Task AddAsync(Admin admin)
    {
        await _context.Admins.AddAsync(admin);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Admin admin)
    {
        _context.Admins.Update(admin);
        await _context.SaveChangesAsync();
    }
}