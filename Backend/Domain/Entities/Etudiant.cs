using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class Etudiant
{
    public int IdEtudiant { get; set; }

    public string CodeApogee { get; set; } = null!;

    public string? Niveau { get; set; }

    public string? Filiere { get; set; }

    public int IdUtilisateur { get; set; }

    public virtual ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();

    public virtual ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();

    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();

    public virtual Utilisateur IdUtilisateurNavigation { get; set; } = null!;

    public virtual ICollection<Prompt> Prompts { get; set; } = new List<Prompt>();
}
