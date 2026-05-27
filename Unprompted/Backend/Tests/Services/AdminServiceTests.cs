using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Services;

public class AdminServiceTests
{
    private readonly Mock<IEtudiantRepository> _etudiantRepoMock;
    private readonly Mock<IEnseignantRepository> _enseignantRepoMock;
    private readonly Mock<IClasseRepository> _classeRepoMock;
    private readonly Mock<IAffectationRepository> _affectationRepoMock;
    private readonly Mock<IEnseignantClasseRepository> _enseignantClasseRepoMock;
    private readonly AdminService _service;

    public AdminServiceTests()
    {
        _etudiantRepoMock        = new Mock<IEtudiantRepository>();
        _enseignantRepoMock      = new Mock<IEnseignantRepository>();
        _classeRepoMock          = new Mock<IClasseRepository>();
        _affectationRepoMock     = new Mock<IAffectationRepository>();
        _enseignantClasseRepoMock = new Mock<IEnseignantClasseRepository>();

        _service = new AdminService(
            _etudiantRepoMock.Object,
            _enseignantRepoMock.Object,
            _classeRepoMock.Object,
            _affectationRepoMock.Object,
            _enseignantClasseRepoMock.Object
        );
    }

    // ─── GetDashboardStatsAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetDashboardStatsAsync_Should_Return_Correct_Stats()
    {
        _etudiantRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(10);
        _enseignantRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(5);
        _classeRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(3);

        var result = await _service.GetDashboardStatsAsync();

        // Cast en dynamic pour accéder aux propriétés de l'objet anonyme
        dynamic stats = result;
        Assert.Equal(10, (int)stats.TotalEtudiants);
        Assert.Equal(5,  (int)stats.TotalEnseignants);
        Assert.Equal(3,  (int)stats.TotalClasses);
    }

    [Fact]
    public async Task GetDashboardStatsAsync_Should_Return_Zero_When_Empty()
    {
        _etudiantRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(0);
        _enseignantRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(0);
        _classeRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(0);

        dynamic stats = await _service.GetDashboardStatsAsync();

        Assert.Equal(0, (int)stats.TotalEtudiants);
        Assert.Equal(0, (int)stats.TotalEnseignants);
        Assert.Equal(0, (int)stats.TotalClasses);
    }

    // ─── AssignerEnseignantAClasseAsync ───────────────────────────────────────

    [Fact]
    public async Task AssignerEnseignantAClasseAsync_Should_Return_True_When_Valid()
    {
        var request = new AffectationEnseignantRequestDto
        {
            IdEnseignant = 1,
            IdClasse = 1
        };

        _enseignantRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Enseignant { IdEnseignant = 1 });

        _classeRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Classe { IdClasse = 1 });

        _enseignantClasseRepoMock
            .Setup(r => r.AddAsync(It.IsAny<EnseignantClasse>()))
            .Returns(Task.CompletedTask);

        var result = await _service.AssignerEnseignantAClasseAsync(request);

        Assert.True(result);
    }

    [Fact]
    public async Task AssignerEnseignantAClasseAsync_Should_Call_AddAsync_Once()
    {
        var request = new AffectationEnseignantRequestDto
        {
            IdEnseignant = 1,
            IdClasse = 1
        };

        _enseignantRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Enseignant { IdEnseignant = 1 });

        _classeRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Classe { IdClasse = 1 });

        _enseignantClasseRepoMock
            .Setup(r => r.AddAsync(It.IsAny<EnseignantClasse>()))
            .Returns(Task.CompletedTask);

        await _service.AssignerEnseignantAClasseAsync(request);

        // Vérifie que AddAsync a bien été appelé une seule fois
        _enseignantClasseRepoMock.Verify(
            r => r.AddAsync(It.Is<EnseignantClasse>(a =>
                a.IdEnseignant == 1 &&
                a.IdClasse == 1
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task AssignerEnseignantAClasseAsync_Should_Throw_When_Enseignant_Not_Found()
    {
        var request = new AffectationEnseignantRequestDto
        {
            IdEnseignant = 99,
            IdClasse = 1
        };

        _enseignantRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Enseignant?)null);

        _classeRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Classe { IdClasse = 1 });

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.AssignerEnseignantAClasseAsync(request)
        );
    }

    [Fact]
    public async Task AssignerEnseignantAClasseAsync_Should_Throw_When_Classe_Not_Found()
    {
        var request = new AffectationEnseignantRequestDto
        {
            IdEnseignant = 1,
            IdClasse = 99
        };

        _enseignantRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(new Enseignant { IdEnseignant = 1 });

        _classeRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Classe?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.AssignerEnseignantAClasseAsync(request)
        );
    }

    [Fact]
    public async Task AssignerEnseignantAClasseAsync_Should_Throw_When_Both_Not_Found()
    {
        var request = new AffectationEnseignantRequestDto
        {
            IdEnseignant = 99,
            IdClasse = 99
        };

        _enseignantRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Enseignant?)null);

        _classeRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Classe?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.AssignerEnseignantAClasseAsync(request)
        );
    }
}