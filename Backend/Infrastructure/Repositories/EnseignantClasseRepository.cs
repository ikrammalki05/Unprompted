using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class EnseignantClasseRepository : IEnseignantClasseRepository
{
    private readonly AppDbContext _context;

    public EnseignantClasseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(EnseignantClasse enseignantClasse)
    {
        await _context.EnseignantClasses.AddAsync(enseignantClasse);
        await _context.SaveChangesAsync();
    }
}