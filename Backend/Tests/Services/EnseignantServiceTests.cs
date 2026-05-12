using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Services;

public class EnseignantServiceTests
{
    private readonly Mock<IEnseignantRepository> _enseignantRepoMock;
    private readonly Mock<IUtilisateurRepository> _utilisateurRepoMock;
    private readonly Mock<IProjetRepository> _projetRepoMock;
    private readonly Mock<IKeycloakAdminService> _keycloakServiceMock;
    private readonly Mock<ILogger<EnseignantService>> _loggerMock;
    private readonly EnseignantService _service;

    public EnseignantServiceTests()
    {
        _enseignantRepoMock   = new Mock<IEnseignantRepository>();
        _utilisateurRepoMock  = new Mock<IUtilisateurRepository>();
        _projetRepoMock       = new Mock<IProjetRepository>();
        _keycloakServiceMock  = new Mock<IKeycloakAdminService>();
        _loggerMock           = new Mock<ILogger<EnseignantService>>();

        _service = new EnseignantService(
            _enseignantRepoMock.Object,
            _utilisateurRepoMock.Object,
            _projetRepoMock.Object,
            _keycloakServiceMock.Object,
            _loggerMock.Object
        );
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private Utilisateur CreateUtilisateur(int id = 1) => new Utilisateur
    {
        IdUtilisateur = id,
        Nom = "Dupont",
        Prenom = "Ahmed",
        Email = $"ahmed.dupont{id}@example.com",
        Statut = "Actif"
    };

    private Enseignant CreateEnseignant(int id = 1, int idUtilisateur = 1) => new Enseignant
    {
        IdEnseignant = id,
        IdUtilisateur = idUtilisateur,
        Specialite = "Informatique",
        IdUtilisateurNavigation = new Utilisateur
        {
            IdUtilisateur = idUtilisateur,
            Nom = "Dupont",
            Prenom = "Ahmed",
            Email = $"ahmed.dupont{id}@example.com",
            Statut = "Actif"
        }
    };

    private EnseignantCreateDto CreateEnseignantDto() => new EnseignantCreateDto
    {
        Nom = "Dupont",
        Prenom = "Ahmed",
        Email = "ahmed.dupont@example.com",
        Specialite = "Informatique"
    };

    // ─── GetAllEnseignantsAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetAllEnseignantsAsync_Should_Return_All_Enseignants()
    {
        _enseignantRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Enseignant>
            {
                CreateEnseignant(1, 1),
                CreateEnseignant(2, 2)
            });

        var result = await _service.GetAllEnseignantsAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllEnseignantsAsync_Should_Map_Fields_Correctly()
    {
        _enseignantRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Enseignant> { CreateEnseignant(1, 1) });

        var result = await _service.GetAllEnseignantsAsync();
        var dto = result.First();

        Assert.Equal(1, dto.Id);
        Assert.Equal("Ahmed Dupont", dto.NomComplet);
        Assert.Equal("ahmed.dupont1@example.com", dto.Email);
        Assert.Equal("Informatique", dto.Specialite);
        Assert.Equal("Actif", dto.Statut);
    }

    [Fact]
    public async Task GetAllEnseignantsAsync_Should_Return_Empty_When_None()
    {
        _enseignantRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Enseignant>());

        var result = await _service.GetAllEnseignantsAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllEnseignantsAsync_Should_Handle_Null_Navigation()
    {
        _enseignantRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Enseignant>
            {
                new Enseignant
                {
                    IdEnseignant = 1,
                    IdUtilisateur = 1,
                    IdUtilisateurNavigation = null // navigation null
                }
            });

        var result = await _service.GetAllEnseignantsAsync();
        var dto = result.First();

