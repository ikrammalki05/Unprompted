using System;

namespace Application.DTOs;

public class PromptCreateDto
{
    public string Contenu { get; set; } = null!;
    public int IdEtudiant { get; set; }
    public int? NbTokensEntree { get; set; }
    public int? NbTokensSortie { get; set; }
}
