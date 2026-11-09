using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class ConfigurationIumService : IConfigurationIumService
{
    private readonly IConfigurationIumRepository _configRepo;
    private readonly IProjetRepository _projetRepo; // Pour vérifier que le projet existe bien

    public ConfigurationIumService(IConfigurationIumRepository configRepo, IProjetRepository projetRepo)
    {
        _configRepo = configRepo;
        _projetRepo = projetRepo;
    }

    public async Task<ConfigurationIumDto?> GetConfigByProjetAsync(int idProjet)
    {
        var config = await _configRepo.GetByProjetIdAsync(idProjet);
        if (config == null) return null;
        return MapToDto(config);
    }

    public async Task<ConfigurationIumDto> DefineConfigAsync(ConfigurationIumCreateDto request)
    {
        // 1. Vérifier si le projet existe
        var projet = await _projetRepo.GetByIdAsync(request.IdProjet);
        if (projet == null) 
            throw new Exception("Le projet spécifié est introuvable.");

        // 2. Chercher la configuration existante
        var existingConfig = await _configRepo.GetByProjetIdAsync(request.IdProjet);

        if (existingConfig != null)
        {
            // UPDATE : La config existe, on la met à jour
            existingConfig.QuotaRequetes = request.QuotaRequetes;
            existingConfig.QuotaTokens = request.QuotaTokens;
            existingConfig.PeriodeQuota = request.PeriodeQuota;
            existingConfig.GenerationCodeAutorisee = request.GenerationCodeAutorisee;
            existingConfig.DateConfiguration = DateTime.UtcNow;

            await _configRepo.UpdateAsync(existingConfig);
            return MapToDto(existingConfig);
        }
        else
        {
            // INSERT : C'est la première fois qu'on configure ce projet
            var newConfig = new ConfigurationIum
            {
                IdProjet = request.IdProjet,
                QuotaRequetes = request.QuotaRequetes,
                QuotaTokens = request.QuotaTokens,
                PeriodeQuota = request.PeriodeQuota,
                GenerationCodeAutorisee = request.GenerationCodeAutorisee,
                DateConfiguration = DateTime.UtcNow
            };

            await _configRepo.AddAsync(newConfig);
            return MapToDto(newConfig);
        }
    }

    private ConfigurationIumDto MapToDto(ConfigurationIum c)
    {
        return new ConfigurationIumDto
        {
            IdConfig = c.IdConfig,
            IdProjet = c.IdProjet,
            QuotaRequetes = c.QuotaRequetes,
            QuotaTokens = c.QuotaTokens,
            PeriodeQuota = c.PeriodeQuota,
            GenerationCodeAutorisee = c.GenerationCodeAutorisee,
            DateConfiguration = c.DateConfiguration
        };
    }
}