namespace Domain.Entities;

public class EnseignantClasse
{
    public int IdEnseignantClasse { get; set; }
    public int IdEnseignant { get; set; }
    public int IdClasse { get; set; }
    public DateTime? DateAffectation { get; set; }

    public virtual Enseignant IdEnseignantNavigation { get; set; } = null!;
    public virtual Classe IdClasseNavigation { get; set; } = null!;
}