namespace Application.DTOs;

public class ProjetDto
{
    public int Id { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly? DateDebut { get; set; }
    public DateOnly? DateFin { get; set; }
    public string Statut { get; set; } = string.Empty; // Ex: "En cours", "Terminé"
    public string NomEnseignant { get; set; } = string.Empty;
}