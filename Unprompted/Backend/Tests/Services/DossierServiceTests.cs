using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Services;

public class DossierServiceTests
{
    private readonly Mock<IDossierRepository> _dossierRepoMock;
    private readonly Mock<ILogger<DossierService>> _loggerMock;
    private readonly DossierService _service;

    public DossierServiceTests()
    {
        _dossierRepoMock = new Mock<IDossierRepository>();
        _loggerMock      = new Mock<ILogger<DossierService>>();

        _service = new DossierService(
            _dossierRepoMock.Object,
            _loggerMock.Object
        );
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private Dossier CreateDossier(int id = 1, int idProjet = 1, int? parentId = null) => new Dossier
    {
        IdDossier      = id,
        Nom            = "Dossier " + id,
        IdProjet       = idProjet,
        DossierParentId = parentId,
        CreatedAt      = DateTime.UtcNow,
        Dossiersfils   = new List<Dossier>(),
        Fichiers       = new List<Fichier>()
    };

    private Fichier CreateFichier(int id, int idDossier) => new Fichier
    {
        IdFichier           = id,
        Nom                 = "fichier" + id + ".cs",
        Extension           = ".cs",
        Language            = "csharp",
        Size                = 512,
        IdDossier           = idDossier,
        IdProjet            = 1,
        CreatedBy           = "user-test",
        DerniereModification = DateTime.UtcNow
    };

    private DossierCreateDto CreateDossierDto(int idProjet = 1, int? parentId = null) => new DossierCreateDto
    {
        Nom             = "Nouveau Dossier",
        IdProjet        = idProjet,
        DossierParentId = parentId
    };

    // ─── CreateAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_Should_Return_Dto_When_Valid()
    {
        var dto = CreateDossierDto();

        _dossierRepoMock
            .Setup(r => r.ExistsAsync(dto.Nom, dto.DossierParentId, dto.IdProjet))
            .ReturnsAsync(false);

        _dossierRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Dossier>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Nouveau Dossier", result.Nom);
        Assert.Null(result.DossierParentId);
    }

    [Fact]
    public async Task CreateAsync_Should_Call_AddAsync_Once()
    {
        var dto = CreateDossierDto();

        _dossierRepoMock
            .Setup(r => r.ExistsAsync(dto.Nom, dto.DossierParentId, dto.IdProjet))
            .ReturnsAsync(false);

        _dossierRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Dossier>()))
            .Returns(Task.CompletedTask);

        await _service.CreateAsync(dto);

