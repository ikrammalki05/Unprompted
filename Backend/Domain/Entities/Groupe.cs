using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Groupe
{
    public int IdGroupe { get; set; }

    public string NomGroupe { get; set; } = null!;

    public int IdProjet { get; set; }

    public virtual ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();

    public virtual Projet IdProjetNavigation { get; set; } = null!;
}
