namespace Application.DTOs;

public class FichierVersionDto
{
    public int Id { get; set; }

    public int IdFichier { get; set; }

    public int Version { get; set; }

    public string Contenu { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}