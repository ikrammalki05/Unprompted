using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Reflection;

namespace Tests.Repositories;
public class ContributionRepositoryTests
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
    private Utilisateur CreateUtilisateur()
    {
        return new Utilisateur
        {
            IdUtilisateur = 1,
            Nom = "Test",
            Prenom = "Test",
            Statut = "Actif",
            Email = "test@example.com"
        };
    }
    private Etudiant CreateEtudiant()
    {
        return new Etudiant
        {
            IdEtudiant = 1,
            CodeApogee = "E12345",
            IdUtilisateur = 1
        };
    }
    private Contribution CreateContribution()
    {
        return new Contribution
        {
            IdContribution = 1,
            IdEtudiant = 1,
            IdProjet = 1,
            MessageCommit = "Initial commit"
        };
    }

    [Fact]
    public async Task GetByEtudiantIdAsync_Should_Return_Contributions()
    {
        var context = GetDbContext();
        var repo = new ContributionRepository(context);
        
        var projet = CreateProjet();
        var utilisateur = CreateUtilisateur();
        var etudiant = CreateEtudiant();
        var contribution = CreateContribution();
        context.Projets.Add(projet);
        context.Utilisateurs.Add(utilisateur);
        context.Etudiants.Add(etudiant);
        context.Contributions.AddRange(contribution);
        await context.SaveChangesAsync();

        var result = await repo.GetByEtudiantIdAsync(1);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByProjetIdAsync_Should_Return_Contributions()
    {
        var context = GetDbContext();
        var repo = new ContributionRepository(context);

        var projet = CreateProjet();
        var utilisateur = CreateUtilisateur();
        var etudiant = CreateEtudiant();
        var contribution = CreateContribution();
        context.Projets.Add(projet);
        context.Utilisateurs.Add(utilisateur);
        context.Etudiants.Add(etudiant);
        context.Contributions.AddRange(contribution);
        await context.SaveChangesAsync();

        var result = await repo.GetByProjetIdAsync(1);

        Assert.NotNull(result);
        Assert.Single(result);
    }
}