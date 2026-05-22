using System;
using System.Collections.Generic;

namespace Unprompted.Services.Projet.Domain.Entities;

public class Projet
{
    public int IdProjet { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public string? Statut { get; set; }
    public string? CahierDesChargesPath { get; set; }

    // RUPTURE : On remplace l'objet EnseignantNavigation par un simple ID numérique
    public int IdEnseignant { get; set; }

    // Les relations internes au microservice restent valides
    public virtual ConfigurationIum? ConfigurationIum { get; set; }
    public virtual ICollection<Groupe> Groupes { get; set; } = new List<Groupe>();
}