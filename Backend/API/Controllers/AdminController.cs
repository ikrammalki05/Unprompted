using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")] // Sécurité stricte : Seul un Admin Keycloak peut entrer ici
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    // GET: api/admin/dashboard-stats
    [HttpGet("dashboard-stats")]
    public async Task<IActionResult> GetDashboardStats()
    {
        try
        {
            var stats = await _adminService.GetDashboardStatsAsync();
            return Ok(stats);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de la récupération des statistiques.", detail = ex.Message });
        }
    }

    // POST: api/admin/affecter-enseignant
    [HttpPost("affecter-enseignant")]
    public async Task<IActionResult> AssignerEnseignant([FromBody] AffectationEnseignantRequestDto request)
    {
        try
        {
            await _adminService.AssignerEnseignantAClasseAsync(request);
            return Ok(new { message = "Enseignant assigné à la classe avec succès." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur interne lors de l'assignation.", detail = ex.Message });
        }
    }
}