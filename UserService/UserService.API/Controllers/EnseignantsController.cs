using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnseignantsController : ControllerBase
{
    private readonly IEnseignantService _enseignantService;

    public EnseignantsController(IEnseignantService enseignantService)
    {
        _enseignantService = enseignantService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<IEnumerable<EnseignantDto>>> GetAll()
    {
        var enseignants = await _enseignantService.GetAllEnseignantsAsync();
        return Ok(enseignants);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<EnseignantDto>> GetById(int id)
    {
        var enseignant = await _enseignantService.GetEnseignantByIdAsync(id);
        if (enseignant == null)
            return NotFound(new { message = "Enseignant introuvable." });

        return Ok(enseignant);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Seul un admin peut créer un compte enseignant
    public async Task<ActionResult<EnseignantDto>> Create([FromBody] EnseignantCreateDto request)
    {
        try
        {
            var enseignant = await _enseignantService.CreateEnseignantAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = enseignant.IdEnseignant }, enseignant);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _enseignantService.DeleteEnseignantAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
