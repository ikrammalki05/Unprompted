using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/terminal")]
public class TerminalController : ControllerBase
{
    private readonly ITerminalService _service;

    public TerminalController(ITerminalService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Creer()
    {
        var userId =
            User.FindFirst("sub")?.Value ?? "system";

        var result =
            await _service.CreerSessionAsync(userId);

        return Ok(result);
    }

    [HttpPost("{idSession}/commande")]
    public async Task<IActionResult> Commande(
        Guid idSession,
        CommandeTerminalDto dto)
    {
        var result =
            await _service.ExecuterCommandeAsync(
                idSession,
                dto.Commande);

        return Ok(result);
    }

    [HttpDelete("{idSession}")]
    public async Task<IActionResult> Fermer(
        Guid idSession)
    {
        await _service.FermerSessionAsync(idSession);

        return Ok();
    }
}