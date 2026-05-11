namespace Domain.Entities;

public class Classe
{
    public int IdClasse { get; set; }
    public string NomClasse { get; set; } = null!;
    public string AnneeAcademique { get; set; } = null!;
    public int EffectifMax { get; set; }
    public DateTime? DateCreation { get; set; }

    public virtual ICollection<EnseignantClasse> EnseignantClasses { get; set; } = new List<EnseignantClasse>();
    public virtual ICollection<Etudiant> Etudiants { get; set; } = new List<Etudiant>();
}
