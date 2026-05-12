using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Reflection;

namespace Tests.Repositories;
public class ConfigurationIumRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
    private Projet CreateProjet()
    {
        return new Projet
        {
            IdProjet = 1,
            Titre = "Projet Test",
        };
    }
    private ConfigurationIum CreateConfigurationIum()
    {
        return new ConfigurationIum
        {
            IdConfig = 1,
            IdProjet = 1,
        };
    }
    [Fact]
    public async Task AddAsync_Should_Add_ConfigurationIum()
    {
        var context = GetDbContext();
        var repo = new ConfigurationIumRepository(context);

        var projet = CreateProjet();
        var configurationIum = CreateConfigurationIum();

        context.Projets.Add(projet);
        await context.SaveChangesAsync();

        await repo.AddAsync(configurationIum);

        var result = await context.ConfigurationIa.FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.Equal(1, result.IdConfig);
        Assert.Equal(1, result.IdProjetNavigation.IdProjet);
    }
    [Fact]
    public async Task GetByProjetIdAsync_Should_Return_ConfigurationIum()
    {
        var context = GetDbContext();
        var repo = new ConfigurationIumRepository(context);

        var projet = CreateProjet();
        var configurationIum = CreateConfigurationIum();

        context.Projets.Add(projet);
        context.ConfigurationIa.Add(configurationIum);
        await context.SaveChangesAsync();

        var result = await repo.GetByProjetIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdConfig);
        Assert.Equal(1, result.IdProjetNavigation.IdProjet);
    }
    [Fact]
    public async Task GetByProjetIdAsync_Should_Return_Null_If_Not_Found()
    {
        var context = GetDbContext();
        var repo = new ConfigurationIumRepository(context);

        var result = await repo.GetByProjetIdAsync(2);

        Assert.Null(result);
    }
    [Fact]
    public async Task UpdateAsync_Should_Update_ConfigurationIum()
    {
        var context = GetDbContext();
        var repo = new ConfigurationIumRepository(context);

        var projet = CreateProjet();
        var configurationIum = CreateConfigurationIum();

        context.Projets.Add(projet);
        context.ConfigurationIa.Add(configurationIum);
        await context.SaveChangesAsync();

        configurationIum.QuotaRequetes = 2;
        await repo.UpdateAsync(configurationIum);

        var result = await context.ConfigurationIa.FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.QuotaRequetes);  
    }
}