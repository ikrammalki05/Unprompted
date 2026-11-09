namespace Application.DTOs;

public class EtudiantProfilDto
{
    public int IdEtudiant { get; set; }
    public string NomComplet { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CodeApogee { get; set; } = string.Empty;
    public string? Niveau { get; set; }
    public string? Filiere { get; set; }
    
    // On ajoute le statut pour qu'il sache si son compte est bien actif
    public string Statut { get; set; } = string.Empty; 
}