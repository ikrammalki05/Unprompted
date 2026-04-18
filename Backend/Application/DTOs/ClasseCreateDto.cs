namespace Application.DTOs;

public class ClasseCreateDto
{
    public string NomClasse { get; set; } = string.Empty;
    public int EffectifMax { get; set; } = 40;
    public string AnneeAcademique { get; set; } = "2023 - 2024";
}