        _dossierRepoMock.Verify(
            r => r.AddAsync(It.Is<Dossier>(d =>
                d.Nom == "Nouveau Dossier" &&
                d.IdProjet == 1
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Dossier_Already_Exists()
    {
        var dto = CreateDossierDto();

        _dossierRepoMock
            .Setup(r => r.ExistsAsync(dto.Nom, dto.DossierParentId, dto.IdProjet))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<Exception>(
            () => _service.CreateAsync(dto)
        );
    }

    [Fact]
    public async Task CreateAsync_Should_Not_Call_AddAsync_When_Already_Exists()
    {
        var dto = CreateDossierDto();

        _dossierRepoMock
            .Setup(r => r.ExistsAsync(dto.Nom, dto.DossierParentId, dto.IdProjet))
            .ReturnsAsync(true);

        try { await _service.CreateAsync(dto); } catch { }

        _dossierRepoMock.Verify(r => r.AddAsync(It.IsAny<Dossier>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_Should_Preserve_ParentId()
    {
        var dto = CreateDossierDto(parentId: 5);

        _dossierRepoMock
            .Setup(r => r.ExistsAsync(dto.Nom, 5, dto.IdProjet))
            .ReturnsAsync(false);

        _dossierRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Dossier>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto);

        Assert.Equal(5, result.DossierParentId);
    }

    // ─── DeleteAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_Should_Call_DeleteAsync_Once()
    {
        _dossierRepoMock
            .Setup(r => r.DeleteAsync(1))
            .Returns(Task.CompletedTask);

        await _service.DeleteAsync(1);

        _dossierRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    // ─── GetByIdAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        _dossierRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Dossier?)null);

        var result = await _service.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Dto_When_Found()
    {
        var dossier = CreateDossier(1, 1);

        _dossierRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(dossier);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Dossier 1", result.Nom);
        Assert.Equal(1, result.IdProjet);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Map_Fichiers()
    {
        var dossier = CreateDossier(1, 1);
        dossier.Fichiers = new List<Fichier>
        {
            CreateFichier(1, 1),
            CreateFichier(2, 1)
        };

        _dossierRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(dossier);

        var result = await _service.GetByIdAsync(1);

        Assert.Equal(2, result!.Fichiers!.Count);
        Assert.Equal("fichier1.cs", result.Fichiers[0].Nom);
        Assert.Equal(".cs", result.Fichiers[0].Extension);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Map_SousDossiers_With_Fichiers()
    {
        var fils = CreateDossier(2, 1, parentId: 1);
        fils.Fichiers = new List<Fichier> { CreateFichier(1, 2) };

        var parent = CreateDossier(1, 1);
        parent.Dossiersfils = new List<Dossier> { fils };

        _dossierRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(parent);

        var result = await _service.GetByIdAsync(1);

        Assert.Single(result!.SousDossiers!);
        Assert.Equal("Dossier 2", result.SousDossiers![0].Nom);
        Assert.Single(result.SousDossiers[0].Fichiers!);
    }

    // ─── GetByProjetIdAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetByProjetIdAsync_Should_Return_All_Dossiers()
    {
        _dossierRepoMock
            .Setup(r => r.GetByProjectIdAsync(1))
            .ReturnsAsync(new List<Dossier>
            {
                CreateDossier(1, 1),
                CreateDossier(2, 1)
            });

        var result = await _service.GetByProjetIdAsync(1);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByProjetIdAsync_Should_Return_Empty_When_None()
    {
        _dossierRepoMock
            .Setup(r => r.GetByProjectIdAsync(99))
            .ReturnsAsync(new List<Dossier>());

        var result = await _service.GetByProjetIdAsync(99);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByProjetIdAsync_Should_Map_SousDossiers_And_Fichiers()
    {
        var fils = CreateDossier(2, 1, parentId: 1);
        fils.Fichiers = new List<Fichier> { CreateFichier(1, 2) };

        var parent = CreateDossier(1, 1);
        parent.Fichiers    = new List<Fichier> { CreateFichier(2, 1) };
        parent.Dossiersfils = new List<Dossier> { fils };

        _dossierRepoMock
            .Setup(r => r.GetByProjectIdAsync(1))
            .ReturnsAsync(new List<Dossier> { parent });

        var result = (await _service.GetByProjetIdAsync(1)).ToList();

        Assert.Single(result);
        Assert.Single(result[0].Fichiers!);
        Assert.Single(result[0].SousDossiers!);
        Assert.Single(result[0].SousDossiers![0].Fichiers!);
    }

    // ─── RenameAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task RenameAsync_Should_Return_Updated_Dto()
    {
        var dossier = CreateDossier(1, 1);
        var renameDto = new DossierRenameDto { Nom = "Nouveau Nom" };

        _dossierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(dossier);
        _dossierRepoMock.Setup(r => r.ExistsAsync("Nouveau Nom", null, 1)).ReturnsAsync(false);
        _dossierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Dossier>())).Returns(Task.CompletedTask);

        var result = await _service.RenameAsync(1, renameDto);

        Assert.NotNull(result);
        Assert.Equal("Nouveau Nom", result.Nom);
        Assert.Equal(1, result.IdProjet);
    }

    [Fact]
    public async Task RenameAsync_Should_Call_UpdateAsync_With_New_Nom()
    {
        var dossier = CreateDossier(1, 1);
        var renameDto = new DossierRenameDto { Nom = "Nouveau Nom" };

        _dossierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(dossier);
        _dossierRepoMock.Setup(r => r.ExistsAsync("Nouveau Nom", null, 1)).ReturnsAsync(false);
        _dossierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Dossier>())).Returns(Task.CompletedTask);

        await _service.RenameAsync(1, renameDto);

        _dossierRepoMock.Verify(
            r => r.UpdateAsync(It.Is<Dossier>(d => d.Nom == "Nouveau Nom")),
            Times.Once
        );
    }

    [Fact]
    public async Task RenameAsync_Should_Throw_When_Dossier_Not_Found()
    {
        _dossierRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Dossier?)null);

        await Assert.ThrowsAsync<Exception>(
            () => _service.RenameAsync(99, new DossierRenameDto { Nom = "X" })
        );
    }

    [Fact]
    public async Task RenameAsync_Should_Throw_When_Name_Already_Exists()
    {
        var dossier = CreateDossier(1, 1);
        var renameDto = new DossierRenameDto { Nom = "Nom Existant" };

        _dossierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(dossier);
        _dossierRepoMock
            .Setup(r => r.ExistsAsync("Nom Existant", null, 1))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<Exception>(
            () => _service.RenameAsync(1, renameDto)
        );
    }

    [Fact]
    public async Task RenameAsync_Should_Not_Call_UpdateAsync_When_Name_Already_Exists()
    {
        var dossier = CreateDossier(1, 1);
        var renameDto = new DossierRenameDto { Nom = "Nom Existant" };

        _dossierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(dossier);
        _dossierRepoMock
            .Setup(r => r.ExistsAsync("Nom Existant", null, 1))
            .ReturnsAsync(true);

        try { await _service.RenameAsync(1, renameDto); } catch { }

        _dossierRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Dossier>()), Times.Never);
    }
}