namespace Application.DTOs;

public class EtudiantRoleDto
{
    public int IdEtudiant { get; set; }
    public int IdRole { get; set; }
}

public class GroupeCreateDto
{
    public string NomGroupe { get; set; } = null!;
    public int? IdProjet { get; set; }
    public List<EtudiantRoleDto> Etudiants { get; set; } = new();
}
