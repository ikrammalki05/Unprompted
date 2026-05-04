namespace Application.DTOs;

public class FichierDto
{
    public int Id { get; set; }
    public string? Nom { get; set; } = string.Empty;
    public string? Extension { get; set; } = string.Empty;
    public string? Contenu { get; set; } = string.Empty;
    public int? Version { get; set; } = 1;
    public DateTime DerniereModification { get; set; }
}