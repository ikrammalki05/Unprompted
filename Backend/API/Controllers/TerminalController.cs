using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/terminal")]
[Authorize(Roles = "etudiant,Etudiant")]
public class TerminalController : ControllerBase
{
    private readonly ITerminalService _terminalService;
    private readonly ICurrentUserService _currentUser;

    public TerminalController(ITerminalService terminalService, ICurrentUserService currentUser)
    {
        _terminalService = terminalService;
        _currentUser = currentUser;
    }

    [HttpPost("session")]
    public async Task<ActionResult<SessionTerminalDto>> CreateSession([FromQuery] int? idProjet = null)
    {
        var userId = _currentUser.GetSubject(User) ?? _currentUser.GetEmail(User) ?? "anonymous";
        var session = await _terminalService.CreerSessionAsync(userId, idProjet);
        return Ok(session);
    }

    [HttpPost("{idSession:guid}/command")]
    public async Task<ActionResult<ResultatTerminalDto>> RunCommand(Guid idSession, [FromBody] CommandeTerminalDto dto)
    {
        try
        {
            var result = await _terminalService.ExecuterCommandeAsync(idSession, dto.Commande);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{idSession:guid}")]
    public async Task<IActionResult> Close(Guid idSession)
    {
        await _terminalService.FermerSessionAsync(idSession);
        return NoContent();
    }
}
