using System;
using System.Collections.Generic;

namespace Domain.Entities;

public class Fichier
{
    public int IdFichier { get; set; }

    public string? Nom { get; set; }

    public string? Extension { get; set; }

    public string? Language { get; set; }

    public string? Contenu { get; set; }
    public string? ContentHash { get; set; }

    public long Size { get; set; }

    public int Version { get; set; } = 1;

    public ICollection<FichierVersion> Versions { get; set; } = new List<FichierVersion>();

    public DateTime DerniereModification { get; set; } = DateTime.UtcNow;

    public int IdProjet { get; set; }
    public Projet? Projet { get; set; }

    public int? IdDossier { get; set; }
    public Dossier? Dossier { get; set; }

    public required string CreatedBy { get; set; } // Keycloak User Id

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}