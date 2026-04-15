using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")] 
[ApiController]
[Route("api/[controller]")]
public class EtudiantController : ControllerBase
{
    private readonly IEtudiantService _etudiantService;

    public EtudiantController(IEtudiantService etudiantService)
    {
        _etudiantService = etudiantService;
    }

    // GET: api/etudiant
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EtudiantDto>>> GetEtudiants()
    {
        var etudiants = await _etudiantService.GetAllEtudiantsAsync();
        return Ok(etudiants);
    }
}