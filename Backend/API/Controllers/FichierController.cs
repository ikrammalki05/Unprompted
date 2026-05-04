using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FichierController : ControllerBase
{
    private readonly IFichierService _fichierService;

    public FichierController(IFichierService fichierService)
    {
        _fichierService = fichierService;
    }

    // POST: api/fichier
    [HttpPost]
    public async Task<IActionResult> Create(FichierCreateDto dto)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value ?? "system";

            var result = await _fichierService.CreateAsync(dto, userId);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET: api/fichier/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var fichier = await _fichierService.GetByIdAsync(id);

        if (fichier == null)
            return NotFound();

        return Ok(fichier);
    }

    // PUT: api/fichier/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, FichierUpdateDto dto)
    {
        try
        {
            await _fichierService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // DELETE: api/fichier/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _fichierService.DeleteAsync(id);
        return NoContent();
    }

    // POST: api/fichier/{id}/autosave
    [HttpPost("{id}/autosave")]
    public async Task<IActionResult> Autosave(int id, FichierUpdateDto dto)
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value ?? "system";

            await _fichierService.AutosaveAsync(id, dto.Contenu, userId);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}