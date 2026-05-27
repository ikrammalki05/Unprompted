using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Reflection;

namespace Tests.Repositories;
public class GroupeRepositoryTests
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
    private Groupe CreateGroupe()
    {
        return new Groupe
        {
            IdGroupe = 1,
            NomGroupe = "Groupe Test",
            IdProjet = 1
        };
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
            IdUtilisateur = 1
        };
    }
    private Role CreateRole()
    {
        return new Role
        {
            IdRole = 1,
            NomRole = "Membre"
        };
    }
    private Affectation CreateAffectation()
    {
        return new Affectation
        {
            IdEtudiant = 1,
            IdGroupe = 1,
            IdRole = 1
        };
    }

    [Fact]
    public async Task AddAsync_Should_Add_Groupe()
  {
    var context = GetDbContext();
    var repo = new GroupeRepository(context);

    var projet = CreateProjet();
    var groupe = CreateGroupe();

    context.Projets.Add(projet);
    await context.SaveChangesAsync();

    await repo.AddAsync(groupe);

    var result = await context.Groupes.FirstOrDefaultAsync();
    Assert.NotNull(result);
    Assert.Equal(1, result.IdGroupe);
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Groupe_with_Projet()
  {
    var context = GetDbContext();
    var repo = new GroupeRepository(context);

    var projet = CreateProjet();
    var groupe = CreateGroupe();

    context.Projets.Add(projet);
    context.Groupes.Add(groupe);
    await context.SaveChangesAsync();

    var result = await repo.GetByIdAsync(1);

    Assert.NotNull(result);
    Assert.Equal(1, result.IdGroupe);
    Assert.Equal(1, result.IdProjetNavigation.IdProjet);
  }

  [Fact]
  public async Task GetAllAsync_Should_Return_All_Groupes_with_Projet()
  {
    var context = GetDbContext();
    var repo = new GroupeRepository(context);

    var projet = CreateProjet();
    var groupe1 = CreateGroupe();
    var groupe2 = CreateGroupe();
    groupe2.IdGroupe = 2;

    context.Projets.Add(projet);
    context.Groupes.AddRange(groupe1, groupe2);
    await context.SaveChangesAsync();

    var result = await repo.GetAllAsync();

    Assert.NotNull(result);
    Assert.Equal(2, result.Count());
    Assert.All(result, g => Assert.Equal(1, g.IdProjetNavigation.IdProjet));
  }

    [Fact]
    public async Task UpdateAsync_Should_Update_Groupe()
    {
        var context = GetDbContext();
        var repo = new GroupeRepository(context);

        var projet = CreateProjet();
        var groupe = CreateGroupe();

        context.Projets.Add(projet);
        context.Groupes.Add(groupe);
        await context.SaveChangesAsync();

        groupe.NomGroupe = "Groupe Modifié";
        await repo.UpdateAsync(groupe);

        var result = await context.Groupes.FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.Equal("Groupe Modifié", result.NomGroupe);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Groupe()
    {
        var context = GetDbContext();
        var repo = new GroupeRepository(context);

        var projet = CreateProjet();
        var groupe = CreateGroupe();

        context.Projets.Add(projet);
        context.Groupes.Add(groupe);
        await context.SaveChangesAsync();

        await repo.DeleteAsync(1);

        var result = await context.Groupes.FirstOrDefaultAsync();

        Assert.Null(result);
    }

    [Fact]    
    public async Task AddAffectationAsync_Should_Add_Affectation_To_Groupe()
  {
    var context = GetDbContext();
    var repo = new GroupeRepository(context);

    var projet = CreateProjet();
    var groupe = CreateGroupe();
    var utilisateur = CreateUtilisateur();
    var etudiant = CreateEtudiant();
    var role = CreateRole();
    var affectation = CreateAffectation();

    context.Projets.Add(projet);
    context.Groupes.Add(groupe);
    context.Utilisateurs.Add(utilisateur);
    context.Etudiants.Add(etudiant);
    context.Roles.Add(role);
    await context.SaveChangesAsync();

    await repo.AddAffectationAsync(affectation);

    var result = await context.Affectations.FirstOrDefaultAsync();
    Assert.NotNull(result);
    Assert.Equal(1, result.IdEtudiant);
    Assert.Equal(1, result.IdGroupe);
    Assert.Equal(1, result.IdRole);
  }
}