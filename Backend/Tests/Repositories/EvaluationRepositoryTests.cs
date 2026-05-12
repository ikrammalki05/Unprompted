using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Reflection;

namespace Tests.Repositories;
public class EvaluationRepositoryTests
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
            Titre = "Test Projet",
        };
    }
    private Etudiant CreateEtudiant()
    {
        return new Etudiant
        {
            IdEtudiant = 1,
            CodeApogee = "APG001",
            IdUtilisateur = 1,
            IdUtilisateurNavigation = new Utilisateur
            {
                IdUtilisateur = 1,
                Email = "test@example.com",
                Nom = "Doe",
                Prenom = "John",
                Statut = "Actif"
            }
        };
    }
    private Enseignant CreateEnseignant()
    {
        return new Enseignant
        {
            IdEnseignant = 2,
            IdUtilisateur = 2,
            IdUtilisateurNavigation = new Utilisateur
            {
                IdUtilisateur = 2,
                Email = "ens@example.com",
                Nom = "Doe",
                Prenom = "John",
                Statut = "Actif"
            }
        };
    }
    private Evaluation CreateEvaluation()
    {
        return new Evaluation
        {
            IdEvaluation = 1,
            IdEtudiant = 1,
            IdProjet = 1,
            IdEnseignant = 2,
        };
    }
    [Fact]
    public async Task GetByEtudiantIdAsync_Should_Return_Evaluations()
    {
        var dbContext = GetDbContext();
        var projet = CreateProjet();
        var etudiant = CreateEtudiant();
        var enseignant = CreateEnseignant();
        var evaluation = CreateEvaluation();
        
        dbContext.Projets.Add(projet);
        dbContext.Etudiants.Add(etudiant);
        dbContext.Enseignants.Add(enseignant);
        dbContext.Evaluations.Add(evaluation);
        await dbContext.SaveChangesAsync();

        var repository = new EvaluationRepository(dbContext);

        var result = await repository.GetByEtudiantIdAsync(1);

        Assert.NotNull(result);
        Assert.Single(result);
    }
}