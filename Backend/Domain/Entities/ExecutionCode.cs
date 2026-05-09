namespace Domain.Entities;

public class ExecutionCode
{
    public Guid IdExecution { get; set; }

    public string Langage { get; set; } = string.Empty;

    public string Statut { get; set; } = "EnCours";

    public string Sortie { get; set; } = string.Empty;

    public string IdConteneurDocker { get; set; } = string.Empty;

    public DateTime DateDebut { get; set; } = DateTime.UtcNow;

    public DateTime? DateFin { get; set; }

    public string IdUtilisateur { get; set; } = string.Empty;

    // optionnel
    public int? IdProjet { get; set; }

    public Projet? Projet { get; set; }
}