namespace Application.DTOs;
public class ProjetDto
{
    public int IDProjet { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Status { get; set; }
    public DateOnly? DateDebut { get; set; }
    public DateOnly? DateFin { get; set; }
    public int? Duree { get; set; }
    public string? UrlGit { get; set; }
    public int? Progression { get; set; }
    public string? NotesEnseignant { get; set; }
    public int? IdEnseignant { get; set; }
    public List<EtudiantGroupeDto> Membres { get; set; } = new();
    public string? Objectifs { get; set; }
public string? Livrables { get; set; }
public string? CriteresEvaluation { get; set; }
public string? TechnologiesRequises { get; set; }
public string? Contraintes { get; set; }
public string? RessourcesDisponibles { get; set; }
public bool HasCahierDesCharges { get; set; } // true/false si PDF existe
}