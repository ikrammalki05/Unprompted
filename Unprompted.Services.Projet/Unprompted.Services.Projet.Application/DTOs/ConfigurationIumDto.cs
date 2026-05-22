using System;

namespace Unprompted.Services.Projet.Application.DTOs;

public class ConfigurationIumDto
{
    public int IdConfig { get; set; }
    public int? QuotaRequetes { get; set; }
    public int? QuotaTokens { get; set; }
    public string? PeriodeQuota { get; set; }
    public bool? GenerationCodeAutorisee { get; set; }
    public int IdProjet { get; set; }
}