using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Nécessite d'être connecté
public class PromptController : ControllerBase
{
    private readonly IPromptService _promptService;

    public PromptController(IPromptService promptService)
    {
        _promptService = promptService;
    }

    // POST: api/Prompt/log
    // Cette route sera appelée par votre Backend IA après avoir interrogé OpenAI/Llama
    [HttpPost("log")]
    public async Task<IActionResult> LogInteraction([FromBody] PromptLogRequestDto request)
    {
        await _promptService.LogInteractionAsync(request);
        return Ok(new { message = "Interaction IA enregistrée avec succès." });
    }

    // GET: api/Prompt/projet/5
    // Pour le Dashboard de l'Enseignant
    [HttpGet("projet/{idProjet}")]
    [Authorize(Roles = "Admin, Enseignant")]
    public async Task<ActionResult<IEnumerable<PromptDto>>> GetHistorique(int idProjet)
    {
        var historique = await _promptService.GetHistoriqueProjetAsync(idProjet);
        return Ok(historique);
    }
}