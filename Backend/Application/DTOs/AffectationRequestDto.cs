namespace Application.DTOs;

public class AffectationRequestDto
{
    public int IdEtudiant { get; set; }
    public int IdGroupe { get; set; }
    public int IdRole { get; set; }
    public int? IdEnseignant { get; set; } // Optionnel, selon ton MCD
}