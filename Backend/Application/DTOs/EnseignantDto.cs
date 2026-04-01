namespace Application.DTOs;

public class EnseignantDto : UtilisateurDto
{
    public int IdEnseignant { get; set; }
    public string? Specialite { get; set; }
    public string? Departement { get; set; }
}