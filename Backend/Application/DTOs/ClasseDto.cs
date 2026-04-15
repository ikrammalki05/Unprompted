namespace Application.DTOs;

public class ClasseDto
{
    public int Id { get; set; }
    public string NomClasse { get; set; } = string.Empty;
    public string AnneeAcademique { get; set; } = string.Empty; // Ex: "2023 - 2024"
    public int EffectifActuel { get; set; }
    public int EffectifMax { get; set; } = 40; // Par défaut, selon la maquette
    public string EnseignantReferent { get; set; } = "Aucun";
}