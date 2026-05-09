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

    /// <summary>Pourcentage d'avancement du projet (0-100).</summary>
    public int? Progression { get; set; }

    /// <summary>Notes / observations de l'enseignant sur le suivi du projet.</summary>
    public string? NotesEnseignant { get; set; }

    public int? IdEnseignant { get; set; }

    public virtual ConfigurationIum? ConfigurationIum { get; set; }

    public virtual ICollection<Contribution> Contributions { get; set; } = new List<Contribution>();

    public virtual ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();

    public virtual ICollection<Groupe> Groupes { get; set; } = new List<Groupe>();

    public virtual Enseignant? IdEnseignantNavigation { get; set; }

    public virtual ICollection<Prompt> Prompts { get; set; } = new List<Prompt>();

    public ICollection<Dossier>? Dossiers { get; set; } = new List<Dossier>();
    public ICollection<Fichier>? Fichiers { get; set; } = new List<Fichier>();
    public virtual ICollection<ExecutionCode> ExecutionsCode { get; set; } = new List<ExecutionCode>();
}
