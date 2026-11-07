using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SessionTerminalRepository
    : ISessionTerminalRepository
{
    private readonly AppDbContext _context;

    public SessionTerminalRepository(
        AppDbContext context)
    {
        _context = context;
    }

    public async Task AjouterAsync(
        SessionTerminal session)
    {
        await _context.SessionTerminals
            .AddAsync(session);

        await _context.SaveChangesAsync();
    }

    public async Task<SessionTerminal?>
        ObtenirParIdAsync(Guid idSession)
    {
        return await _context.SessionTerminals
            .FirstOrDefaultAsync(
                s => s.IdSession == idSession);
    }

    public async Task MettreAJourAsync(
        SessionTerminal session)
    {
        _context.SessionTerminals.Update(session);

        await _context.SaveChangesAsync();
    }
}