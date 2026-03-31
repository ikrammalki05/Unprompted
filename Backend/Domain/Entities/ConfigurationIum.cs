using System;
using System.Collections.Generic;

namespace Infrastructure;

public partial class ConfigurationIum
{
    public int IdConfig { get; set; }

    public int? QuotaRequetes { get; set; }

    public int? QuotaTokens { get; set; }

    public string? PeriodeQuota { get; set; }

    public bool? GenerationCodeAutorisee { get; set; }

    public DateTime? DateConfiguration { get; set; }

    public int IdProjet { get; set; }

    public virtual Projet IdProjetNavigation { get; set; } = null!;
}
