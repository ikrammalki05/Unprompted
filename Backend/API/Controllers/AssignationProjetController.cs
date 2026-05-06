using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Enseignant")]
public class AssignationProjetController : ControllerBase
{
    private readonly IGroupeProjetService _groupeProjetService;

    public AssignationProjetController(IGroupeProjetService groupeProjetService)
    {
        _groupeProjetService = groupeProjetService;
    }

    // POST: api/AssignationProjet
    [HttpPost]
    public async Task<IActionResult> AssignerEquipe([FromBody] AssignationProjetRequestDto request)
    {
        try
        {
            await _groupeProjetService.CreerEquipeEtAssignerAsync(request);
            return Ok(new { message = $"L'équipe '{request.NomGroupe}' a été créée et assignée avec succès au projet." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}