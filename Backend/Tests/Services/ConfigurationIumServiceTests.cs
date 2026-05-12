using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Services;

public class ConfigurationIumServiceTests
{
    private readonly Mock<IConfigurationIumRepository> _configRepoMock;
    private readonly Mock<IProjetRepository> _projetRepoMock;
    private readonly ConfigurationIumService _service;

    public ConfigurationIumServiceTests()
    {
        _configRepoMock = new Mock<IConfigurationIumRepository>();
        _projetRepoMock = new Mock<IProjetRepository>();

        _service = new ConfigurationIumService(
            _configRepoMock.Object,
            _projetRepoMock.Object
        );
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private Projet CreateProjet(int id = 1) => new Projet
    {
        IdProjet = id,
        Titre = "Projet " + id
    };

    private ConfigurationIum CreateConfig(int idProjet = 1) => new ConfigurationIum
    {
        IdConfig = 1,
        IdProjet = idProjet,
        QuotaRequetes = 100,
        QuotaTokens = 5000,
        PeriodeQuota = "Mensuel",
        GenerationCodeAutorisee = true,
        DateConfiguration = DateTime.UtcNow
    };

    private ConfigurationIumCreateDto CreateConfigDto(int idProjet = 1) => new ConfigurationIumCreateDto
    {
        IdProjet = idProjet,
        QuotaRequetes = 100,
        QuotaTokens = 5000,
        PeriodeQuota = "Mensuel",
        GenerationCodeAutorisee = true
    };

    // ─── GetConfigByProjetAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetConfigByProjetAsync_Should_Return_Dto_When_Found()
    {
        _configRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(CreateConfig(1));

        var result = await _service.GetConfigByProjetAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdProjet);
        Assert.Equal(100, result.QuotaRequetes);
        Assert.Equal(5000, result.QuotaTokens);
        Assert.Equal("Mensuel", result.PeriodeQuota);
        Assert.True(result.GenerationCodeAutorisee);
    }

    [Fact]
    public async Task GetConfigByProjetAsync_Should_Return_Null_When_Not_Found()
    {
        _configRepoMock
            .Setup(r => r.GetByProjetIdAsync(99))
            .ReturnsAsync((ConfigurationIum?)null);

        var result = await _service.GetConfigByProjetAsync(99);

        Assert.Null(result);
    }

    // ─── DefineConfigAsync : projet introuvable ────────────────────────────────

