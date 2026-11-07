using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfilController : ControllerBase
{
    private readonly IEtudiantService _etudiantService;
    private readonly ICurrentUserService _currentUser;
    private readonly IUtilisateurRepository _utilisateurRepo;
    private readonly IEtudiantRepository _etudiantRepo;

    public ProfilController(
        IEtudiantService etudiantService,
        ICurrentUserService currentUser,
        IUtilisateurRepository utilisateurRepo,
        IEtudiantRepository etudiantRepo)
    {
        _etudiantService = etudiantService;
        _currentUser = currentUser;
        _utilisateurRepo = utilisateurRepo;
        _etudiantRepo = etudiantRepo;
    }

    [HttpGet("me")]
    [Authorize(Roles = "etudiant,Etudiant")]
    public async Task<ActionResult<EtudiantProfilDto>> GetMe()
    {
        var etudiant = await _currentUser.GetOrCreateEtudiantAsync(User);
        if (etudiant == null)
            return Unauthorized(new { message = "Profil étudiant introuvable." });

        var profil = await _etudiantService.GetProfilEtudiantAsync(etudiant.IdEtudiant);
        return profil == null ? NotFound() : Ok(profil);
    }

    [HttpPut("me")]
    [Authorize(Roles = "etudiant,Etudiant")]
    public async Task<IActionResult> UpdateMe([FromBody] ProfilUpdateDto dto)
    {
        var etudiant = await _currentUser.GetOrCreateEtudiantAsync(User);
        if (etudiant == null)
            return Unauthorized();

        var full = await _etudiantRepo.GetByIdAsync(etudiant.IdEtudiant);
        if (full?.IdUtilisateurNavigation == null)
            return NotFound();

        var utilisateur = full.IdUtilisateurNavigation;
        if (!string.IsNullOrWhiteSpace(dto.Nom)) utilisateur.Nom = dto.Nom.Trim();
        if (!string.IsNullOrWhiteSpace(dto.Prenom)) utilisateur.Prenom = dto.Prenom.Trim();
        if (!string.IsNullOrWhiteSpace(dto.Email)) utilisateur.Email = dto.Email.Trim();
        if (!string.IsNullOrWhiteSpace(dto.CodeApogee)) full.CodeApogee = dto.CodeApogee.Trim();
        if (dto.Niveau != null) full.Niveau = dto.Niveau;
        if (dto.Filiere != null) full.Filiere = dto.Filiere;

        await _utilisateurRepo.UpdateAsync(utilisateur);
        await _etudiantRepo.UpdateAsync(full);

        var profil = await _etudiantService.GetProfilEtudiantAsync(full.IdEtudiant);
        return Ok(profil);
    }

    [HttpGet("me/historique")]
    [Authorize(Roles = "etudiant,Etudiant")]
    public async Task<ActionResult<HistoriqueEtudiantDto>> GetMonHistoriqueMe()
    {
        var etudiant = await _currentUser.GetOrCreateEtudiantAsync(User);
        if (etudiant == null) return Unauthorized();

        var historique = await _etudiantService.GetHistoriqueAsync(etudiant.IdEtudiant);
        return historique == null ? NotFound() : Ok(historique);
    }

    [HttpGet("etudiant/{idEtudiant}")]
    [Authorize(Roles = "etudiant,Etudiant,enseignant,Enseignant,admin,Admin")]
    public async Task<ActionResult<EtudiantProfilDto>> GetMonProfil(int idEtudiant)
    {
        var profil = await _etudiantService.GetProfilEtudiantAsync(idEtudiant);
        if (profil == null)
            return NotFound(new { message = "Profil introuvable." });
        return Ok(profil);
    }

    [HttpGet("etudiant/{idEtudiant}/historique")]
    [Authorize(Roles = "etudiant,Etudiant,enseignant,Enseignant,admin,Admin")]
    public async Task<ActionResult<HistoriqueEtudiantDto>> GetMonHistorique(int idEtudiant)
    {
        var historique = await _etudiantService.GetHistoriqueAsync(idEtudiant);
        if (historique == null)
            return NotFound(new { message = "Étudiant introuvable." });
        return Ok(historique);
    }
}
