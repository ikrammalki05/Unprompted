using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupeController : ControllerBase
{
    private readonly IGroupeService _groupeService;

    public GroupeController(IGroupeService groupeService)
    {
        _groupeService = groupeService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<IEnumerable<GroupeDto>>> GetAll()
        => Ok(await _groupeService.GetAllGroupesAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<GroupeDto>> GetById(int id)
    {
        var groupe = await _groupeService.GetGroupeByIdAsync(id);
        if (groupe == null) return NotFound(new { message = "Groupe introuvable." });
        return Ok(groupe);
    }

    [HttpPost]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<ActionResult<GroupeDto>> Create([FromBody] GroupeCreateDto dto)
    {
        try
        {
            var result = await _groupeService.CreateGroupeAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.IdGroupe }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _groupeService.DeleteGroupeAsync(id);
        return NoContent();
    }

    [HttpPost("{id:int}/etudiant")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> AddEtudiant(int id, [FromBody] EtudiantRoleDto dto)
    {
        try
        {
            await _groupeService.AddEtudiantToGroupeAsync(id, dto);
            return Ok(new { message = "Etudiant ajouté au groupe avec succès." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}/etudiant/{idEtudiant:int}")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> RemoveEtudiant(int id, int idEtudiant)
    {
        await _groupeService.RemoveEtudiantFromGroupeAsync(id, idEtudiant);
        return NoContent();
    }
}
