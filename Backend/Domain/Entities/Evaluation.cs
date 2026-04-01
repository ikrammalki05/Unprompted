using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class Evaluation
{
    public int IdEvaluation { get; set; }

    public decimal? Note { get; set; }

    public string? Commentaire { get; set; }

    public DateTime? DateEvaluation { get; set; }

    public decimal? IndicateurDependanceIa { get; set; }

    public int IdEtudiant { get; set; }

    public int IdProjet { get; set; }

    public int IdEnseignant { get; set; }

    public virtual Enseignant IdEnseignantNavigation { get; set; } = null!;

    public virtual Etudiant IdEtudiantNavigation { get; set; } = null!;

    public virtual Projet IdProjetNavigation { get; set; } = null!;
}
