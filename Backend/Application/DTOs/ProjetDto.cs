namespace Application.DTOs;
public class ProjetDto
{
    public int Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateOnly? DateDebut { get; set; }
    public DateOnly? DateFin { get; set; }
    public int? Duree { get; set; }
    public string? UrlGit { get; set; }
    public int? Progression { get; set; }
    public string? NotesEnseignant { get; set; }
    public int? IdEnseignant { get; set; }
}