using System;

namespace Application.DTOs;

public class ContributionCreateDto
{
    public string MessageCommit { get; set; } = string.Empty;
    public DateTime? DateCommit { get; set; }
    public string? HashCommit { get; set; }
    public int? LignesAjoutees { get; set; }
    public int? LignesSupprimees { get; set; }
}
