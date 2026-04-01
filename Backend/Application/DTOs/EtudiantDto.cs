namespace Application.DTOs;

public class EtudiantDto : UtilisateurDto
{
    public int IdEtudiant { get; set; }
    public string CodeApogee { get; set; } = string.Empty;
    public string? Niveau { get; set; }
    public string? Filiere { get; set; }
}