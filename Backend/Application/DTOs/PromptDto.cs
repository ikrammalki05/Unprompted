using System;

namespace Application.DTOs;

public class PromptDto
{
    public int IdPrompt { get; set; }
    public string Contenu { get; set; } = null!;
    public DateTime? DatePrompt { get; set; }
    public int? NbTokensEntree { get; set; }
    public int? NbTokensSortie { get; set; }
    public int IdEtudiant { get; set; }
    public int IdProjet { get; set; }
}
