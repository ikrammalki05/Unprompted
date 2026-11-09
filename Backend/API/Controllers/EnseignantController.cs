using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

// [Authorize(Roles = "Admin")] 
[ApiController]
[Route("api/[controller]")]
public class EnseignantController : ControllerBase
{
    private readonly IEnseignantService _enseignantService;
    private readonly ICurrentUserService _currentUser;

    public EnseignantController(IEnseignantService enseignantService, ICurrentUserService currentUser)
    {
        _enseignantService = enseignantService;
        _currentUser = currentUser;
    }

    [HttpGet("me")]
    [Authorize(Roles = "enseignant,Enseignant")]
    public async Task<IActionResult> GetMe()
    {
        var enseignant = await _currentUser.GetOrCreateEnseignantAsync(User);
        if (enseignant == null) return Unauthorized();

        var u = enseignant.IdUtilisateurNavigation;
        var prenom = u?.Prenom ?? "";
        var nom = u?.Nom ?? "";
        return Ok(new
        {
            id = enseignant.IdEnseignant,
            idEnseignant = enseignant.IdEnseignant,
            nom,
            prenom,
            nomComplet = $"{prenom} {nom}".Trim(),
            email = u?.Email,
            specialite = enseignant.Specialite,
            departement = enseignant.Departement
        });
    }

    // GET: api/enseignant
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnseignantDto>>> GetEnseignants()
    {
        var enseignants = await _enseignantService.GetAllEnseignantsAsync();
        return Ok(enseignants);
    }

    // POST: api/enseignant
    [HttpPost]
    public async Task<IActionResult> CreateEnseignant([FromBody] EnseignantCreateDto request)
    {
        try
        {
            var result = await _enseignantService.CreateEnseignantAsync(request);
            return CreatedAtAction(nameof(GetEnseignants), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEnseignant(int id, [FromBody] EnseignantCreateDto request)
    {
        try
        {
            await _enseignantService.UpdateEnseignantAsync(id, request);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEnseignant(int id)
    {
        try
        {
            await _enseignantService.DeleteEnseignantAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("{idEnseignant}/statistiques")]
    public async Task<IActionResult> GetStatistiques(int idEnseignant)
    {
        try
        {
            var stats = await _enseignantService.GetStatistiquesAsync(idEnseignant);
            return Ok(stats);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    
}