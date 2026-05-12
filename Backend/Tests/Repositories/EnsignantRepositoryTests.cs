using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Repositories;
public class EnseignantRepositoryTests
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
          Email = "ens@test.com",
          Nom = "Prof",
          Prenom = "Test",
          Statut = "Actif"
      };
  }

  private Enseignant CreateEnseignant(Utilisateur user)
  {
      return new Enseignant
      {
          IdEnseignant = 1,
          IdUtilisateur = user.IdUtilisateur,
          Specialite = "Math",
          IdUtilisateurNavigation = user
      };
  }

  [Fact]
  public async Task AddAsync_Should_Add_Enseignant()
  {
      var context = GetDbContext();
      var repo = new EnseignantRepository(context);

      var user = CreateUtilisateur();
      var enseignant = CreateEnseignant(user);

      context.Utilisateurs.Add(user);
      await context.SaveChangesAsync();

      await repo.AddAsync(enseignant);

      var result = await context.Enseignants.FirstOrDefaultAsync();

      Assert.NotNull(result);
      Assert.Equal("Math", result.Specialite);
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Enseignant_With_User()
  {
      var context = GetDbContext();
      var repo = new EnseignantRepository(context);

      var user = CreateUtilisateur();
      var enseignant = CreateEnseignant(user);

      context.Utilisateurs.Add(user);
      context.Enseignants.Add(enseignant);
      await context.SaveChangesAsync();

      var result = await repo.GetByIdAsync(1);

      Assert.NotNull(result);
      Assert.NotNull(result.IdUtilisateurNavigation);
      Assert.Equal("ens@test.com", result.IdUtilisateurNavigation.Email);
  }

  [Fact]
  public async Task GetBySpecialiteAsync_Should_Return_Filtered_Results()
  {
      var context = GetDbContext();
      var repo = new EnseignantRepository(context);

      var user1 = CreateUtilisateur();
      var enseignant = CreateEnseignant(user1);

      context.Utilisateurs.Add(user1);
      context.Enseignants.Add(enseignant);
      await context.SaveChangesAsync();

      var result = await repo.GetBySpecialiteAsync("Math");

      Assert.Single(result);
      Assert.Equal("Math", result.First().Specialite);
  }

  [Fact]
  public async Task CountAsync_Should_Return_Number_Of_Enseignants()
  {
      var context = GetDbContext();
      var repo = new EnseignantRepository(context);

      var user = CreateUtilisateur();
      var enseignant = CreateEnseignant(user);

      context.Utilisateurs.Add(user);
      context.Enseignants.Add(enseignant);
      await context.SaveChangesAsync();

      var count = await repo.CountAsync();

      Assert.Equal(1, count);
  }

  [Fact]
  public async Task DeleteAsync_Should_Remove_Enseignant()
  {
      var context = GetDbContext();
      var repo = new EnseignantRepository(context);

      var user = CreateUtilisateur();
      var enseignant = CreateEnseignant(user);

      context.Utilisateurs.Add(user);
      context.Enseignants.Add(enseignant);
      await context.SaveChangesAsync();

      await repo.DeleteAsync(1);

      var result = await context.Enseignants.FindAsync(1);

      Assert.Null(result);
  }
}
