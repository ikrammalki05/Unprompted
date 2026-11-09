namespace Application.DTOs;

public class ExecutionProjetDto
{
    public string Langage { get; set; } = string.Empty;

    public List<FichierProjetDto> Fichiers { get; set; } = new();
}