using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")] 
[ApiController]
[Route("api/[controller]")]
public class ClasseController : ControllerBase
{
    private readonly IClasseService _classeService;

    public ClasseController(IClasseService classeService)
    {
        _classeService = classeService;
    }

    // GET: api/classe
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClasseDto>>> GetClasses()
    {
        var classes = await _classeService.GetAllClassesAsync();
        return Ok(classes);
    }
    // GET: api/classe/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ClasseDto>> GetClasse(int id)
    {
        var classe = await _classeService.GetClasseByIdAsync(id);
        if (classe == null) return NotFound(new { message = "Classe introuvable." });
        return Ok(classe);
    }

    // POST: api/classe
    [HttpPost]
    public async Task<IActionResult> CreateClasse([FromBody] ClasseCreateDto request)
    {
        try
        {
            var result = await _classeService.CreateClasseAsync(request);
            return CreatedAtAction(nameof(GetClasse), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/classe/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClasse(int id, [FromBody] ClasseCreateDto request)
    {
        try
        {
            await _classeService.UpdateClasseAsync(id, request);
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

    // DELETE: api/classe/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClasse(int id)
    {
        try
        {
            await _classeService.DeleteClasseAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
