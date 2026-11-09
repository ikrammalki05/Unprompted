using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Enseignant")] // Strictement réservé au staff
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    // GET: api/Analytics/projet/5
    [HttpGet("projet/{idProjet}")]
    public async Task<ActionResult<IEnumerable<AnalyticsEtudiantDto>>> GetRapportProjet(int idProjet)
    {
        try
        {
            var rapport = await _analyticsService.GenererRapportProjetAsync(idProjet);
            return Ok(rapport);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}