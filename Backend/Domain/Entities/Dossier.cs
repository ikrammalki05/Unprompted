using System;
using System.Collections.Generic;

namespace Domain.Entities;
public class Dossier
{
    public int IdDossier { get; set; }

    public required string Nom { get; set; }

    public int IdProjet { get; set; }
    public Projet? Projet { get; set; }

    public int? DossierParentId { get; set; }
    public Dossier? DossierParent { get; set; }

    public ICollection<Dossier> Dossiersfils { get; set; } = new List<Dossier>();

    public ICollection<Fichier>? Fichiers { get; set; } = new List<Fichier>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}