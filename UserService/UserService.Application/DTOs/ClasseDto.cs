namespace Application.DTOs;

public class ClasseDto
{
    public int IdClasse { get; set; }
    public string NomClasse { get; set; } = string.Empty;
    public string AnneeAcademique { get; set; } = string.Empty;
    public int EffectifMax { get; set; }
    public DateTime? DateCreation { get; set; }
}
