namespace Application.DTOs;

public class EvaluationDto
{
    public int IdEvaluation { get; set; }
    public decimal Note { get; set; }
    public string? Commentaire { get; set; }
    public DateTime DateEvaluation { get; set; }
    public int IdEtudiant { get; set; }
    public int IdProjet { get; set; }
    public int IdEnseignant { get; set; }
}

public class EvaluationCreateDto
{
    public int EtudiantId { get; set; }
    public int ProjetId { get; set; }
    public int EnseignantId { get; set; }
    public decimal PerformanceTechnique { get; set; }
    public string? Commentaire { get; set; }
}
