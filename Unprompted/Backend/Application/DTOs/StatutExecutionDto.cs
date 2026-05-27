namespace Application.DTOs;

public class StatutExecutionDto
{
    public Guid IdExecution { get; set; }

    public string Statut { get; set; } = string.Empty;

    public string Sortie { get; set; } = string.Empty;
}