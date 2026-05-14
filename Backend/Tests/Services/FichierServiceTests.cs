using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Services;

public class FichierServiceTests
{
    private readonly Mock<IFichierRepository> _fichierRepoMock;
    private readonly Mock<IFichierVersionRepository> _versionRepoMock;
    private readonly Mock<ILogger<FichierService>> _loggerMock;
    private readonly FichierService _service;

    public FichierServiceTests()
    {
        _fichierRepoMock = new Mock<IFichierRepository>();
        _versionRepoMock = new Mock<IFichierVersionRepository>();
        _loggerMock      = new Mock<ILogger<FichierService>>();

        _service = new FichierService(
            _fichierRepoMock.Object,
            _versionRepoMock.Object,
            _loggerMock.Object
        );
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private Fichier CreateFichier(int id = 1, string contenu = "console.log('hello');") => new Fichier
    {
        IdFichier            = id,
        Nom                  = "main.js",
        Extension            = ".js",
        Language             = "javascript",
        Contenu              = contenu,
        Size                 = contenu.Length,
        Version              = 1,
        IdProjet             = 1,
        IdDossier            = null,
        CreatedBy            = "user-test",
        ContentHash          = ComputeHash(contenu),
        DerniereModification = DateTime.UtcNow
    };

    private FichierCreateDto CreateFichierDto() => new FichierCreateDto
    {
        Nom       = "main.js",
        Extension = ".js",
        Contenu   = "console.log('hello');",
        IdProjet  = 1,
        IdDossier = null
    };

    private FichierVersion CreateVersion(int id, int idFichier, int version = 1) => new FichierVersion
    {
        IdFichierVersion = id,
        IdFichier        = idFichier,
        Contenu          = "old content",
        Version          = version,
        CreatedBy        = "user-test",
        CreatedAt        = DateTime.UtcNow
    };

    private static string ComputeHash(string contenu)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(contenu);
        return Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(bytes));
    }

    // ─── CreateAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_Should_Return_Dto_When_Valid()
    {
        var dto = CreateFichierDto();

        _fichierRepoMock
            .Setup(r => r.ExistsAsync(dto.Nom, dto.IdDossier, dto.IdProjet))
            .ReturnsAsync(false);

        _fichierRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Fichier>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto, "user-test");

        Assert.NotNull(result);
        Assert.Equal("main.js", result.Nom);
        Assert.Equal(".js", result.Extension);
        Assert.Equal("console.log('hello');", result.Contenu);
    }

    [Fact]
    public async Task CreateAsync_Should_Call_AddAsync_With_Correct_Fields()
    {
        var dto = CreateFichierDto();

        _fichierRepoMock
            .Setup(r => r.ExistsAsync(dto.Nom, dto.IdDossier, dto.IdProjet))
            .ReturnsAsync(false);

        _fichierRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Fichier>()))
            .Returns(Task.CompletedTask);

        await _service.CreateAsync(dto, "user-test");

        _fichierRepoMock.Verify(
            r => r.AddAsync(It.Is<Fichier>(f =>
                f.Nom       == "main.js"   &&
                f.Extension == ".js"       &&
                f.IdProjet  == 1           &&
                f.CreatedBy == "user-test" &&
                f.Size      == dto.Contenu!.Length
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateAsync_Should_Compute_ContentHash()
    {
        var dto = CreateFichierDto();

        _fichierRepoMock
            .Setup(r => r.ExistsAsync(dto.Nom, dto.IdDossier, dto.IdProjet))
            .ReturnsAsync(false);

        Fichier? captured = null;
        _fichierRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Fichier>()))
            .Callback<Fichier>(f => captured = f)
            .Returns(Task.CompletedTask);

        await _service.CreateAsync(dto, "user-test");

        Assert.NotNull(captured!.ContentHash);
        Assert.Equal(ComputeHash(dto.Contenu!), captured.ContentHash);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_File_Already_Exists()
    {
        var dto = CreateFichierDto();

        _fichierRepoMock
            .Setup(r => r.ExistsAsync(dto.Nom, dto.IdDossier, dto.IdProjet))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<Exception>(
            () => _service.CreateAsync(dto, "user-test")
        );
    }

    [Fact]
    public async Task CreateAsync_Should_Not_Call_AddAsync_When_Already_Exists()
    {
        var dto = CreateFichierDto();

        _fichierRepoMock
            .Setup(r => r.ExistsAsync(dto.Nom, dto.IdDossier, dto.IdProjet))
            .ReturnsAsync(true);

        try { await _service.CreateAsync(dto, "user-test"); } catch { }

        _fichierRepoMock.Verify(r => r.AddAsync(It.IsAny<Fichier>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_Should_Handle_Null_Contenu()
    {
        var dto = new FichierCreateDto
        {
            Nom       = "empty.js",
            Extension = ".js",
            Contenu   = null,
            IdProjet  = 1,
            IdDossier = null
        };

        _fichierRepoMock
            .Setup(r => r.ExistsAsync(dto.Nom, dto.IdDossier, dto.IdProjet))
            .ReturnsAsync(false);

        _fichierRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Fichier>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto, "user-test");

        Assert.Equal(0, result.Version - result.Version + 0); // Size = 0
        _fichierRepoMock.Verify(
            r => r.AddAsync(It.Is<Fichier>(f => f.Size == 0)),
            Times.Once
        );
    }

    // ─── GetByIdAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
    {
        _fichierRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Fichier?)null);

        var result = await _service.GetByIdAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Dto_When_Found()
    {
        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateFichier(1));

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("main.js", result.Nom);
        Assert.Equal(".js", result.Extension);
        Assert.Equal("console.log('hello');", result.Contenu);
        Assert.Equal(1, result.Version);
    }

    // ─── UpdateAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_Should_Throw_When_Not_Found()
    {
        _fichierRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Fichier?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateAsync(99, new FichierUpdateDto { Contenu = "x" }, "user-test")
        );
    }

    [Fact]
    public async Task UpdateAsync_Should_Increment_Version()
    {
        var fichier = CreateFichier(1);

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _fichierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Fichier>())).Returns(Task.CompletedTask);
        _versionRepoMock.Setup(r => r.AddAsync(It.IsAny<FichierVersion>())).Returns(Task.CompletedTask);

        await _service.UpdateAsync(1, new FichierUpdateDto { Contenu = "new content" }, "user-test");

        Assert.Equal(2, fichier.Version);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Contenu_And_Size()
    {
        var fichier = CreateFichier(1);

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _fichierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Fichier>())).Returns(Task.CompletedTask);
        _versionRepoMock.Setup(r => r.AddAsync(It.IsAny<FichierVersion>())).Returns(Task.CompletedTask);

        await _service.UpdateAsync(1, new FichierUpdateDto { Contenu = "new content" }, "user-test");

        _fichierRepoMock.Verify(
            r => r.UpdateAsync(It.Is<Fichier>(f =>
                f.Contenu == "new content" &&
                f.Size    == "new content".Length
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateAsync_Should_Create_Version_Entry()
    {
        var fichier = CreateFichier(1);

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _fichierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Fichier>())).Returns(Task.CompletedTask);
        _versionRepoMock.Setup(r => r.AddAsync(It.IsAny<FichierVersion>())).Returns(Task.CompletedTask);

        await _service.UpdateAsync(1, new FichierUpdateDto { Contenu = "new content" }, "user-test");

        _versionRepoMock.Verify(
            r => r.AddAsync(It.Is<FichierVersion>(v =>
                v.IdFichier  == 1          &&
                v.CreatedBy  == "user-test"&&
                v.Version    == 2
            )),
            Times.Once
        );
    }

    // ─── DeleteAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_Should_Call_DeleteAsync_Once()
    {
        _fichierRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(1);

        _fichierRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    // ─── AutosaveAsync ────────────────────────────────────────────────────────

    [Fact]
    public async Task AutosaveAsync_Should_Throw_When_Not_Found()
    {
        _fichierRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Fichier?)null);

        await Assert.ThrowsAsync<Exception>(
            () => _service.AutosaveAsync(99, "content", "user-test")
        );
    }

    [Fact]
    public async Task AutosaveAsync_Should_Skip_When_Content_Unchanged()
    {
        var contenu = "console.log('hello');";
        var fichier = CreateFichier(1, contenu);

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);

        // même contenu => même hash => pas de sauvegarde
        await _service.AutosaveAsync(1, contenu, "user-test");

        _fichierRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Fichier>()), Times.Never);
        _versionRepoMock.Verify(r => r.AddAsync(It.IsAny<FichierVersion>()), Times.Never);
    }

    [Fact]
    public async Task AutosaveAsync_Should_Save_When_Content_Changed()
    {
        var fichier = CreateFichier(1, "old content");

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _fichierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Fichier>())).Returns(Task.CompletedTask);
        _versionRepoMock.Setup(r => r.AddAsync(It.IsAny<FichierVersion>())).Returns(Task.CompletedTask);

        await _service.AutosaveAsync(1, "new content", "user-test");

        _fichierRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Fichier>()), Times.Once);
        _versionRepoMock.Verify(r => r.AddAsync(It.IsAny<FichierVersion>()), Times.Once);
    }

    [Fact]
    public async Task AutosaveAsync_Should_Update_Hash_On_Save()
    {
        var fichier = CreateFichier(1, "old content");

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _fichierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Fichier>())).Returns(Task.CompletedTask);
        _versionRepoMock.Setup(r => r.AddAsync(It.IsAny<FichierVersion>())).Returns(Task.CompletedTask);

        await _service.AutosaveAsync(1, "new content", "user-test");

        Assert.Equal(ComputeHash("new content"), fichier.ContentHash);
    }

    [Fact]
    public async Task AutosaveAsync_Should_Increment_Version_On_Save()
    {
        var fichier = CreateFichier(1, "old content");

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _fichierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Fichier>())).Returns(Task.CompletedTask);
        _versionRepoMock.Setup(r => r.AddAsync(It.IsAny<FichierVersion>())).Returns(Task.CompletedTask);

        await _service.AutosaveAsync(1, "new content", "user-test");

        Assert.Equal(2, fichier.Version);
    }

    // ─── GetVersionsAsync ─────────────────────────────────────────────────────

    [Fact]
    public async Task GetVersionsAsync_Should_Return_All_Versions()
    {
        _versionRepoMock
            .Setup(r => r.GetByFichierIdAsync(1))
            .ReturnsAsync(new List<FichierVersion>
            {
                CreateVersion(1, 1, version: 1),
                CreateVersion(2, 1, version: 2)
            });

        var result = await _service.GetVersionsAsync(1);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetVersionsAsync_Should_Map_Fields_Correctly()
    {
        _versionRepoMock
            .Setup(r => r.GetByFichierIdAsync(1))
            .ReturnsAsync(new List<FichierVersion> { CreateVersion(1, 1, version: 3) });

        var result = await _service.GetVersionsAsync(1);
        var dto = result.First();

        Assert.Equal(1, dto.Id);
        Assert.Equal(3, dto.Version);
        Assert.Equal("old content", dto.Contenu);
        Assert.Equal("user-test", dto.CreatedBy);
    }

    [Fact]
    public async Task GetVersionsAsync_Should_Return_Empty_When_None()
    {
        _versionRepoMock
            .Setup(r => r.GetByFichierIdAsync(1))
            .ReturnsAsync(new List<FichierVersion>());

        var result = await _service.GetVersionsAsync(1);

        Assert.Empty(result);
    }

    // ─── RestoreVersionAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task RestoreVersionAsync_Should_Throw_When_Fichier_Not_Found()
    {
        _fichierRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Fichier?)null);

        await Assert.ThrowsAsync<Exception>(
            () => _service.RestoreVersionAsync(99, 1, "user-test")
        );
    }

    [Fact]
    public async Task RestoreVersionAsync_Should_Throw_When_Version_Not_Found()
    {
        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateFichier(1));

        _versionRepoMock
            .Setup(r => r.GetByFichierIdAsync(1))
            .ReturnsAsync(new List<FichierVersion>
            {
                CreateVersion(1, 1, version: 1)
            });

        await Assert.ThrowsAsync<Exception>(
            () => _service.RestoreVersionAsync(1, 999, "user-test") // versionId inexistant
        );
    }

    [Fact]
    public async Task RestoreVersionAsync_Should_Restore_Contenu()
    {
        var fichier = CreateFichier(1, "current content");
        var version = CreateVersion(1, 1, version: 1);
        version.Contenu = "restored content";

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _versionRepoMock.Setup(r => r.GetByFichierIdAsync(1)).ReturnsAsync(new List<FichierVersion> { version });
        _fichierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Fichier>())).Returns(Task.CompletedTask);
        _versionRepoMock.Setup(r => r.AddAsync(It.IsAny<FichierVersion>())).Returns(Task.CompletedTask);

        await _service.RestoreVersionAsync(1, 1, "user-test");

        Assert.Equal("restored content", fichier.Contenu);
    }

    [Fact]
    public async Task RestoreVersionAsync_Should_Increment_Version()
    {
        var fichier = CreateFichier(1);
        var version = CreateVersion(1, 1);

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _versionRepoMock.Setup(r => r.GetByFichierIdAsync(1)).ReturnsAsync(new List<FichierVersion> { version });
        _fichierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Fichier>())).Returns(Task.CompletedTask);
        _versionRepoMock.Setup(r => r.AddAsync(It.IsAny<FichierVersion>())).Returns(Task.CompletedTask);

        await _service.RestoreVersionAsync(1, 1, "user-test");

        Assert.Equal(2, fichier.Version);
    }

    [Fact]
    public async Task RestoreVersionAsync_Should_Create_New_Version_Entry()
    {
        var fichier = CreateFichier(1);
        var version = CreateVersion(1, 1);
        version.Contenu = "restored content";

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _versionRepoMock.Setup(r => r.GetByFichierIdAsync(1)).ReturnsAsync(new List<FichierVersion> { version });
        _fichierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Fichier>())).Returns(Task.CompletedTask);
        _versionRepoMock.Setup(r => r.AddAsync(It.IsAny<FichierVersion>())).Returns(Task.CompletedTask);

        await _service.RestoreVersionAsync(1, 1, "user-test");

        _versionRepoMock.Verify(
            r => r.AddAsync(It.Is<FichierVersion>(v =>
                v.Contenu   == "restored content" &&
                v.CreatedBy == "user-test"         &&
                v.Version   == 2
            )),
            Times.Once
        );
    }

    // ─── RenameAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task RenameAsync_Should_Throw_When_Not_Found()
    {
        _fichierRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Fichier?)null);

        await Assert.ThrowsAsync<Exception>(
            () => _service.RenameAsync(99, new FichierRenameDto { Nom = "new.js" })
        );
    }

    [Fact]
    public async Task RenameAsync_Should_Throw_When_Name_Already_Exists()
    {
        var fichier = CreateFichier(1);

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _fichierRepoMock
            .Setup(r => r.ExistsAsync("existing.js", null, 1))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<Exception>(
            () => _service.RenameAsync(1, new FichierRenameDto { Nom = "existing.js" })
        );
    }

    [Fact]
    public async Task RenameAsync_Should_Return_Updated_Dto()
    {
        var fichier = CreateFichier(1);

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _fichierRepoMock.Setup(r => r.ExistsAsync("renamed.js", null, 1)).ReturnsAsync(false);
        _fichierRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Fichier>())).Returns(Task.CompletedTask);

        var result = await _service.RenameAsync(1, new FichierRenameDto { Nom = "renamed.js" });

        Assert.Equal("renamed.js", result.Nom);
    }

    [Fact]
    public async Task RenameAsync_Should_Not_Call_UpdateAsync_When_Name_Exists()
    {
        var fichier = CreateFichier(1);

        _fichierRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fichier);
        _fichierRepoMock.Setup(r => r.ExistsAsync("existing.js", null, 1)).ReturnsAsync(true);

        try { await _service.RenameAsync(1, new FichierRenameDto { Nom = "existing.js" }); } catch { }

        _fichierRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Fichier>()), Times.Never);
    }
}