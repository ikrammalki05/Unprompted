using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Etudiant")] // 🔒 Strictement réservé aux étudiants !
public class ProfilController : ControllerBase
{
    private readonly IEtudiantService _etudiantService;

    public ProfilController(IEtudiantService etudiantService)
    {
        _etudiantService = etudiantService;
    }

    // GET: api/Profil/etudiant/5
    [HttpGet("etudiant/{idEtudiant}")]
    public async Task<ActionResult<EtudiantProfilDto>> GetMonProfil(int idEtudiant)
    {
        try
        {
            var profil = await _etudiantService.GetProfilEtudiantAsync(idEtudiant);
            
            if (profil == null)
                return NotFound(new { message = "Profil introuvable." });

            return Ok(profil);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de la récupération du profil.", detail = ex.Message });
        }
    }

    // GET: api/Profil/etudiant/5/historique
    [HttpGet("etudiant/{idEtudiant}/historique")]
    public async Task<ActionResult<HistoriqueEtudiantDto>> GetMonHistorique(int idEtudiant)
    {
        try
        {
            var historique = await _etudiantService.GetHistoriqueAsync(idEtudiant);
            
            if (historique == null)
                return NotFound(new { message = "Étudiant introuvable." });

            return Ok(historique);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erreur lors de l'agrégation de l'historique.", detail = ex.Message });
        }
    }
    
}