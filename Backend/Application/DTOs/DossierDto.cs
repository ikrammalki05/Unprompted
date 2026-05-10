namespace Application.DTOs;

public class DossierDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public int? DossierParentId { get; set; }
    public int IdProjet { get; set; }
    public DateTime CreatedAt { get; set; }
}