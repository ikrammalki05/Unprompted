using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Nécessite d'être connecté via Keycloak
public class ProjetController : ControllerBase
{
    private readonly IProjetService _projetService;

    public ProjetController(IProjetService projetService)
    {
        _projetService = projetService;
    }

    // GET: api/Projet
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjetDto>>> GetAll()
    {
        var projets = await _projetService.GetAllProjetsAsync();
        return Ok(projets);
    }

    // GET: api/Projet/enseignant/5
    [HttpGet("enseignant/{idEnseignant}")]
    public async Task<ActionResult<IEnumerable<ProjetDto>>> GetByEnseignant(int idEnseignant)
    {
        var projets = await _projetService.GetProjetsByEnseignantAsync(idEnseignant);
        return Ok(projets);
    }

    // POST: api/Projet
    [HttpPost]
    [Authorize(Roles = "Admin, Enseignant")] // Seuls les admins ou profs peuvent créer
    public async Task<ActionResult<ProjetDto>> Create([FromBody] ProjetCreateDto request)
    {
        try
        {
            var result = await _projetService.CreateProjetAsync(request);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}