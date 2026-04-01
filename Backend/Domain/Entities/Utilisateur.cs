using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class Utilisateur
{
    public int IdUtilisateur { get; set; }

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string MotDePasse { get; set; } = null!;

    public string Statut { get; set; } = null!;

    public DateTime? DateCreation { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Enseignant? Enseignant { get; set; }

    public virtual Etudiant? Etudiant { get; set; }
}
