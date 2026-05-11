using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly IClasseService _classeService;

    public ClassesController(IClasseService classeService)
    {
        _classeService = classeService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<IEnumerable<ClasseDto>>> GetAll()
    {
        var classes = await _classeService.GetAllClassesAsync();
        return Ok(classes);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<ClasseDto>> GetById(int id)
    {
        var classe = await _classeService.GetClasseByIdAsync(id);
        if (classe == null)
            return NotFound(new { message = "Classe introuvable." });

        return Ok(classe);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ClasseDto>> Create([FromBody] ClasseCreateDto request)
    {
        try
        {
            var classe = await _classeService.CreateClasseAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = classe.IdClasse }, classe);
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
            await _classeService.DeleteClasseAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{idClasse:int}/etudiants/{idEtudiant:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AffecterEtudiant(int idClasse, int idEtudiant)
    {
        try
        {
            await _classeService.AffecterEtudiantAsync(idClasse, idEtudiant);
            return Ok(new { message = "Étudiant affecté à la classe avec succès." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{idClasse:int}/enseignants/{idEnseignant:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AffecterEnseignant(int idClasse, int idEnseignant)
    {
        try
        {
            await _classeService.AffecterEnseignantAsync(idClasse, idEnseignant);
            return Ok(new { message = "Enseignant affecté à la classe avec succès." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
