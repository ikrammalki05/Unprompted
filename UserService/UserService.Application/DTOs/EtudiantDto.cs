namespace Application.DTOs;

public class EtudiantDto
{
    public int IdEtudiant { get; set; }
    public string CodeApogee { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Niveau { get; set; }
    public string? Filiere { get; set; }
    public string Statut { get; set; } = "Actif";
}
