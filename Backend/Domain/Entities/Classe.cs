namespace Domain.Entities;

public class Classe
{
    public int IdClasse { get; set; }
    public string NomClasse { get; set; } = null!;
    public string AnneeAcademique { get; set; } = null!;
    public int EffectifMax { get; set; }
    public DateTime? DateCreation { get; set; }
}