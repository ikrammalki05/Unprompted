using Application.DTOs;

namespace Application.Interfaces;

public interface ITerminalService
{
    Task<SessionTerminalDto> CreerSessionAsync(
        string idUtilisateur);

    Task<ResultatTerminalDto>
        ExecuterCommandeAsync(
            Guid idSession,
            string commande);

    Task FermerSessionAsync(Guid idSession);
}