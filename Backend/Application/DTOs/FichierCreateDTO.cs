namespace Application.DTOs;

public class FichierCreateDto
{
    public string Nom { get; set; } = string.Empty;

    public string Extension { get; set; } = string.Empty;

    public string Contenu { get; set; } = string.Empty;

    public int Size { get; set; } = 0;

    public int IdProjet { get; set; }

    public int? IdDossier { get; set; }
}