using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ReponseIum
{
    public int IdReponse { get; set; }

    public string? ContenuReponse { get; set; }

    public DateTime? DateReponse { get; set; }

    public string? ModeleIa { get; set; }

    public int IdPrompt { get; set; }

    public virtual Prompt IdPromptNavigation { get; set; } = null!;
}
