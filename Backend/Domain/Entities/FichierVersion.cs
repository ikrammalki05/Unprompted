using Domain.Entities;

namespace Domain.Entities;

public class FichierVersion
{
    public int IdFichierVersion { get; set; }

    public int IdFichier { get; set; }
    public Fichier Fichier { get; set; } = null!;

    public string Contenu { get; set; } = string.Empty;

    public int Version { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}