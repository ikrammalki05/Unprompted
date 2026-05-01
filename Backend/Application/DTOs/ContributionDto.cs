using System;

namespace Application.DTOs;

public class ContributionDto
{
    public int IdContribution { get; set; }
    public string? MessageCommit { get; set; }
    public DateTime? DateCommit { get; set; }
    public string? HashCommit { get; set; }
    public int IdEtudiant { get; set; }
    public int IdProjet { get; set; }
}
