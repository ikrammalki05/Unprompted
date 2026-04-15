using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Admin
{
    public int IdAdmin { get; set; }

    public int IdUtilisateur { get; set; }

    public virtual Utilisateur IdUtilisateurNavigation { get; set; } = null!;
}
