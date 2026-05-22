using System;
using System.Collections.Generic;

namespace Unprompted.Services.Projet.Domain.Entities;

public class Groupe
{
    public int IdGroupe { get; set; }
    public string NomGroupe { get; set; } = string.Empty;
    public DateTime? DateCreation { get; set; }
    
    // Relations internes au microservice
    public int IdProjet { get; set; }
    public virtual Projet? IdProjetNavigation { get; set; }

    public virtual ICollection<Affectation> Affectations { get; set; } = new List<Affectation>();
}