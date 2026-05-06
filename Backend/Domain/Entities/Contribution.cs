using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Contribution
{
    public int IdContribution { get; set; }

    public string? MessageCommit { get; set; }

    public DateTime? DateCommit { get; set; }

    public string? HashCommit { get; set; }

    public int? LignesAjoutees { get; set; }

    public int? LignesSupprimees { get; set; }

    public int IdEtudiant { get; set; }

    public int IdProjet { get; set; }

    public virtual Etudiant IdEtudiantNavigation { get; set; } = null!;

    public virtual Projet IdProjetNavigation { get; set; } = null!;
}
