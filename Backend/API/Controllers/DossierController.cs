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
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/dossier/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _dossierService.DeleteAsync(id);
        return NoContent();
    }
}