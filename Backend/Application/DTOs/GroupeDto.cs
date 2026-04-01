namespace Application.DTOs;

public class GroupeDto
{
    public int IdGroupe { get; set; }
    public string NomGroupe { get; set; } = string.Empty;
    public int IdProjet { get; set; }
    public string? NomProjet { get; set; }
}