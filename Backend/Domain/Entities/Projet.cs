using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Projet
{
    public int IdProjet { get; set; }

    public string Titre { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? DateDebut { get; set; }

    public DateOnly? DateFin { get; set; }

    public string? Statut { get; set; }

    public int? Duree { get; set; }

    public string? UrlGit { get; set; }

    public int? IdEnseignant { get; set; }

    public virtual ConfigurationIum? ConfigurationIum { get; set; }

    public virtual ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();

    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();

    public virtual ICollection<Groupe> Groupes { get; set; } = new List<Groupe>();

    public virtual Enseignant? IdEnseignantNavigation { get; set; }

    public virtual ICollection<Prompt> Prompts { get; set; } = new List<Prompt>();
}
