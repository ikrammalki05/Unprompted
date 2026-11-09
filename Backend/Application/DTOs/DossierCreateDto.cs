namespace Application.DTOs;

public class DossierCreateDto
{
    public string Nom { get; set; } = string.Empty;
    public int IdProjet { get; set; }
    public int? DossierParentId { get; set; }
}