using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok(new { message = "Endpoint public, pas besoin de token" });
    }

    [HttpGet("protected")]
    [Authorize]
    public IActionResult Protected()
    {
        var username = User.Identity?.Name;
        return Ok(new { message = "Vous êtes authentifié !", user = username });
    }

  [HttpGet("etudiant")]
[Authorize(Roles = "Etudiant")]
public IActionResult EtudiantOnly()
{
    return Ok(new { message = "Bienvenue Etudiant !" });
}
}