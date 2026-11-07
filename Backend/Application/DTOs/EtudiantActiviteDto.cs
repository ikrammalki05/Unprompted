using System.Collections.Generic;

namespace Application.DTOs;

public class EtudiantActiviteDto
{
    public IEnumerable<PromptDto> Prompts { get; set; } = new List<PromptDto>();
    public IEnumerable<ContributionDto> Contributions { get; set; } = new List<ContributionDto>();
}
