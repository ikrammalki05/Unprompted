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
    
}