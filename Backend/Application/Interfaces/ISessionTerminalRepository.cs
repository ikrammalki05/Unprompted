using Domain.Entities;

namespace Application.Interfaces;

public interface ISessionTerminalRepository
{
    Task AjouterAsync(SessionTerminal session);

    Task<SessionTerminal?> ObtenirParIdAsync(
        Guid idSession);

    Task MettreAJourAsync(
        SessionTerminal session);
}