namespace Application.DTOs;

public class AssignationProjetRequestDto
{
    public int IdProjet { get; set; }
    public string NomGroupe { get; set; } = string.Empty; // Ex: "Équipe Alpha"
    
    // La liste des étudiants à mettre dans ce groupe
    public List<EtudiantRoleDto> Etudiants { get; set; } = new();
}

public class EtudiantRoleDto
{
    public int IdEtudiant { get; set; }
    public int IdRole { get; set; } // 1: Chef, 2: Dev Front, 3: Dev Back, etc.
}