using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Application.DTOs;
using Unprompted.Services.Projet.Application.Services;

namespace Unprompted.Services.Projet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupeController : ControllerBase
{
    private readonly GroupeService _groupeService;

    public GroupeController(GroupeService groupeService)
    {
        _groupeService = groupeService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupeCreateDto dto)
    {
        var groupe = await _groupeService.CreateGroupeAsync(dto);
        return Ok(groupe);
    }

    [HttpPost("{id}/assigner")]
    public async Task<IActionResult> AssignerEtudiant(int id, [FromQuery] int etudiantId, [FromQuery] int roleId)
    {
        var result = await _groupeService.AssignerEtudiantAsync(id, etudiantId, roleId);
        if (!result) return BadRequest(new { message = "Impossible d'affecter l'étudiant" });
        return Ok(new { message = "Étudiant affecté avec succès au groupe" });
    }
}