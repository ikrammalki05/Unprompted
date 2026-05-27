using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Reflection;

namespace Tests.Repositories;
public class EtudiantRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
    private Utilisateur CreateUtilisateur()
    {
        return new Utilisateur
        {
            IdUtilisateur = 1,
            Nom = "Test",
            Prenom = "Test",
            Email = "test@example.com",
            Statut = "Actif",
        };
    }
    private Etudiant CreateEtudiant()
    {
        return new Etudiant
        {
            IdEtudiant = 1,
            CodeApogee = "E12345",
            Filiere = "Info",
            IdUtilisateur = 1
        };
    }

    [Fact]
    public async Task AddAsync_Should_Add_Etudiant()
    {
        var context = GetDbContext();
        var repo = new EtudiantRepository(context);

        var utilisateur = CreateUtilisateur();
        var etudiant = CreateEtudiant();

        context.Utilisateurs.Add(utilisateur);
        await context.SaveChangesAsync();

        await repo.AddAsync(etudiant);

        var result = await context.Etudiants.FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.Equal(1, result.IdEtudiant);
        Assert.Equal("E12345", result.CodeApogee);
        Assert.Equal(1, result.IdUtilisateurNavigation.IdUtilisateur);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Etudiant_With_User()
    {
        var context = GetDbContext();

        var utilisateur = CreateUtilisateur();
        var etudiant = CreateEtudiant();

        context.Utilisateurs.Add(utilisateur);
        context.Etudiants.Add(etudiant);
        await context.SaveChangesAsync();

        var repo = new EtudiantRepository(context);

        var result = await repo.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.NotNull(result!.IdUtilisateurNavigation);
        Assert.Equal(1, result.IdEtudiant);
    }

    [Fact]
    public async Task GetByFiliereAsync_Should_Filter_By_Filiere()
    {
        var context = GetDbContext();

        var utilisateur = CreateUtilisateur();
        var etudiant = CreateEtudiant();
        context.Utilisateurs.Add(utilisateur);
        context.Etudiants.Add(etudiant);

        await context.SaveChangesAsync();

        var repo = new EtudiantRepository(context);

        var result = await repo.GetByFiliereAsync("Info");

        Assert.Single(result);
    }

    [Fact]
    public async Task GetByStatutAsync_Should_Filter_By_Statut()
    {
        var context = GetDbContext();

        var utilisateur = CreateUtilisateur();
        var etudiant = CreateEtudiant();
        context.Utilisateurs.Add(utilisateur);
        context.Etudiants.Add(etudiant);

        await context.SaveChangesAsync();

        var repo = new EtudiantRepository(context);

        var result = await repo.GetByStatutAsync("Actif");

        Assert.Single(result);
    }

    [Fact]
    public async Task CountAsync_Should_Return_Number_Of_Etudiants()
    {
        var context = GetDbContext();

        var utilisateur = CreateUtilisateur();
        var etudiant = CreateEtudiant();
        context.Utilisateurs.Add(utilisateur);
        context.Etudiants.Add(etudiant);

        await context.SaveChangesAsync();

        var repo = new EtudiantRepository(context);

        var result = await repo.CountAsync();

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Etudiant()
    {
        var context = GetDbContext();
        
        var utilisateur = CreateUtilisateur();
        var etudiant = CreateEtudiant();
        context.Utilisateurs.Add(utilisateur);
        context.Etudiants.Add(etudiant);
        
        await context.SaveChangesAsync();

        var repo = new EtudiantRepository(context);

        await repo.DeleteAsync(1);

        var result = await context.Etudiants.FindAsync(1);

        Assert.Null(result);
    }
}