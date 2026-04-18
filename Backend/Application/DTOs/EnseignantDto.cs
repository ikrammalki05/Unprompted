namespace Application.DTOs;

public class EnseignantDto
{
    public int Id { get; set; }
    public string NomComplet { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialite { get; set; } = string.Empty;
    public List<string> ClassesAssignees { get; set; } = new List<string>(); // Ex: ["M1 IABD", "M2 Data"]
    public string Statut { get; set; } = "Inactif";
    public string MotDePasse { get; set; } = string.Empty;
}