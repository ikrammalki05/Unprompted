using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class Affectation
{
    public int IdAffectation { get; set; }

    public int IdEtudiant { get; set; }

    public int IdGroupe { get; set; }

    public int IdRole { get; set; }

    public int? IdEnseignant { get; set; }

    public DateTime? DateAffectation { get; set; }

    public virtual Enseignant? IdEnseignantNavigation { get; set; }

    public virtual Etudiant IdEtudiantNavigation { get; set; } = null!;

    public virtual Groupe IdGroupeNavigation { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
