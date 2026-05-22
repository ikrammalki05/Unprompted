using System;

namespace Unprompted.Services.Projet.Domain.Entities;

public class ConfigurationIum
{
    public int IdConfig { get; set; }
    public int? QuotaRequetes { get; set; }
    public int? QuotaTokens { get; set; }
    public string? PeriodeQuota { get; set; }
    public bool? GenerationCodeAutorisee { get; set; }

    // Relation interne au microservice
    public int IdProjet { get; set; }
    public virtual Projet? IdProjetNavigation { get; set; }
}