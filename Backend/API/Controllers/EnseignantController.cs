using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")] 
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
    
}