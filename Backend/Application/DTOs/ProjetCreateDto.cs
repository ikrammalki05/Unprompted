namespace Application.DTOs;

public class ProjetCreateDto
{
    public string Titre {get ; set;} = string.Empty;
    public string? Description {get;set;}
    public DateOnly? DateDebut {get; set;}
    public DateOnly? DateFin {get; set;}
    public string? UrlGit{get; set;}
    public int? Duree {get; set;}
}