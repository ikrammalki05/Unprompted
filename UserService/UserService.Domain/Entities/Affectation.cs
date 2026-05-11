namespace Domain.Entities;

public partial class Affectation
{
    public int IdAffectation { get; set; }
    public int? IdEtudiant { get; set; }
    public int? IdEnseignant { get; set; }
    public int IdRole { get; set; }
    public int IdGroupe { get; set; }
    public DateTime? DateAffectation { get; set; }

    public virtual Etudiant? IdEtudiantNavigation { get; set; }
    public virtual Enseignant? IdEnseignantNavigation { get; set; }
    public virtual Role IdRoleNavigation { get; set; } = null!;
}
