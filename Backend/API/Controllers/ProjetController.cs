using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjetController : ControllerBase
{
    private readonly IProjetService _projetService;

    public ProjetController(IProjetService projetService)
    {
        _projetService = projetService;
    }

    // GET: api/projet — Admin + Enseignant
    [HttpGet]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<IEnumerable<ProjetDto>>> GetProjets()
        => Ok(await _projetService.GetAllProjectsAsync());

    // GET: api/projet/count
    [HttpGet("count")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<int>> GetProjetsCount()
        => Ok(await _projetService.GetProjetsCountAsync());

    // GET: api/projet/5
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<ProjetDto>> GetProjet(int id)
    {
        var projet = await _projetService.GetProjectByIdAsync(id);
        if (projet == null) return NotFound(new { message = "Projet introuvable." });
        return Ok(projet);
    }

    // GET: api/projet/5/groupes
    [HttpGet("{id:int}/groupes")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<IEnumerable<GroupeDto>>> GetGroupes(int id)
    {
        try
        {
            var groupes = await _projetService.GetGroupesProjetAsync(id);
            return Ok(groupes);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // GET: api/projet/enseignant/3 — projets d'un enseignant
    [HttpGet("enseignant/{idEnseignant}")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<IActionResult> GetProjetsByEnseignant(int idEnseignant)
        => Ok(await _projetService.GetProjectsByEnseignantIdAsync(idEnseignant));

    // POST: api/projet/enseignant/3 — créer un projet
    [HttpPost("enseignant/{idEnseignant}")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> CreateProjet(int idEnseignant, [FromBody] ProjetCreateDto request)
    {
        try
        {
            var result = await _projetService.CreateProjetAsync(idEnseignant, request);
            return CreatedAtAction(nameof(GetProjet), new { id = result.IDProjet }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT: api/projet/5/enseignant/3
    [HttpPut("{id}/enseignant/{idEnseignant}")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> UpdateProjet(int id, int idEnseignant, [FromBody] ProjetCreateDto request)
    {
        try
        {
            await _projetService.UpdateProjetAsync(id, idEnseignant, request);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE: api/projet/5/enseignant/3
    [HttpDelete("{id}/enseignant/{idEnseignant}")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> DeleteProjet(int id, int idEnseignant)
    {
        try
        {
            await _projetService.DeleteProjetAsync(id, idEnseignant);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}/contributions")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<IActionResult> GetContributions(int id)
    {
        var contributions = await _projetService.GetContributionsByProjectIdAsync(id);
        return Ok(contributions);
    }

    [HttpPatch("{id:int}/suivi")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> UpdateProjetSuivi(int id, [FromBody] ProjetSuiviDto request)
    {
        try
        {
            await _projetService.UpdateProjetSuiviAsync(id, request);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("assigner-etudiant")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> AssignerEtudiant([FromBody] AssignerEtudiantProjetDto request , int idEnseignant)
    {
        try
        {
            await _projetService.AssignerEtudiantAsync(request, idEnseignant);
            return Ok(new { message = "Etudiant assigné au projet avec succès." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("assigner-groupe")]
    [Authorize(Roles = "Enseignant,Admin")]
    public async Task<IActionResult> AssignerProjetAuGroupe([FromBody] AssignerProjetGroupeDto request)
    {
        try
        {
            await _projetService.AssignerProjetAuGroupeAsync(request);
            return Ok(new { message = "Projet assigné au groupe avec succès." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{idProjet:int}/etudiant/{idEtudiant:int}/activite")]
    [Authorize(Roles = "Admin,Enseignant")]
    public async Task<ActionResult<EtudiantActiviteDto>> GetEtudiantActivite(int idProjet, int idEtudiant)
    {
        var activite = await _projetService.GetEtudiantActiviteAsync(idProjet, idEtudiant);
        return Ok(activite);
    }

    [HttpPost("{idProjet:int}/prompts")]
    [Authorize(Roles = "Etudiant")]
    public async Task<ActionResult<PromptDto>> CreatePrompt(int idProjet, [FromBody] PromptCreateDto request)
    {
        var prompt = await _projetService.CreatePromptAsync(idProjet, request);
        return CreatedAtAction(nameof(GetEtudiantActivite), new { idProjet, idEtudiant = request.IdEtudiant }, prompt);
    }

    [HttpGet("etudiant/{idEtudiant:int}")]
    [Authorize(Roles = "Admin,Enseignant,Etudiant")]
    public async Task<ActionResult<IEnumerable<ProjetDto>>> GetProjectsByEtudiant(int idEtudiant)
    {
        var projets = await _projetService.GetProjectsByEtudiantIdAsync(idEtudiant);
        return Ok(projets);
    }

    [HttpGet("{idProjet:int}/etudiant/{idEtudiant:int}/collegues")]
    [Authorize(Roles = "Admin,Enseignant,Etudiant")]
    public async Task<ActionResult<IEnumerable<EtudiantGroupeDto>>> GetCollegues(int idProjet, int idEtudiant)
    {
        var collegues = await _projetService.GetColleguesAsync(idProjet, idEtudiant);
        return Ok(collegues);
    }

    // POST: api/projet/1/upload-cahier
[HttpPost("{id}/upload-cahier")]
[Authorize(Roles = "Enseignant,Admin")]
public async Task<IActionResult> UploadCahier(int id, IFormFile fichier)
{
    if (fichier == null || fichier.Length == 0)
        return BadRequest(new { message = "Fichier vide." });

    if (!fichier.ContentType.Contains("pdf"))
        return BadRequest(new { message = "Seulement les PDF sont acceptés." });

    using var memoryStream = new MemoryStream();
    await fichier.CopyToAsync(memoryStream);
    await _projetService.SaveCahierAsync(id, memoryStream.ToArray());
    return Ok(new { message = "Cahier des charges uploadé." });
}

// GET: api/projet/1/download-cahier
[HttpGet("{id}/download-cahier")]
[Authorize(Roles = "Enseignant,Admin,Etudiant")]
public async Task<IActionResult> DownloadCahier(int id)
{
    var bytes = await _projetService.GetCahierAsync(id);
    if (bytes == null)
        return NotFound(new { message = "Cahier des charges introuvable." });

    return File(bytes, "application/pdf", "cahier_des_charges.pdf");
}
}