using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Enseignant")]
public class EvaluationController : ControllerBase
{
    private readonly IEvaluationRepository _evaluationRepository;

    public EvaluationController(IEvaluationRepository evaluationRepository)
    {
        _evaluationRepository = evaluationRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EvaluationDto>>> GetAll()
    {
        var evaluations = await _evaluationRepository.GetAllAsync();
        return Ok(evaluations.Select(e => new EvaluationDto
        {
            IdEvaluation = e.IdEvaluation,
            Note = e.Note ?? 0,
            Commentaire = e.Commentaire,
            DateEvaluation = e.DateEvaluation ?? DateTime.Now,
            IdEtudiant = e.IdEtudiant,
            IdProjet = e.IdProjet,
            IdEnseignant = e.IdEnseignant
        }));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EvaluationCreateDto request)
    {
        var evaluation = new Evaluation
        {
            IdEtudiant = request.EtudiantId,
            IdProjet = request.ProjetId,
            IdEnseignant = request.EnseignantId != 0 ? request.EnseignantId : 1, // Fallback si non fourni
            Note = request.PerformanceTechnique,
            Commentaire = request.Commentaire,
            DateEvaluation = DateTime.Now
        };

        var result = await _evaluationRepository.AddAsync(evaluation);
        return Ok(result);
    }
}
