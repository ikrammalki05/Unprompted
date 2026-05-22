using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Application.Services;

namespace Unprompted.Services.Projet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigurationIumController : ControllerBase
{
    private readonly ConfigurationIumService _configService;

    public ConfigurationIumController(ConfigurationIumService configService)
    {
        _configService = configService;
    }

    [HttpGet("projet/{projetId}")]
    public async Task<IActionResult> GetByProjet(int projetId)
    {
        var config = await _configService.GetByProjetIdAsync(projetId);
        if (config == null) return NotFound(new { message = "Configuration IA introuvable pour ce projet." });
        return Ok(config);
    }
}