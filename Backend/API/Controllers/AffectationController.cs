using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // L'URL sera /api/affectation
public class AffectationController : ControllerBase
{
    private readonly IAffectationService _affectationService;

    public AffectationController(IAffectationService affectationService)
    {
        _affectationService = affectationService;
    }

    // Endpoint : POST /api/affectation
    [HttpPost]
    public async Task<IActionResult> CreerAffectation([FromBody] AffectationRequestDto request)
    {
        try
        {
            // On envoie la requête à la cuisine (le service)
            await _affectationService.CreerAffectationAsync(request);
            
            // Si tout s'est bien passé, on renvoie un HTTP 200 avec un message de succès
            return Ok(new { message = "L'étudiant a été affecté au groupe avec succès." });
        }
        catch (ArgumentException ex)
        {
            // Erreur 400 (Bad Request) : Le client s'est trompé dans les IDs
            return BadRequest(new { erreur = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // Erreur 409 (Conflict) : Règle métier non respectée (déjà affecté)
            return Conflict(new { erreur = ex.Message });
        }
        catch (Exception)
        {
            // Erreur 500 (Internal Server Error) : Erreur inattendue (ex: base de données plantée)
            return StatusCode(500, new { erreur = "Une erreur interne est survenue lors de l'affectation." });
        }
    }
}