        Assert.Equal("Email inconnu", dto.Email);
        Assert.Equal("Inactif", dto.Statut);
        Assert.Equal("Non spécifiée", dto.Specialite);
    }

    // ─── CreateEnseignantAsync ────────────────────────────────────────────────

    [Fact]
    public async Task CreateEnseignantAsync_Should_Return_Dto_When_Valid()
    {
        var request = CreateEnseignantDto();

        _utilisateurRepoMock
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((Utilisateur?)null);

        _keycloakServiceMock
            .Setup(k => k.CreateUserAsync(request.Email, request.Prenom, request.Nom))
            .ReturnsAsync("keycloak-id-123");

        _keycloakServiceMock
            .Setup(k => k.SetUserPasswordAsync("keycloak-id-123", It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _utilisateurRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Utilisateur>()))
            .Returns(Task.CompletedTask);

        _enseignantRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Enseignant>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateEnseignantAsync(request);

        Assert.NotNull(result);
        Assert.Equal("Ahmed Dupont", result.NomComplet);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal("Informatique", result.Specialite);
        Assert.Equal("Actif", result.Statut);
    }

    [Fact]
    public async Task CreateEnseignantAsync_Should_Throw_When_Email_Already_Exists()
    {
        var request = CreateEnseignantDto();

        _utilisateurRepoMock
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync(CreateUtilisateur());

        await Assert.ThrowsAsync<Exception>(
            () => _service.CreateEnseignantAsync(request)
        );
    }

    [Fact]
    public async Task CreateEnseignantAsync_Should_Call_Keycloak_CreateUser()
    {
        var request = CreateEnseignantDto();

        _utilisateurRepoMock
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((Utilisateur?)null);

        _keycloakServiceMock
            .Setup(k => k.CreateUserAsync(request.Email, request.Prenom, request.Nom))
            .ReturnsAsync("keycloak-id-123");

        _keycloakServiceMock
            .Setup(k => k.SetUserPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _utilisateurRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Utilisateur>()))
            .Returns(Task.CompletedTask);

        _enseignantRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Enseignant>()))
            .Returns(Task.CompletedTask);

        await _service.CreateEnseignantAsync(request);

        _keycloakServiceMock.Verify(
            k => k.CreateUserAsync(request.Email, request.Prenom, request.Nom),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateEnseignantAsync_Should_Rollback_Keycloak_When_DB_Fails()
    {
        var request = CreateEnseignantDto();

        _utilisateurRepoMock
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((Utilisateur?)null);

        _keycloakServiceMock
            .Setup(k => k.CreateUserAsync(request.Email, request.Prenom, request.Nom))
            .ReturnsAsync("keycloak-id-123");

        _keycloakServiceMock
            .Setup(k => k.SetUserPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // La BD échoue
        _utilisateurRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Utilisateur>()))
            .ThrowsAsync(new Exception("DB error"));

        _keycloakServiceMock
            .Setup(k => k.DeleteUserAsync("keycloak-id-123"))
            .Returns(Task.CompletedTask);

        await Assert.ThrowsAsync<Exception>(
            () => _service.CreateEnseignantAsync(request)
        );

        // Vérifie que le rollback Keycloak a bien été appelé
        _keycloakServiceMock.Verify(
            k => k.DeleteUserAsync("keycloak-id-123"),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateEnseignantAsync_Should_Throw_When_Keycloak_Fails()
    {
        var request = CreateEnseignantDto();

        _utilisateurRepoMock
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((Utilisateur?)null);

        _keycloakServiceMock
            .Setup(k => k.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Keycloak unavailable"));

        await Assert.ThrowsAsync<Exception>(
            () => _service.CreateEnseignantAsync(request)
        );

        // Vérifie que la BD n'a pas été touchée
        _utilisateurRepoMock.Verify(
            r => r.AddAsync(It.IsAny<Utilisateur>()),
            Times.Never
        );
    }

    // ─── UpdateEnseignantAsync ────────────────────────────────────────────────

    [Fact]
    public async Task UpdateEnseignantAsync_Should_Update_Fields()
    {
        var enseignant = CreateEnseignant(1, 1);
        var utilisateur = CreateUtilisateur(1);
        var request = new EnseignantCreateDto
        {
            Nom = "NouveauNom",
            Prenom = "NouveauPrenom",
            Email = "nouveau@example.com",
            Specialite = "Mathématiques"
        };

        _enseignantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(enseignant);
        _utilisateurRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(utilisateur);
        _utilisateurRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Utilisateur>())).Returns(Task.CompletedTask);
        _enseignantRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Enseignant>())).Returns(Task.CompletedTask);

        await _service.UpdateEnseignantAsync(1, request);

        _utilisateurRepoMock.Verify(r => r.UpdateAsync(It.Is<Utilisateur>(u =>
            u.Nom == "NouveauNom" &&
            u.Prenom == "NouveauPrenom" &&
            u.Email == "nouveau@example.com"
        )), Times.Once);

        _enseignantRepoMock.Verify(r => r.UpdateAsync(It.Is<Enseignant>(e =>
            e.Specialite == "Mathématiques"
        )), Times.Once);
    }

    [Fact]
    public async Task UpdateEnseignantAsync_Should_Throw_When_Enseignant_Not_Found()
    {
        _enseignantRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Enseignant?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateEnseignantAsync(99, CreateEnseignantDto())
        );
    }

    [Fact]
    public async Task UpdateEnseignantAsync_Should_Throw_When_Utilisateur_Not_Found()
    {
        _enseignantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEnseignant());
        _utilisateurRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Utilisateur?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateEnseignantAsync(1, CreateEnseignantDto())
        );
    }

    // ─── DeleteEnseignantAsync ────────────────────────────────────────────────

    [Fact]
    public async Task DeleteEnseignantAsync_Should_Call_DeleteAsync()
    {
        _enseignantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEnseignant());
        _enseignantRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _service.DeleteEnseignantAsync(1);

        _enseignantRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteEnseignantAsync_Should_Throw_When_Not_Found()
    {
        _enseignantRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Enseignant?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.DeleteEnseignantAsync(99)
        );
    }

    // ─── GetStatistiquesAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetStatistiquesAsync_Should_Return_Correct_ProjetCount()
    {
        _projetRepoMock
            .Setup(r => r.GetByEnseignantIdAsync(1))
            .ReturnsAsync(new List<Projet>
            {
                new Projet { IdProjet = 1 },
                new Projet { IdProjet = 2 }
            });

        var result = await _service.GetStatistiquesAsync(1);

        Assert.Equal(2, result.ProjetsSupervisés);
    }

    [Fact]
    public async Task GetStatistiquesAsync_Should_Return_Zero_When_No_Projets()
    {
        _projetRepoMock
            .Setup(r => r.GetByEnseignantIdAsync(1))
            .ReturnsAsync(new List<Projet>());

        var result = await _service.GetStatistiquesAsync(1);

        Assert.Equal(0, result.ProjetsSupervisés);
        Assert.Equal(0, result.ÉtudiantsActifs);
    }
}