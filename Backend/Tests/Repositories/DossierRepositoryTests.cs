using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Reflection;

namespace Tests.Repositories;

public class DossierRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    private Projet CreateProjet(int id = 1) => new Projet
    {
        IdProjet = id,
        Titre = "Projet " + id
    };

    private Dossier CreateDossier(int id, int idProjet, int? parentId = null) => new Dossier
    {
        IdDossier = id,
        Nom = "Dossier " + id,
        IdProjet = idProjet,
        DossierParentId = parentId
    };

    private Fichier CreateFichier(int id, int idDossier) => new Fichier
    {
        IdFichier = id,
        Nom = "Fichier " + id,
        IdDossier = idDossier,
        CreatedBy = "keycloak-user-test-id",
    };

    // GetByProjectIdAsync

    [Fact]
    public async Task GetByProjectIdAsync_Should_Return_Dossiers_Of_Project()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Dossiers.AddRange(
            CreateDossier(1, 1),
            CreateDossier(2, 1),
            CreateDossier(3, 2) // autre projet
        );
        await context.SaveChangesAsync();

        var result = await repo.GetByProjectIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, d => Assert.Equal(1, d.IdProjet));
    }

    [Fact]
    public async Task GetByProjectIdAsync_Should_Include_Fichiers()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Dossiers.Add(CreateDossier(1, 1));
        context.Fichiers.AddRange(
            CreateFichier(1, 1),
            CreateFichier(2, 1)
        );
        await context.SaveChangesAsync();

        var result = await repo.GetByProjectIdAsync(1);
        
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(2, result.First().Fichiers.Count);
    }

    [Fact]
    public async Task GetByProjectIdAsync_Should_Include_Dossiersfils_With_Fichiers()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Dossiers.Add(CreateDossier(1, 1));           // parent
        context.Dossiers.Add(CreateDossier(2, 1, parentId: 1)); // fils
        context.Fichiers.Add(CreateFichier(1, 2));           // fichier dans le fils
        await context.SaveChangesAsync();

        var result = await repo.GetByProjectIdAsync(1);

        var parent = result.FirstOrDefault(d => d.IdDossier == 1);
        Assert.NotNull(parent);
        Assert.Single(parent.Dossiersfils);
        Assert.Single(parent.Dossiersfils.First().Fichiers);
    }

    [Fact]
    public async Task GetByProjectIdAsync_Should_Return_Empty_When_No_Match()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        var result = await repo.GetByProjectIdAsync(999);

        Assert.Empty(result);
    }

    // GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_Should_Return_Dossier_When_Exists()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Dossiers.Add(CreateDossier(1, 1));
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdDossier);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Include_Fichiers()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Dossiers.Add(CreateDossier(1, 1));
        context.Fichiers.AddRange(CreateFichier(1, 1), CreateFichier(2, 1));
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(2, result.Fichiers.Count);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Include_Dossiersfils_With_Fichiers()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Dossiers.Add(CreateDossier(1, 1));
        context.Dossiers.Add(CreateDossier(2, 1, parentId: 1));
        context.Fichiers.Add(CreateFichier(1, 2));
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Single(result.Dossiersfils);
        Assert.Single(result.Dossiersfils.First().Fichiers);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        var result = await repo.GetByIdAsync(999);

        Assert.Null(result);
    }

    // ExistsAsync

    [Fact]
    public async Task ExistsAsync_Should_Return_True_When_Dossier_Exists()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        var dossier = CreateDossier(1, 1);
        dossier.Nom = "MonDossier";
        context.Dossiers.Add(dossier);
        await context.SaveChangesAsync();

        var result = await repo.ExistsAsync("MonDossier", null, 1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_Should_Return_False_When_Nom_Differs()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Dossiers.Add(CreateDossier(1, 1));
        await context.SaveChangesAsync();

        var result = await repo.ExistsAsync("AutreNom", null, 1);

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_Should_Return_False_When_Project_Differs()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        var dossier = CreateDossier(1, 1);
        dossier.Nom = "MonDossier";
        context.Dossiers.Add(dossier);
        await context.SaveChangesAsync();

        var result = await repo.ExistsAsync("MonDossier", null, 999);

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_Should_Match_ParentId()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Dossiers.Add(CreateDossier(1, 1));           // parent
        var fils = CreateDossier(2, 1, parentId: 1);
        fils.Nom = "SousDossier";
        context.Dossiers.Add(fils);
        await context.SaveChangesAsync();

        var existsWithParent    = await repo.ExistsAsync("SousDossier", 1, 1);
        var existsWithoutParent = await repo.ExistsAsync("SousDossier", null, 1);

        Assert.True(existsWithParent);
        Assert.False(existsWithoutParent);
    }

    // AddAsync

    [Fact]
    public async Task AddAsync_Should_Persist_Dossier()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        await context.SaveChangesAsync();

        await repo.AddAsync(CreateDossier(1, 1));

        Assert.Equal(1, await context.Dossiers.CountAsync());
    }

    // DeleteAsync

    [Fact]
    public async Task DeleteAsync_Should_Remove_Dossier()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Dossiers.Add(CreateDossier(1, 1));
        await context.SaveChangesAsync();

        await repo.DeleteAsync(1);

        Assert.Equal(0, await context.Dossiers.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_Should_Do_Nothing_When_Not_Found()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Dossiers.Add(CreateDossier(1, 1));
        await context.SaveChangesAsync();

        await repo.DeleteAsync(999);

        Assert.Equal(1, await context.Dossiers.CountAsync());
    }

    // UpdateAsync

    [Fact]
    public async Task UpdateAsync_Should_Modify_Dossier()
    {
        var context = GetDbContext();
        var repo = new DossierRepository(context);

        context.Projets.Add(CreateProjet(1));
        var dossier = CreateDossier(1, 1);
        context.Dossiers.Add(dossier);
        await context.SaveChangesAsync();

        dossier.Nom = "Nouveau Nom";
        await repo.UpdateAsync(dossier);

        var updated = await context.Dossiers.FindAsync(1);
        Assert.Equal("Nouveau Nom", updated!.Nom);
    }
}