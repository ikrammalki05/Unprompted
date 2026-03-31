using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class Prompt
{
    public int IdPrompt { get; set; }

    public string Contenu { get; set; } = null!;

    public DateTime? DatePrompt { get; set; }

    public int? NbTokensEntree { get; set; }

    public int? NbTokensSortie { get; set; }

    public int IdEtudiant { get; set; }

    public int IdProjet { get; set; }

    public virtual Etudiant IdEtudiantNavigation { get; set; } = null!;

    public virtual Projet IdProjetNavigation { get; set; } = null!;

    public virtual ICollection<ReponseIum> ReponseIa { get; set; } = new List<ReponseIum>();
}
