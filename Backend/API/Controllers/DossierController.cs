using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DossierController : ControllerBase
{
    private readonly IDossierService _dossierService;

    public DossierController(IDossierService dossierService)
    {
        _dossierService = dossierService;
    }

    // POST: api/dossier
    [HttpPost]
    public async Task<IActionResult> Create(DossierCreateDto dto)
    {
        try
        {
            var result = await _dossierService.CreateAsync(dto);
            return Created("", result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message, inner = ex.InnerException?.Message });
        }
    }

    // DELETE: api/dossier/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dossierService.DeleteAsync(id);
        return NoContent();
    }

    // GET: api/dossier/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _dossierService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    // GET: api/dossier/projet/{projetId}
    [HttpGet("projet/{projetId}")]
    public async Task<IActionResult> GetByProjet(int projetId)
    {
        var result = await _dossierService.GetByProjetIdAsync(projetId);
        return Ok(result);
    }

    // PUT: api/dossier/{id}/rename
    [HttpPut("{id}/rename")]
    public async Task<IActionResult> Rename(int id, DossierRenameDto dto)
    {
        try
        {
            var result = await _dossierService.RenameAsync(id, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}