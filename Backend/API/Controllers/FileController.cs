using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FileController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;

    public FileController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        try 
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Aucun fichier sélectionné." });

            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { 
                message = "Fichier uploadé avec succès.",
                fileName = fileName,
                path = $"/uploads/{fileName}"
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"UPLOAD ERROR: {ex.Message}");
            return StatusCode(500, new { message = "Erreur interne lors de l'upload.", details = ex.Message });
        }
    }
}
