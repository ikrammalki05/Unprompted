using Infrastructure.Data;
using Infrastructure.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Reflection;

namespace Tests.Repositories;
public class AffectationRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private Affectation CreateAffectation()
    {
        return new Affectation
        {
            IdAffectation = 1,
            IdEtudiant = 1,
            IdGroupe = 1,
            IdRole = 1
        };
    }

    [Fact]
    public async Task AddAsync_Should_Add_Affectation()
    {
        var context = GetDbContext();
        var repo = new AffectationRepository(context);

        var affectation = CreateAffectation();
        await repo.AddAsync(affectation);

        var result = await context.Affectations.FirstOrDefaultAsync();
        Assert.NotNull(result);
    }
}