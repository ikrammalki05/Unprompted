using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController] // Indique à .NET que cette classe répond à des requêtes web
[Route("api/[controller]")] // L'URL sera automatiquement /api/groupe (nom de la classe sans "Controller")
public class GroupeController : ControllerBase
{
    private readonly IGroupeService _groupeService;

    // Injection de dépendance de l'interface (le "Menu")
    public GroupeController(IGroupeService groupeService)
    {
        _groupeService = groupeService;
    }

    // Endpoint : GET /api/groupe
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GroupeDto>>> GetGroupes()
    {
        var groupes = await _groupeService.GetAllGroupesAsync();
        return Ok(groupes); // Renvoie un statut HTTP 200 (OK) avec les données en JSON
    }

    // Endpoint : GET /api/groupe/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GroupeDto>> GetGroupe(int id)
    {
        var groupe = await _groupeService.GetGroupeByIdAsync(id);
        
        if (groupe == null)
        {
            return NotFound(new { message = "Le groupe demandé n'existe pas." }); // HTTP 404
        }
        
        return Ok(groupe); // HTTP 200
    }
}