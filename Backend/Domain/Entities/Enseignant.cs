using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class Enseignant
{
    public int IdEnseignant { get; set; }

    public string? Specialite { get; set; }

    public string? Departement { get; set; }

    public int IdUtilisateur { get; set; }

    public virtual ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();

    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();

    public virtual Utilisateur IdUtilisateurNavigation { get; set; } = null!;

    public virtual ICollection<Projet> Projets { get; set; } = new List<Projet>();
}
