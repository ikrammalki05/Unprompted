using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/ai")]
[Authorize]
public class AiController : ControllerBase
{
    private readonly IAiGatewayService _aiGateway;
    private readonly ICurrentUserService _currentUser;

    public AiController(IAiGatewayService aiGateway, ICurrentUserService currentUser)
    {
        _aiGateway = aiGateway;
        _currentUser = currentUser;
    }

    [HttpPost("ask")]
    [Authorize(Roles = "etudiant,Etudiant")]
    public async Task<ActionResult<AiAskResponseDto>> Ask([FromBody] AiAskRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest(new { message = "Message vide." });

        var etudiant = await _currentUser.GetOrCreateEtudiantAsync(User);
        if (etudiant == null)
            return Unauthorized(new { message = "Étudiant introuvable." });

        var result = await _aiGateway.AskAsync(request.IdProjet, etudiant.IdEtudiant, request.Message.Trim());
        return Ok(result);
    }

    [HttpPost("upload/{idProjet:int}")]
    [Authorize(Roles = "enseignant,Enseignant,admin,Admin")]
    public async Task<IActionResult> Upload(int idProjet, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "Fichier vide." });

        await using var stream = file.OpenReadStream();
        await _aiGateway.UploadCahierAsync(idProjet, stream, file.FileName, file.ContentType);
        return Ok(new { message = "Cahier uploadé et indexé pour le RAG." });
    }
}
