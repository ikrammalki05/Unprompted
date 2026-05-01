namespace Application.DTOs;

public class GroupeDto
{
    public int IdGroupe { get; set; }
    public string NomGroupe { get; set; } = null!;
    public int? IdProjet { get; set; }
    public List<EtudiantGroupeDto> Etudiants { get; set; } = new();
}

public class EtudiantGroupeDto
{
    public int IdEtudiant { get; set; }
    public string NomComplet { get; set; } = null!;
    public string Role { get; set; } = null!;
}
