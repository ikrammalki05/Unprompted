namespace Application.DTOs;

public class EtudiantDto
{
    public int Id { get; set; }
    public string CodeApogee { get; set; } = string.Empty;
    public string NomComplet { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ClasseNom { get; set; } = "Non assigné"; // Ex: "Master IABD - G1"
    public string Statut { get; set; } = "Inactif"; // Actif ou Inactif
}