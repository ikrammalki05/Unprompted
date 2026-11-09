using Application.DTOs;

namespace Application.Interfaces;

public interface IConfigurationIumService
{
    Task<ConfigurationIumDto?> GetConfigByProjetAsync(int idProjet);
    Task<ConfigurationIumDto> DefineConfigAsync(ConfigurationIumCreateDto request);
}