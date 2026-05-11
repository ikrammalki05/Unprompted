namespace Domain.Entities;

public partial class Etudiant
{
    public int IdEtudiant { get; set; }
    public string CodeApogee { get; set; } = null!;
    public string? Niveau { get; set; }
    public string? Filiere { get; set; }
    public int IdUtilisateur { get; set; }

    public int? IdClasse { get; set; }

    public virtual Classe? IdClasseNavigation { get; set; }
    public virtual Utilisateur IdUtilisateurNavigation { get; set; } = null!;
    public virtual ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
}
