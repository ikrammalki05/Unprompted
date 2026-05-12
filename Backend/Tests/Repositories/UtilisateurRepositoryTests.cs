using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class UtilisateurRepositoryTests
{
  private AppDbContext GetDbContext()
  {
      var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // DB unique à chaque test
          .Options;

      return new AppDbContext(options);
  }

  [Fact]
  public async Task AddAsync_Should_Add_Utilisateur()
  {
      // Arrange
      var context = GetDbContext();
      var repo = new UtilisateurRepository(context);

      var utilisateur = new Utilisateur
      {
          IdUtilisateur = 1,
          Nom = "Test",
          Prenom = "User",
          Statut = "Actif",
          Email = "test@test.com"
      };

      // Act
      await repo.AddAsync(utilisateur);

      // Assert
      var result = await context.Utilisateurs.FirstOrDefaultAsync();

      Assert.NotNull(result);
      Assert.Equal("test@test.com", result.Email);
      Assert.Equal("Test", result.Nom);
      Assert.Equal("User", result.Prenom);
      Assert.Equal("Actif", result.Statut);
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Utilisateur_When_Exists()
  {
      var context = GetDbContext();

      var utilisateur = new Utilisateur
      {
          IdUtilisateur = 1,
          Nom = "Test",
          Prenom = "User",
          Statut = "Actif",
          Email = "test@test.com"
      };

      context.Utilisateurs.Add(utilisateur);
      await context.SaveChangesAsync();

      var repo = new UtilisateurRepository(context);

      var result = await repo.GetByIdAsync(1);

      Assert.NotNull(result);
      Assert.Equal(1, result.IdUtilisateur);
  }

  [Fact]
  public async Task UpdateAsync_Should_Update_Utilisateur_When_Exists()
  {
      var context = GetDbContext();

      var utilisateur = new Utilisateur
      {
          IdUtilisateur = 1,
          Nom = "Test",
          Prenom = "User",
          Statut = "Actif",
          Email = "test@test.com"
      };

      context.Utilisateurs.Add(utilisateur);
      await context.SaveChangesAsync();

      // Détacher pour simuler un vrai scénario (entité venant d'une requête HTTP par exemple)
      context.Entry(utilisateur).State = EntityState.Detached;

      // Modifier les propriétés
      utilisateur.Nom = "Test1";
      utilisateur.Prenom = "User1";

      var repo = new UtilisateurRepository(context);
      await repo.UpdateAsync(utilisateur);

      // Relire depuis la base pour vérifier
      var result = await context.Utilisateurs.FindAsync(1);

      Assert.NotNull(result);
      Assert.Equal("Test1", result.Nom);
      Assert.Equal("User1", result.Prenom);
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Null_When_NotFound()
  {
      var context = GetDbContext();
      var repo = new UtilisateurRepository(context);

      var result = await repo.GetByIdAsync(999);

      Assert.Null(result);
  }

  [Fact]
  public async Task DeleteAsync_Should_Remove_Utilisateur()
  {
      var context = GetDbContext();

      var utilisateur = new Utilisateur
      {
          IdUtilisateur = 1,
          Nom = "Test",
          Prenom = "User",
          Statut = "Actif",
          Email = "test@test.com"
      };

      context.Utilisateurs.Add(utilisateur);
      await context.SaveChangesAsync();

      var repo = new UtilisateurRepository(context);

      await repo.DeleteAsync(1);

      var result = await context.Utilisateurs.FindAsync(1);
      Assert.Null(result);
  }

  [Fact]
  public async Task GetByEmailAsync_Should_Return_Utilisateur_When_Exists()
  {
      var context = GetDbContext();

      var utilisateur = new Utilisateur
      {
          IdUtilisateur = 1,
          Nom = "Test",
          Prenom = "User",
          Statut = "Actif",
          Email = "test@test.com"
      };

      context.Utilisateurs.Add(utilisateur);
      await context.SaveChangesAsync();

      var repo = new UtilisateurRepository(context);

      var result = await repo.GetByEmailAsync("test@test.com");

      Assert.NotNull(result);
      Assert.Equal(1, result.IdUtilisateur);
  }
}