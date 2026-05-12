using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class AdminRepositoryTests
{
  private AppDbContext GetDbContext()
  {
      var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // DB unique à chaque test
          .Options;

      return new AppDbContext(options);
  }

  
  private Utilisateur CreateUtilisateur()
  {
      return new Utilisateur
      {
          IdUtilisateur = 1,
          Email = "admin@test.com",
          Nom = "Admin",
          Prenom = "System",
          Statut = "Actif"
      };
  }

  private Admin CreateAdmin(Utilisateur user)
  {
      return new Admin
      {
          IdAdmin = 1,
          IdUtilisateur = user.IdUtilisateur,
          IdUtilisateurNavigation = user
      };
  }

  [Fact]
  public async Task AddAsync_Should_Add_Admin()
  {
      var context = GetDbContext();
      var repo = new AdminRepository(context);

      var user = CreateUtilisateur();
      var admin = CreateAdmin(user);

      context.Utilisateurs.Add(user);
      await context.SaveChangesAsync();

      await repo.AddAsync(admin);

      var result = await context.Admins.FirstOrDefaultAsync();

      Assert.NotNull(result);
      Assert.Equal(1, result.IdAdmin);
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Admin_With_User()
  {
      var context = GetDbContext();
      var repo = new AdminRepository(context);

      var user = CreateUtilisateur();
      var admin = CreateAdmin(user);

      context.Utilisateurs.Add(user);
      context.Admins.Add(admin);
      await context.SaveChangesAsync();

      var result = await repo.GetByIdAsync(1);

      Assert.NotNull(result);
      Assert.NotNull(result.IdUtilisateurNavigation);
      Assert.Equal("admin@test.com", result.IdUtilisateurNavigation.Email);
  }

  [Fact]
  public async Task UpdateAsync_Should_Update_Admin()
  {
      var context = GetDbContext();
      var repo = new AdminRepository(context);

      var user = CreateUtilisateur();
      var admin = CreateAdmin(user);

      context.Utilisateurs.Add(user);
      context.Admins.Add(admin);
      await context.SaveChangesAsync();

      admin.IdUtilisateurNavigation.Nom = "Updated Admin"; // simulation update
      await repo.UpdateAsync(admin);

      var result = await context.Admins.FirstOrDefaultAsync();
      Assert.NotNull(result);
      Assert.NotNull(result.IdUtilisateurNavigation);
      Assert.Equal("Updated Admin", result.IdUtilisateurNavigation.Nom);
  }
}