namespace Domain.Entities;

public partial class Enseignant
{
    public int IdEnseignant { get; set; }
    public string? Specialite { get; set; }
    public string? Departement { get; set; }
    public int IdUtilisateur { get; set; }

    public virtual Utilisateur IdUtilisateurNavigation { get; set; } = null!;
    public virtual ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
    public virtual ICollection<EnseignantClasse> EnseignantClasses { get; set; } = new List<EnseignantClasse>();
}
