using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class Admin
{
    public int IdAdmin { get; set; }

    public int IdUtilisateur { get; set; }

    public virtual Utilisateur IdUtilisateurNavigation { get; set; } = null!;
}
