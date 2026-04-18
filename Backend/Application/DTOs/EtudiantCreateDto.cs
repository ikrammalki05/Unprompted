namespace Application.DTOs;

public class EtudiantCreateDto
{
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CodeApogee { get; set; } = string.Empty;
    public string MotDePasse { get; set; } = string.Empty;
}