using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Roles = "Admin")] 
[ApiController]
[Route("api/[controller]")]
public class ClasseController : ControllerBase
{
    private readonly IClasseService _classeService;

    public ClasseController(IClasseService classeService)
    {
        _classeService = classeService;
    }

    // GET: api/classe
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClasseDto>>> GetClasses()
    {
        var classes = await _classeService.GetAllClassesAsync();
        return Ok(classes);
    }
}