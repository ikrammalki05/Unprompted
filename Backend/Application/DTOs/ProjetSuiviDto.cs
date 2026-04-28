using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class ProjetSuiviDto
{
    [Range(0, 100)]
    public int? Progression { get; set; }

    public string? NotesEnseignant { get; set; }
}
