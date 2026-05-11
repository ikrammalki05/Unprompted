namespace Application.DTOs;

public class EnseignantDto
{
    public int IdEnseignant { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Specialite { get; set; }
    public string? Departement { get; set; }
    public string Statut { get; set; } = "Actif";
}
