namespace Application.DTOs;

public class EnseignantCreateDto
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialite { get; set; } = string.Empty;
    public string MotDePasse { get; set; } = string.Empty;
}