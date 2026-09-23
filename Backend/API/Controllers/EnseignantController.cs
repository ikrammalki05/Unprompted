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

    public EnseignantController(IEnseignantService enseignantService)
    {
        _enseignantService = enseignantService;
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

    // GET: api/enseignant/me
    [HttpGet("me")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> GetMyProfile()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value 
                    ?? User.FindFirst("email")?.Value;
        
        var firstName = User.FindFirst(System.Security.Claims.ClaimTypes.GivenName)?.Value 
                        ?? User.FindFirst("given_name")?.Value ?? "Prénom";
        var lastName = User.FindFirst(System.Security.Claims.ClaimTypes.Surname)?.Value 
                       ?? User.FindFirst("family_name")?.Value ?? "Nom";

        if (string.IsNullOrEmpty(email))
            return Unauthorized(new { message = "Email non trouvé dans le token." });

        var enseignant = await _enseignantService.EnsureEnseignantExistsAsync(email, firstName, lastName);

        return Ok(enseignant);
    }

    // PUT: api/enseignant/me
    [HttpPut("me")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] EnseignantCreateDto request)
    {
        try
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value 
                        ?? User.FindFirst("email")?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new { message = "Email non trouvé dans le token." });

            await _enseignantService.UpdateProfilByEmailAsync(email, request);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de la mise à jour du profil.", detail = ex.Message });
        }
    }
}