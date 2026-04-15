using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")] 
[ApiController]
[Route("api/[controller]")]
public class EnseignantController : ControllerBase
{
    private readonly IEnseignantService _enseignantService;

    public EnseignantController(IEnseignantService enseignantService)
    {
        _enseignantService = enseignantService;
    }

    // GET: api/enseignant
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnseignantDto>>> GetEnseignants()
    {
        var enseignants = await _enseignantService.GetAllEnseignantsAsync();
        return Ok(enseignants);
    }
}