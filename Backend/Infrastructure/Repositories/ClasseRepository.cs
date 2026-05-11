using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClasseRepository : IClasseRepository
{
    private readonly AppDbContext _context;

    public ClasseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Classe>> GetAllAsync()
    {
        return await _context.Classes.ToListAsync();
    }

    public async Task<Classe?> GetByIdAsync(int id)
    {
        return await _context.Classes.FindAsync(id);
    }

    public async Task<int> CountAsync()
    {
        return await _context.Classes.CountAsync();
    }

    public async Task AddAsync(Classe classe)
    {
        await _context.Classes.AddAsync(classe);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Classe classe)
    {
        _context.Classes.Update(classe);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var classe = await GetByIdAsync(id);
        if (classe != null)
        {
            _context.Classes.Remove(classe);
            await _context.SaveChangesAsync();
        }
    }
}