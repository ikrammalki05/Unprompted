using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Repositories;
public class ClasseRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private Classe CreateClasse()
    {
        return new Classe
        {
            IdClasse = 1,
            NomClasse = "Classe Test",
            AnneeAcademique = "2023-2024",
            EffectifMax = 30
        };
    }

    [Fact]
    public async Task AddAsync_Should_Add_Classe()
    {
        var context = GetDbContext();
        var repo = new ClasseRepository(context);

        var classe = CreateClasse();

        await repo.AddAsync(classe);

        var result = await context.Classes.FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.Equal(1, result.IdClasse);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Classe()
    {
        var context = GetDbContext();
        var repo = new ClasseRepository(context);

        var classe = CreateClasse();

        context.Classes.Add(classe);
        await context.SaveChangesAsync();

        var result = await repo.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdClasse);
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Classes()
    {
        var context = GetDbContext();
        var repo = new ClasseRepository(context);

        var classe1 = CreateClasse();
        var classe2 = CreateClasse();
        classe2.IdClasse = 2;

        context.Classes.AddRange(classe1, classe2);
        await context.SaveChangesAsync();

        var result = await repo.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CountAsync_Should_Return_Number_Of_Classes()
    {
        var context = GetDbContext();
        var repo = new ClasseRepository(context);

        var classe = CreateClasse();

        context.Classes.Add(classe);
        await context.SaveChangesAsync();

        var count = await repo.CountAsync();

        Assert.Equal(1, count);
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Classe()
    {
        var context = GetDbContext();
        var repo = new ClasseRepository(context);

        var classe = CreateClasse();

        context.Classes.Add(classe);
        await context.SaveChangesAsync();

        await repo.DeleteAsync(1);

        var result = await context.Classes.FindAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Classe()
    {
        var context = GetDbContext();
        var repo = new ClasseRepository(context);

        var classe = CreateClasse();

        context.Classes.Add(classe);
        await context.SaveChangesAsync();

        classe.NomClasse = "Updated Classe";
        await repo.UpdateAsync(classe);

        var result = await context.Classes.FindAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Updated Classe", result.NomClasse);
    }
}