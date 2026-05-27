using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Role
{
    public int IdRole { get; set; }

    public string NomRole { get; set; } = null!;

    public virtual ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
}
