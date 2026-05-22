using System.Threading.Tasks;
using Unprompted.Services.Projet.Application.DTOs;
using Unprompted.Services.Projet.Application.Interfaces;

namespace Unprompted.Services.Projet.Application.Services;

public class ConfigurationIumService
{
    private readonly IConfigurationIumRepository _configRepository;

    public ConfigurationIumService(IConfigurationIumRepository configRepository)
    {
        _configRepository = configRepository;
    }

    public async Task<ConfigurationIumDto?> GetByProjetIdAsync(int projetId)
    {
        var config = await _configRepository.GetByProjetIdAsync(projetId);

        if (config == null) return null;

        return new ConfigurationIumDto
        {
            IdConfig = config.IdConfig,
            QuotaRequetes = config.QuotaRequetes,
            QuotaTokens = config.QuotaTokens,
            PeriodeQuota = config.PeriodeQuota,
            GenerationCodeAutorisee = config.GenerationCodeAutorisee,
            IdProjet = config.IdProjet
        };
    }
}