    [Fact]
    public async Task DefineConfigAsync_Should_Throw_When_Projet_Not_Found()
    {
        var dto = CreateConfigDto(99);

        _projetRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Projet?)null);

        await Assert.ThrowsAsync<Exception>(
            () => _service.DefineConfigAsync(dto)
        );
    }

    [Fact]
    public async Task DefineConfigAsync_Should_Not_Call_Repo_When_Projet_Not_Found()
    {
        var dto = CreateConfigDto(99);

        _projetRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Projet?)null);

        try { await _service.DefineConfigAsync(dto); } catch { }

        _configRepoMock.Verify(r => r.AddAsync(It.IsAny<ConfigurationIum>()), Times.Never);
        _configRepoMock.Verify(r => r.UpdateAsync(It.IsAny<ConfigurationIum>()), Times.Never);
    }

    // ─── DefineConfigAsync : INSERT (première configuration) ──────────────────

    [Fact]
    public async Task DefineConfigAsync_Should_Call_AddAsync_When_No_Existing_Config()
    {
        var dto = CreateConfigDto(1);

        _projetRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(CreateProjet(1));

        _configRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync((ConfigurationIum?)null); // pas de config existante

        _configRepoMock
            .Setup(r => r.AddAsync(It.IsAny<ConfigurationIum>()))
            .Returns(Task.CompletedTask);

        await _service.DefineConfigAsync(dto);

        _configRepoMock.Verify(
            r => r.AddAsync(It.Is<ConfigurationIum>(c =>
                c.IdProjet == 1 &&
                c.QuotaRequetes == 100 &&
                c.QuotaTokens == 5000 &&
                c.PeriodeQuota == "Mensuel" &&
                c.GenerationCodeAutorisee == true
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task DefineConfigAsync_Should_Not_Call_UpdateAsync_When_No_Existing_Config()
    {
        var dto = CreateConfigDto(1);

        _projetRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(CreateProjet(1));

        _configRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync((ConfigurationIum?)null);

        _configRepoMock
            .Setup(r => r.AddAsync(It.IsAny<ConfigurationIum>()))
            .Returns(Task.CompletedTask);

        await _service.DefineConfigAsync(dto);

        _configRepoMock.Verify(r => r.UpdateAsync(It.IsAny<ConfigurationIum>()), Times.Never);
    }

    [Fact]
    public async Task DefineConfigAsync_Should_Return_Dto_After_Insert()
    {
        var dto = CreateConfigDto(1);

        _projetRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(CreateProjet(1));

        _configRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync((ConfigurationIum?)null);

        _configRepoMock
            .Setup(r => r.AddAsync(It.IsAny<ConfigurationIum>()))
            .Returns(Task.CompletedTask);

        var result = await _service.DefineConfigAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdProjet);
        Assert.Equal(100, result.QuotaRequetes);
        Assert.Equal(5000, result.QuotaTokens);
        Assert.Equal("Mensuel", result.PeriodeQuota);
        Assert.True(result.GenerationCodeAutorisee);
    }

    // ─── DefineConfigAsync : UPDATE (config existante) ────────────────────────

    [Fact]
    public async Task DefineConfigAsync_Should_Call_UpdateAsync_When_Config_Exists()
    {
        var existingConfig = CreateConfig(1);
        var dto = new ConfigurationIumCreateDto
        {
            IdProjet = 1,
            QuotaRequetes = 200,
            QuotaTokens = 10000,
            PeriodeQuota = "Hebdomadaire",
            GenerationCodeAutorisee = false
        };

        _projetRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(CreateProjet(1));

        _configRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(existingConfig); // config déjà existante

        _configRepoMock
            .Setup(r => r.UpdateAsync(It.IsAny<ConfigurationIum>()))
            .Returns(Task.CompletedTask);

        await _service.DefineConfigAsync(dto);

        _configRepoMock.Verify(
            r => r.UpdateAsync(It.Is<ConfigurationIum>(c =>
                c.QuotaRequetes == 200 &&
                c.QuotaTokens == 10000 &&
                c.PeriodeQuota == "Hebdomadaire" &&
                c.GenerationCodeAutorisee == false
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task DefineConfigAsync_Should_Not_Call_AddAsync_When_Config_Exists()
    {
        var dto = CreateConfigDto(1);

        _projetRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(CreateProjet(1));

        _configRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(CreateConfig(1));

        _configRepoMock
            .Setup(r => r.UpdateAsync(It.IsAny<ConfigurationIum>()))
            .Returns(Task.CompletedTask);

        await _service.DefineConfigAsync(dto);

        _configRepoMock.Verify(r => r.AddAsync(It.IsAny<ConfigurationIum>()), Times.Never);
    }

    [Fact]
    public async Task DefineConfigAsync_Should_Return_Updated_Dto_When_Config_Exists()
    {
        var existingConfig = CreateConfig(1);
        var dto = new ConfigurationIumCreateDto
        {
            IdProjet = 1,
            QuotaRequetes = 200,
            QuotaTokens = 10000,
            PeriodeQuota = "Hebdomadaire",
            GenerationCodeAutorisee = false
        };

        _projetRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(CreateProjet(1));

        _configRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(existingConfig);

        _configRepoMock
            .Setup(r => r.UpdateAsync(It.IsAny<ConfigurationIum>()))
            .Returns(Task.CompletedTask);

        var result = await _service.DefineConfigAsync(dto);

        Assert.NotNull(result);
        Assert.Equal(200, result.QuotaRequetes);
        Assert.Equal(10000, result.QuotaTokens);
        Assert.Equal("Hebdomadaire", result.PeriodeQuota);
        Assert.False(result.GenerationCodeAutorisee);
    }
}