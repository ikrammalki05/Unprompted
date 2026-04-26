using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories;

public class GroupeRepository : IGroupeRepository
{
    private readonly AppDbContext _context;

    public GroupeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Groupe> AddAsync(Groupe groupe)
    {
        await _context.Groupes.AddAsync(groupe);
        await _context.SaveChangesAsync();
        return groupe; // Retourne le groupe avec son nouvel ID généré par la DB
    }
}