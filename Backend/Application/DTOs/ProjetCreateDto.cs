namespace Application.DTOs;

public class ProjetCreateDto
{
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly? DateDebut { get; set; }
    public DateOnly? DateFin { get; set; }
    public int IdEnseignant { get; set; } // L'enseignant qui crée le projet
}