using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClasseRepository : IClasseRepository
{
    private readonly UserDbContext _context;

    public ClasseRepository(UserDbContext context)
    {
        _context = context;
    }

    public async Task<Classe?> GetByIdAsync(int id)
        => await _context.Classes.FindAsync(id);

    public async Task<IEnumerable<Classe>> GetAllAsync()
        => await _context.Classes.ToListAsync();

    public async Task<Classe> AddAsync(Classe classe)
    {
        _context.Classes.Add(classe);
        await _context.SaveChangesAsync();
        return classe;
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

    public async Task AddEnseignantToClasseAsync(int idClasse, int idEnseignant)
    {
        var enseignantClasse = new EnseignantClasse
        {
            IdClasse = idClasse,
            IdEnseignant = idEnseignant,
            DateAffectation = DateTime.UtcNow
        };
        _context.EnseignantClasses.Add(enseignantClasse);
        await _context.SaveChangesAsync();
    }
}
