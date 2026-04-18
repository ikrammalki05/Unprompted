using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")] 
[ApiController]
[Route("api/[controller]")]
public class EtudiantController : ControllerBase
{
    private readonly IEtudiantService _etudiantService;

    public EtudiantController(IEtudiantService etudiantService)
    {
        _etudiantService = etudiantService;
    }

    // GET: api/etudiant
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EtudiantDto>>> GetEtudiants()
    {
        var etudiants = await _etudiantService.GetAllEtudiantsAsync();
        return Ok(etudiants);
    }

    // POST: api/etudiant
    [HttpPost]
    public async Task<IActionResult> CreateEtudiant([FromBody] EtudiantCreateDto request)
    {
        try
        {
            var result = await _etudiantService.CreateEtudiantAsync(request);
            // Renvoie un code 201 Created avec l'objet créé
            return CreatedAtAction(nameof(GetEtudiants), new { id = result.Id }, result); 
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEtudiant(int id, [FromBody] EtudiantCreateDto request)
    {
        try
        {
            await _etudiantService.UpdateEtudiantAsync(id, request);
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
    public async Task<IActionResult> DeleteEtudiant(int id)
    {
        try
        {
            await _etudiantService.DeleteEtudiantAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    
}