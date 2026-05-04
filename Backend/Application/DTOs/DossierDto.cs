namespace Application.DTOs;

public class DossierDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public int? DossierParentId { get; set; }
}