namespace Application.DTOs;

public class FichierDto
{
    public int Id { get; set; }
    public string? Nom { get; set; } = string.Empty;
    public string? Extension { get; set; } = string.Empty;
    public string? Contenu { get; set; } = string.Empty;
    public long Size { get; set; }
    public string? CreatedBy { get; set; }
    public string? Language { get; set; }
    public int? Version { get; set; } = 1;
    public int IdProjet { get; set; }
    public int? IdDossier { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DerniereModification { get; set; }
}