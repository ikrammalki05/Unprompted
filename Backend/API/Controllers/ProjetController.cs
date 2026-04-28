using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjetController : ControllerBase
{
    private readonly IProjetService _projetService;

    public ProjetController(IProjetService projetService)
    {
        _projetService = projetService;
    }

    // GET: api/projet — Admin + Enseignant
    [HttpGet]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<IEnumerable<ProjetDto>>> GetProjets()
        => Ok(await _projetService.GetAllProjectsAsync());

    // GET: api/projet/count
    [HttpGet("count")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<int>> GetProjetsCount()
        => Ok(await _projetService.GetProjetsCountAsync());

    // GET: api/projet/5
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<ProjetDto>> GetProjet(int id)
    {
        var projet = await _projetService.GetProjectByIdAsync(id);
        if (projet == null) return NotFound(new { message = "Projet introuvable." });
        return Ok(projet);
    }

    // GET: api/projet/5/groupes
    [HttpGet("{id:int}/groupes")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<IEnumerable<GroupeDto>>> GetGroupes(int id)
    {
        try
        {
            var groupes = await _projetService.GetGroupesProjetAsync(id);
            return Ok(groupes);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/projet/enseignant/3 — projets d'un enseignant
    [HttpGet("enseignant/{idEnseignant}")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<IActionResult> GetProjetsByEnseignant(int idEnseignant)
        => Ok(await _projetService.GetProjectsByEnseignantIdAsync(idEnseignant));

    // POST: api/projet/enseignant/3 — créer un projet
    [HttpPost("enseignant/{idEnseignant}")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> CreateProjet(int idEnseignant, [FromBody] ProjetCreateDto request)
    {
        try
        {
            var result = await _projetService.CreateProjetAsync(idEnseignant, request);
            return CreatedAtAction(nameof(GetProjet), new { id = result.IDProjet }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/projet/5/enseignant/3
    [HttpPut("{id}/enseignant/{idEnseignant}")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> UpdateProjet(int id, int idEnseignant, [FromBody] ProjetCreateDto request)
    {
        try
        {
            await _projetService.UpdateProjetAsync(id, idEnseignant, request);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/projet/5/enseignant/3
    [HttpDelete("{id}/enseignant/{idEnseignant}")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> DeleteProjet(int id, int idEnseignant)
    {
        try
        {
            await _projetService.DeleteProjetAsync(id, idEnseignant);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/projet/{id}/contributions
    [HttpGet("{id:int}/contributions")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<IActionResult> GetContributions(int id)
    {
        var contributions = await _projetService.GetContributionsByProjectIdAsync(id);
        return Ok(contributions);
    }

    // POST: api/projet/{id}/contributions
    [HttpPost("{id:int}/contributions")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> CreateContribution(int id, [FromBody] ContributionCreateDto request)
    {
        try
        {
            var contribution = await _projetService.CreateContributionAsync(id, request);
            return CreatedAtAction(nameof(GetContributions), new { id }, contribution);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    

    [HttpPatch("{id:int}/suivi")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> UpdateProjetSuivi(int id, [FromBody] ProjetSuiviDto request)
    {
        try
        {
            await _projetService.UpdateProjetSuiviAsync(id, request);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("assigner-etudiant")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> AssignerEtudiant([FromBody] AssignerEtudiantProjetDto request , int idEnseignant)
    {
        try
        {
            await _projetService.AssignerEtudiantAsync(request, idEnseignant);
            return Ok(new { message = "Etudiant assigné au projet avec succès." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("assigner-groupe")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> AssignerProjetAuGroupe([FromBody] AssignerProjetGroupeDto request)
    {
        try
        {
            await _projetService.AssignerProjetAuGroupeAsync(request);
            return Ok(new { message = "Projet assigné au groupe avec succès." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}