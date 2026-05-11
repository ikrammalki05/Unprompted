using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin, Enseignant")] // Seul le staff gère l'IA
public class ConfigurationIumController : ControllerBase
{
    private readonly IConfigurationIumService _configService;

    public ConfigurationIumController(IConfigurationIumService configService)
    {
        _configService = configService;
    }

    // GET: api/ConfigurationIum/projet/5
    [HttpGet("projet/{idProjet}")]
    public async Task<ActionResult<ConfigurationIumDto>> GetConfigByProjet(int idProjet)
    {
        var config = await _configService.GetConfigByProjetAsync(idProjet);
        
        if (config == null)
            return NotFound(new { message = "Aucune configuration IA trouvée pour ce projet." });

        return Ok(config);
    }

    // POST: api/ConfigurationIum
    [HttpPost]
    public async Task<ActionResult<ConfigurationIumDto>> DefineConfig([FromBody] ConfigurationIumCreateDto request)
    {
        try
        {
            var result = await _configService.DefineConfigAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}