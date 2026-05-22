using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Application.DTOs;
using Unprompted.Services.Projet.Application.Services;

namespace Unprompted.Services.Projet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjetController : ControllerBase
{
    private readonly ProjetService _projetService;

    public ProjetController(ProjetService projetService)
    {
        _projetService = projetService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var projet = await _projetService.GetProjetByIdAsync(id);
        if (projet == null) return NotFound(new { message = "Projet introuvable" });
        return Ok(projet);
    }

    [HttpGet("enseignant/{enseignantId}")]
    public async Task<IActionResult> GetByEnseignant(int enseignantId)
    {
        var projets = await _projetService.GetProjetsByEnseignantAsync(enseignantId);
        return Ok(projets);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProjetCreateDto dto)
    {
        var projet = await _projetService.CreateProjetAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = projet.IdProjet }, projet);
    }
}