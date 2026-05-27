using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Services;

public class ClasseServiceTests
{
    private readonly Mock<IClasseRepository> _classeRepoMock;
    private readonly ClasseService _service;

    public ClasseServiceTests()
    {
        _classeRepoMock = new Mock<IClasseRepository>();
        _service = new ClasseService(_classeRepoMock.Object);
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private Classe CreateClasse(int id = 1) => new Classe
    {
        IdClasse = id,
        NomClasse = "L3 Informatique",
        AnneeAcademique = "2024-2025",
        EffectifMax = 30,
        DateCreation = DateTime.Now
    };

    private ClasseCreateDto CreateClasseDto() => new ClasseCreateDto
    {
        NomClasse = "L3 Informatique",
        AnneeAcademique = "2024-2025",
        EffectifMax = 30
    };

    // ─── GetAllClassesAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllClassesAsync_Should_Return_All_Classes()
    {
        _classeRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Classe> { CreateClasse(1), CreateClasse(2) });

        var result = await _service.GetAllClassesAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllClassesAsync_Should_Map_Fields_Correctly()
    {
        _classeRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Classe> { CreateClasse(1) });

        var result = await _service.GetAllClassesAsync();
        var dto = result.First();

        Assert.Equal(1, dto.Id);
        Assert.Equal("L3 Informatique", dto.NomClasse);
        Assert.Equal("2024-2025", dto.AnneeAcademique);
        Assert.Equal(30, dto.EffectifMax);
        Assert.Equal(0, dto.EffectifActuel);
        Assert.Equal("Aucun", dto.EnseignantReferent);
    }

    [Fact]
    public async Task GetAllClassesAsync_Should_Return_Empty_When_None()
    {
        _classeRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Classe>());

        var result = await _service.GetAllClassesAsync();

        Assert.Empty(result);
    }

    // ─── GetClasseByIdAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task GetClasseByIdAsync_Should_Return_Dto_When_Found()
    {
        _classeRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(CreateClasse(1));

        var result = await _service.GetClasseByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("L3 Informatique", result.NomClasse);
    }

    [Fact]
    public async Task GetClasseByIdAsync_Should_Return_Null_When_Not_Found()
    {
        _classeRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Classe?)null);

        var result = await _service.GetClasseByIdAsync(99);

        Assert.Null(result);
    }

    // ─── CreateClasseAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CreateClasseAsync_Should_Return_Dto_With_Correct_Fields()
    {
        var dto = CreateClasseDto();

        _classeRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Classe>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateClasseAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("L3 Informatique", result.NomClasse);
        Assert.Equal("2024-2025", result.AnneeAcademique);
        Assert.Equal(30, result.EffectifMax);
        Assert.Equal(0, result.EffectifActuel);
        Assert.Equal("Aucun", result.EnseignantReferent);
    }

    [Fact]
    public async Task CreateClasseAsync_Should_Call_AddAsync_Once()
    {
        var dto = CreateClasseDto();

        _classeRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Classe>()))
            .Returns(Task.CompletedTask);

        await _service.CreateClasseAsync(dto);

        _classeRepoMock.Verify(
            r => r.AddAsync(It.Is<Classe>(c =>
                c.NomClasse == dto.NomClasse &&
                c.AnneeAcademique == dto.AnneeAcademique &&
                c.EffectifMax == dto.EffectifMax
            )),
            Times.Once
        );
    }

    // ─── UpdateClasseAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateClasseAsync_Should_Update_Fields()
    {
        var classe = CreateClasse(1);
        var dto = new ClasseCreateDto
        {
            NomClasse = "M1 Réseaux",
            AnneeAcademique = "2025-2026",
            EffectifMax = 25
        };

        _classeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(classe);
        _classeRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Classe>())).Returns(Task.CompletedTask);

        await _service.UpdateClasseAsync(1, dto);

        _classeRepoMock.Verify(
            r => r.UpdateAsync(It.Is<Classe>(c =>
                c.NomClasse == "M1 Réseaux" &&
                c.AnneeAcademique == "2025-2026" &&
                c.EffectifMax == 25
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateClasseAsync_Should_Throw_When_Not_Found()
    {
        _classeRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Classe?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateClasseAsync(99, CreateClasseDto())
        );
    }

    [Fact]
    public async Task UpdateClasseAsync_Should_Not_Call_UpdateAsync_When_Not_Found()
    {
        _classeRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Classe?)null);

        try { await _service.UpdateClasseAsync(99, CreateClasseDto()); } catch { }

        _classeRepoMock.Verify(
            r => r.UpdateAsync(It.IsAny<Classe>()),
            Times.Never
        );
    }

    // ─── DeleteClasseAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteClasseAsync_Should_Call_DeleteAsync()
    {
        _classeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateClasse(1));
        _classeRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _service.DeleteClasseAsync(1);

        _classeRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteClasseAsync_Should_Throw_When_Not_Found()
    {
        _classeRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Classe?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.DeleteClasseAsync(99)
        );
    }

    [Fact]
    public async Task DeleteClasseAsync_Should_Not_Call_DeleteAsync_When_Not_Found()
    {
        _classeRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Classe?)null);

        try { await _service.DeleteClasseAsync(99); } catch { }

        _classeRepoMock.Verify(
            r => r.DeleteAsync(It.IsAny<int>()),
            Times.Never
        );
    }
}