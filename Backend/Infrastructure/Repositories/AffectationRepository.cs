using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class AffectationRepository : IAffectationRepository
{
    private readonly AppDbContext _context;

    public AffectationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Affectation affectation)
    {
        await _context.Affectations.AddAsync(affectation);
        await _context.SaveChangesAsync();
    }
}