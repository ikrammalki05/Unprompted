using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Repositories;

public class ProjetRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }
    private Utilisateur CreateUtilisateur(int id = 1) => new Utilisateur
    {
        IdUtilisateur = id,
        Nom = "Nom" + id,
        Prenom = "Prenom" + id,
        Statut = "Actif",
        Email = $"user{id}@example.com"
    };

    private Enseignant CreateEnseignant(int id = 1, int idUtilisateur = 1) => new Enseignant
    {
        IdEnseignant = id,
        IdUtilisateur = idUtilisateur
    };

    private Etudiant CreateEtudiant(int id = 1, int idUtilisateur = 1) => new Etudiant
    {
        IdEtudiant = id,
        CodeApogee = "E" + id,
        IdUtilisateur = idUtilisateur
    };

    private Projet CreateProjet(int id = 1, int idEnseignant = 1) => new Projet
    {
        IdProjet = id,
        Titre = "Projet " + id,
        IdEnseignant = idEnseignant
    };

    private Groupe CreateGroupe(int id = 1, int idProjet = 1) => new Groupe
    {
        IdGroupe = id,
        IdProjet = idProjet,
        NomGroupe = "Groupe " + id
    };

    private Role CreateRole(int id = 1) => new Role
    {
        IdRole = id,
        NomRole = "Role " + id
    };

    private Affectation CreateAffectation(int id, int idEtudiant, int idGroupe, int idRole = 1) => new Affectation
    {
        IdAffectation = id,
        IdEtudiant = idEtudiant,
        IdGroupe = idGroupe,
        IdRole = idRole
    };

    private Contribution CreateContribution(int id, int idProjet, int idEtudiant) => new Contribution
    {
        IdContribution = id,
        IdProjet = idProjet,
        IdEtudiant = idEtudiant,
        MessageCommit = "Commit " + id
    };

    private Prompt CreatePrompt(int id, int idProjet, int idEtudiant) => new Prompt
    {
        IdPrompt = id,
        IdProjet = idProjet,
        IdEtudiant = idEtudiant,
        Contenu = "Prompt " + id
    };

    // GetAllAsync

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Projets()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Projets.AddRange(CreateProjet(1, 1), CreateProjet(2, 1));
        await context.SaveChangesAsync();

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_Empty_When_No_Projets()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        var result = await repo.GetAllAsync();

        Assert.Empty(result);
    }

    // GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_Should_Return_Projet_When_Exists()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Projets.Add(CreateProjet(1, 1));
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdProjet);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        var result = await repo.GetByIdAsync(999);

        Assert.Null(result);
    }

    // GetByEnseignantIdAsync

    [Fact]
    public async Task GetByEnseignantIdAsync_Should_Return_Projets_Of_Enseignant()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.AddRange(CreateUtilisateur(1), CreateUtilisateur(2));
        context.Enseignants.AddRange(CreateEnseignant(1, 1), CreateEnseignant(2, 2));
        context.Projets.AddRange(CreateProjet(1, 1), CreateProjet(2, 1), CreateProjet(3, 2));
        await context.SaveChangesAsync();

        var result = await repo.GetByEnseignantIdAsync(1);

        Assert.Equal(2, result.Count());
        Assert.All(result, p => Assert.Equal(1, p.IdEnseignant));
    }

    [Fact]
    public async Task GetByEnseignantIdAsync_Should_Return_Empty_When_No_Match()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Projets.Add(CreateProjet(1, 1));
        await context.SaveChangesAsync();

        var result = await repo.GetByEnseignantIdAsync(999);

        Assert.Empty(result);
    }

    // AddAsync

    [Fact]
    public async Task AddAsync_Should_Persist_Projet()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        await context.SaveChangesAsync();

        await repo.AddAsync(CreateProjet(1, 1));

        Assert.Equal(1, await context.Projets.CountAsync());
    }

    // UpdateAsync

    [Fact]
    public async Task UpdateAsync_Should_Modify_Projet()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        var projet = CreateProjet(1, 1);
        context.Projets.Add(projet);
        await context.SaveChangesAsync();

        projet.Titre = "Titre Modifié";
        await repo.UpdateAsync(projet);

        var updated = await context.Projets.FindAsync(1);
        Assert.Equal("Titre Modifié", updated!.Titre);
    }

    // DeleteAsync

    [Fact]
    public async Task DeleteAsync_Should_Remove_Projet()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Projets.Add(CreateProjet(1, 1));
        await context.SaveChangesAsync();

        await repo.DeleteAsync(1);

        Assert.Equal(0, await context.Projets.CountAsync());
    }

    [Fact]
    public async Task DeleteAsync_Should_Do_Nothing_When_Not_Found()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Projets.Add(CreateProjet(1, 1));
        await context.SaveChangesAsync();

        await repo.DeleteAsync(999); // n'existe pas

        Assert.Equal(1, await context.Projets.CountAsync());
    }

    // CountAsync

    [Fact]
    public async Task CountAsync_Should_Return_Correct_Count()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Projets.AddRange(CreateProjet(1, 1), CreateProjet(2, 1), CreateProjet(3, 1));
        await context.SaveChangesAsync();

        var count = await repo.CountAsync();

        Assert.Equal(3, count);
    }

    // GetContributionsByProjectIdAsync

    [Fact]
    public async Task GetContributionsByProjectIdAsync_Should_Return_Contributions()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Etudiants.Add(CreateEtudiant(1, 1));
        context.Projets.Add(CreateProjet(1, 1));
        context.Contributions.AddRange(
            CreateContribution(1, 1, 1),
            CreateContribution(2, 1, 1)
        );
        await context.SaveChangesAsync();

        var result = await repo.GetContributionsByProjectIdAsync(1);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetContributionsByProjectIdAsync_Should_Return_Empty_When_No_Match()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        var result = await repo.GetContributionsByProjectIdAsync(999);

        Assert.Empty(result);
    }

    // AddContributionAsync
    
    [Fact]
    public async Task AddContributionAsync_Should_Persist_Contribution()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Etudiants.Add(CreateEtudiant(1, 1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Projets.Add(CreateProjet(1, 1));
        await context.SaveChangesAsync();

        await repo.AddContributionAsync(CreateContribution(1, 1, 1));

        Assert.Equal(1, await context.Contributions.CountAsync());
    }

    // AddPromptAsync

    [Fact]
    public async Task AddPromptAsync_Should_Persist_Prompt()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Etudiants.Add(CreateEtudiant(1, 1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Projets.Add(CreateProjet(1, 1));
        await context.SaveChangesAsync();

        await repo.AddPromptAsync(CreatePrompt(1, 1, 1));

        Assert.Equal(1, await context.Prompts.CountAsync());
    }

    // AddAffectationAsync

    [Fact]
    public async Task AddAffectationAsync_Should_Persist_Affectation()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.AddRange(CreateUtilisateur(1), CreateUtilisateur(2));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Etudiants.Add(CreateEtudiant(1, 2));
        context.Roles.Add(CreateRole(1));
        context.Projets.Add(CreateProjet(1, 1));
        context.Groupes.Add(CreateGroupe(1, 1));
        await context.SaveChangesAsync();

        await repo.AddAffectationAsync(CreateAffectation(1, 1, 1));

        Assert.Equal(1, await context.Affectations.CountAsync());
    }

    // AssignProjectToGroupAsync 

    [Fact]
    public async Task AssignProjectToGroupAsync_Should_Update_Groupe_IdProjet()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Projets.AddRange(CreateProjet(1, 1), CreateProjet(2, 1));
        var groupe = CreateGroupe(1, 1); // initialement lié au projet 1
        context.Groupes.Add(groupe);
        await context.SaveChangesAsync();

        await repo.AssignProjectToGroupAsync(2, 1); // on réassigne au projet 2

        var updated = await context.Groupes.FindAsync(1);
        Assert.Equal(2, updated!.IdProjet);
    }

    // GetByEtudiantIdAsync

    [Fact]
    public async Task GetByEtudiantIdAsync_Should_Return_Projets_Of_Etudiant()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.AddRange(CreateUtilisateur(1), CreateUtilisateur(2));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Etudiants.Add(CreateEtudiant(1, 2));
        context.Roles.Add(CreateRole(1));
        context.Projets.Add(CreateProjet(1, 1));
        context.Groupes.Add(CreateGroupe(1, 1));
        context.Affectations.Add(CreateAffectation(1, 1, 1));
        await context.SaveChangesAsync();

        var result = await repo.GetByEtudiantIdAsync(1);

        Assert.Single(result);
        Assert.Equal(1, result.First().IdProjet);
    }

    [Fact]
    public async Task GetByEtudiantIdAsync_Should_Return_Empty_When_No_Affectation()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        var result = await repo.GetByEtudiantIdAsync(999);

        Assert.Empty(result);
    }

    // GetPromptsByProjectAndEtudiantAsync

    [Fact]
    public async Task GetPromptsByProjectAndEtudiantAsync_Should_Return_Prompts()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Etudiants.Add(CreateEtudiant(1, 1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Projets.Add(CreateProjet(1, 1));
        context.Prompts.AddRange(CreatePrompt(1, 1, 1), CreatePrompt(2, 1, 1));
        await context.SaveChangesAsync();

        var result = await repo.GetPromptsByProjectAndEtudiantAsync(1, 1);

        Assert.Equal(2, result.Count());
    }

    // GetContributionsByProjectAndEtudiantAsync

    [Fact]
    public async Task GetContributionsByProjectAndEtudiantAsync_Should_Return_Contributions()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        context.Utilisateurs.Add(CreateUtilisateur(1));
        context.Etudiants.Add(CreateEtudiant(1, 1));
        context.Enseignants.Add(CreateEnseignant(1, 1));
        context.Projets.Add(CreateProjet(1, 1));
        context.Contributions.AddRange(
            CreateContribution(1, 1, 1),
            CreateContribution(2, 1, 1)
        );
        await context.SaveChangesAsync();

        var result = await repo.GetContributionsByProjectAndEtudiantAsync(1, 1);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetContributionsByProjectAndEtudiantAsync_Should_Return_Empty_When_No_Match()
    {
        var context = GetDbContext();
        var repo = new ProjetRepository(context);

        var result = await repo.GetContributionsByProjectAndEtudiantAsync(999, 999);

        Assert.Empty(result);
    }
}