namespace Application.DTOs;

public class UtilisateurDto
{
    public int IdUtilisateur { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Statut { get; set; } = string.Empty;
}