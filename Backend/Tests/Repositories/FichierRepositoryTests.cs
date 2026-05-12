using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Repositories;

public class FichierRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private Fichier CreateFichier(int id, int idProjet, int? idDossier = null, string nom = null) => new Fichier
    {
        IdFichier = id,
        Nom = nom ?? "Fichier " + id,
        IdProjet = idProjet,
        IdDossier = idDossier,
        CreatedBy = "keycloak-user-test-id"
    };

    private Dossier CreateDossier(int id, int idProjet) => new Dossier
    {
        IdDossier = id,
        Nom = "Dossier " + id,
        IdProjet = idProjet
    };

    private Projet CreateProjet(int id = 1) => new Projet
    {
        IdProjet = id,
        Titre = "Projet " + id
    };

    // ─── GetByProjectIdAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetByProjectIdAsync_Should_Return_Fichiers_Of_Project()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        context.Projets.AddRange(CreateProjet(1), CreateProjet(2));
        context.Fichiers.AddRange(
            CreateFichier(1, 1),
            CreateFichier(2, 1),
            CreateFichier(3, 2) // autre projet
        );
        await context.SaveChangesAsync();

        var result = await repo.GetByProjectIdAsync(1);

        Assert.Equal(2, result.Count());
        Assert.All(result, f => Assert.Equal(1, f.IdProjet));
    }

    [Fact]
    public async Task GetByProjectIdAsync_Should_Return_Empty_When_No_Match()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        var result = await repo.GetByProjectIdAsync(999);

        Assert.Empty(result);
    }

    // ─── GetByIdAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_Should_Return_Fichier_When_Exists()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Fichiers.Add(CreateFichier(1, 1));
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdFichier);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        var result = await repo.GetByIdAsync(999);

        Assert.Null(result);
    }

    // ─── ExistsAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task ExistsAsync_Should_Return_True_When_Fichier_Exists()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Fichiers.Add(CreateFichier(1, 1, nom: "readme.md"));
        await context.SaveChangesAsync();

        var result = await repo.ExistsAsync("readme.md", null, 1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_Should_Return_False_When_Nom_Differs()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Fichiers.Add(CreateFichier(1, 1, nom: "readme.md"));
        await context.SaveChangesAsync();

        var result = await repo.ExistsAsync("autre.md", null, 1);

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_Should_Return_False_When_Project_Differs()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Fichiers.Add(CreateFichier(1, 1, nom: "readme.md"));
        await context.SaveChangesAsync();

        var result = await repo.ExistsAsync("readme.md", null, 999);

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_Should_Match_DossierId()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Dossiers.Add(CreateDossier(1, 1));
        context.Fichiers.Add(CreateFichier(1, 1, idDossier: 1, nom: "readme.md"));
        await context.SaveChangesAsync();

        var existsWithDossier    = await repo.ExistsAsync("readme.md", 1, 1);
        var existsWithoutDossier = await repo.ExistsAsync("readme.md", null, 1);

        Assert.True(existsWithDossier);
        Assert.False(existsWithoutDossier);
    }

    // ─── AddAsync ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task AddAsync_Should_Persist_Fichier()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        context.Projets.Add(CreateProjet(1));
        await context.SaveChangesAsync();

        await repo.AddAsync(CreateFichier(1, 1));

        Assert.Equal(1, await context.Fichiers.CountAsync());
    }

    // ─── UpdateAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_Should_Modify_Fichier()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        context.Projets.Add(CreateProjet(1));
        var fichier = CreateFichier(1, 1);
        context.Fichiers.Add(fichier);
        await context.SaveChangesAsync();

        fichier.Nom = "nouveau_nom.md";
        await repo.UpdateAsync(fichier);

        var updated = await context.Fichiers.FindAsync(1);
        Assert.Equal("nouveau_nom.md", updated!.Nom);
    }

    // ─── DeleteAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_Should_Remove_Fichier()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Fichiers.Add(CreateFichier(1, 1));
        await context.SaveChangesAsync();

        await repo.DeleteAsync(1);

        Assert.Equal(0, await context.Fichiers.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_Should_Do_Nothing_When_Not_Found()
    {
        var context = GetDbContext();
        var repo = new FichierRepository(context);

        context.Projets.Add(CreateProjet(1));
        context.Fichiers.Add(CreateFichier(1, 1));
        await context.SaveChangesAsync();

        await repo.DeleteAsync(999);

        Assert.Equal(1, await context.Fichiers.CountAsync());
    }
}