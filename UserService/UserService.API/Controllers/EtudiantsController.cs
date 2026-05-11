using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EtudiantsController : ControllerBase
{
    private readonly IEtudiantService _etudiantService;

    public EtudiantsController(IEtudiantService etudiantService)
    {
        _etudiantService = etudiantService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<IEnumerable<EtudiantDto>>> GetAll()
    {
        var etudiants = await _etudiantService.GetAllEtudiantsAsync();
        return Ok(etudiants);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Enseignant,Etudiant")]
    public async Task<ActionResult<EtudiantDto>> GetById(int id)
    {
        var etudiant = await _etudiantService.GetEtudiantByIdAsync(id);
        if (etudiant == null)
            return NotFound(new { message = "Étudiant introuvable." });

        return Ok(etudiant);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Seul un admin peut créer un compte étudiant
    public async Task<ActionResult<EtudiantDto>> Create([FromBody] EtudiantCreateDto request)
    {
        try
        {
            var etudiant = await _etudiantService.CreateEtudiantAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = etudiant.IdEtudiant }, etudiant);
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
            await _etudiantService.DeleteEtudiantAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
