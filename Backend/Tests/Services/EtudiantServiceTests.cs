using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Services;

public class EtudiantServiceTests
{
    private readonly Mock<IEtudiantRepository> _etudiantRepoMock;
    private readonly Mock<IUtilisateurRepository> _utilisateurRepoMock;
    private readonly Mock<IKeycloakAdminService> _keycloakServiceMock;
    private readonly Mock<ILogger<EtudiantService>> _loggerMock;
    private readonly Mock<IContributionRepository> _contributionRepoMock;
    private readonly Mock<IPromptRepository> _promptRepoMock;
    private readonly Mock<IEvaluationRepository> _evaluationRepoMock;
    private readonly EtudiantService _service;

    public EtudiantServiceTests()
    {
        _etudiantRepoMock     = new Mock<IEtudiantRepository>();
        _utilisateurRepoMock  = new Mock<IUtilisateurRepository>();
        _keycloakServiceMock  = new Mock<IKeycloakAdminService>();
        _loggerMock           = new Mock<ILogger<EtudiantService>>();
        _contributionRepoMock = new Mock<IContributionRepository>();
        _promptRepoMock       = new Mock<IPromptRepository>();
        _evaluationRepoMock   = new Mock<IEvaluationRepository>();

        _service = new EtudiantService(
            _etudiantRepoMock.Object,
            _utilisateurRepoMock.Object,
            _keycloakServiceMock.Object,
            _loggerMock.Object,
            _contributionRepoMock.Object,
            _promptRepoMock.Object,
            _evaluationRepoMock.Object
        );
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private Utilisateur CreateUtilisateur(int id = 1) => new Utilisateur
    {
        IdUtilisateur = id,
        Nom           = "Martin",
        Prenom        = "Alice",
        Email         = $"alice.martin{id}@example.com",
        Statut        = "Actif"
    };

    private Etudiant CreateEtudiant(int id = 1, int idUtilisateur = 1) => new Etudiant
    {
        IdEtudiant    = id,
        IdUtilisateur = idUtilisateur,
        CodeApogee    = "E1234" + id,
        Niveau        = "L3",
        Filiere       = "Informatique",
        IdUtilisateurNavigation = new Utilisateur
        {
            IdUtilisateur = idUtilisateur,
            Nom           = "Martin",
            Prenom        = "Alice",
            Email         = $"alice.martin{id}@example.com",
            Statut        = "Actif"
        }
    };

    private EtudiantCreateDto CreateEtudiantDto() => new EtudiantCreateDto
    {
        Nom        = "Martin",
        Prenom     = "Alice",
        Email      = "alice.martin@example.com",
        CodeApogee = "E12345"
    };

    private Contribution CreateContribution(int id, int idEtudiant) => new Contribution
    {
        IdContribution   = id,
        IdEtudiant       = idEtudiant,
        MessageCommit    = "Commit " + id,
        DateCommit       = DateTime.UtcNow,
        LignesAjoutees   = 10,
        LignesSupprimees = 2,
        IdProjetNavigation = new Projet { IdProjet = 1, Titre = "Projet Test" }
    };

    private Prompt CreatePrompt(int id, int idEtudiant) => new Prompt
    {
        IdPrompt           = id,
        IdEtudiant         = idEtudiant,
        Contenu            = "Question " + id,
        DatePrompt         = DateTime.UtcNow,
        NbTokensEntree     = 100,
        NbTokensSortie     = 50,
        IdProjetNavigation = new Projet { IdProjet = 1, Titre = "Projet Test" }
    };

    private Evaluation CreateEvaluation(int id, int idEtudiant) => new Evaluation
    {
        IdEvaluation    = id,
        IdEtudiant      = idEtudiant,
        Note            = 15,
        Commentaire     = "Bon travail",
        DateEvaluation  = DateTime.UtcNow,
        IdProjetNavigation = new Projet { IdProjet = 1, Titre = "Projet Test" },
        IdEnseignantNavigation = new Enseignant
        {
            IdEnseignant = 1,
            IdUtilisateurNavigation = new Utilisateur
            {
                Prenom = "Prof",
                Nom    = "Dupont"
            }
        }
    };

    // ─── CreateEtudiantAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task CreateEtudiantAsync_Should_Return_Dto_When_Valid()
    {
        var request = CreateEtudiantDto();

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

        _etudiantRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Etudiant>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateEtudiantAsync(request);

        Assert.NotNull(result);
        Assert.Equal("Alice Martin", result.NomComplet);
        Assert.Equal(request.Email, result.Email);
        Assert.Equal("E12345", result.CodeApogee);
        Assert.Equal("Actif", result.Statut);
        Assert.Equal("Non assigné", result.ClasseNom);
    }

    [Fact]
    public async Task CreateEtudiantAsync_Should_Throw_When_Email_Already_Exists()
    {
        var request = CreateEtudiantDto();

        _utilisateurRepoMock
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync(CreateUtilisateur());

        await Assert.ThrowsAsync<Exception>(
            () => _service.CreateEtudiantAsync(request)
        );
    }

    [Fact]
    public async Task CreateEtudiantAsync_Should_Throw_When_Keycloak_Fails()
    {
        var request = CreateEtudiantDto();

        _utilisateurRepoMock
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((Utilisateur?)null);

        _keycloakServiceMock
            .Setup(k => k.CreateUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception("Keycloak unavailable"));

        await Assert.ThrowsAsync<Exception>(
            () => _service.CreateEtudiantAsync(request)
        );

        _utilisateurRepoMock.Verify(r => r.AddAsync(It.IsAny<Utilisateur>()), Times.Never);
    }

    [Fact]
    public async Task CreateEtudiantAsync_Should_Rollback_Keycloak_When_DB_Fails()
    {
        var request = CreateEtudiantDto();

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
            .ThrowsAsync(new Exception("DB error"));

        _keycloakServiceMock
            .Setup(k => k.DeleteUserAsync("keycloak-id-123"))
            .Returns(Task.CompletedTask);

        await Assert.ThrowsAsync<Exception>(
            () => _service.CreateEtudiantAsync(request)
        );

        _keycloakServiceMock.Verify(k => k.DeleteUserAsync("keycloak-id-123"), Times.Once);
    }

    [Fact]
    public async Task CreateEtudiantAsync_Should_Set_Password_With_Prenom_And_CodeApogee()
    {
        var request = CreateEtudiantDto();

        _utilisateurRepoMock
            .Setup(r => r.GetByEmailAsync(request.Email))
            .ReturnsAsync((Utilisateur?)null);

        _keycloakServiceMock
            .Setup(k => k.CreateUserAsync(request.Email, request.Prenom, request.Nom))
            .ReturnsAsync("keycloak-id-123");

        _keycloakServiceMock
            .Setup(k => k.SetUserPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _utilisateurRepoMock.Setup(r => r.AddAsync(It.IsAny<Utilisateur>())).Returns(Task.CompletedTask);
        _etudiantRepoMock.Setup(r => r.AddAsync(It.IsAny<Etudiant>())).Returns(Task.CompletedTask);

        await _service.CreateEtudiantAsync(request);

        // Le mot de passe = Prenom + CodeApogee
        _keycloakServiceMock.Verify(
            k => k.SetUserPasswordAsync("keycloak-id-123", "AliceE12345"),
            Times.Once
        );
    }

    // ─── GetAllEtudiantsAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetAllEtudiantsAsync_Should_Return_All_Etudiants()
    {
        _etudiantRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Etudiant> { CreateEtudiant(1), CreateEtudiant(2, 2) });

        var result = await _service.GetAllEtudiantsAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllEtudiantsAsync_Should_Map_Fields_Correctly()
    {
        _etudiantRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Etudiant> { CreateEtudiant(1) });

        var result = await _service.GetAllEtudiantsAsync();
        var dto = result.First();

        Assert.Equal(1, dto.Id);
        Assert.Equal("Alice Martin", dto.NomComplet);
        Assert.Equal("alice.martin1@example.com", dto.Email);
        Assert.Equal("Actif", dto.Statut);
        Assert.Equal("E12341", dto.CodeApogee);
    }

    [Fact]
    public async Task GetAllEtudiantsAsync_Should_Handle_Null_Navigation()
    {
        _etudiantRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Etudiant>
            {
                new Etudiant
                {
                    IdEtudiant = 1,
                    IdUtilisateurNavigation = null
                }
            });

        var result = await _service.GetAllEtudiantsAsync();
        var dto = result.First();

        Assert.Equal("Email inconnu", dto.Email);
        Assert.Equal("Inactif", dto.Statut);
        Assert.Equal("Non assigné", dto.ClasseNom);
    }

    [Fact]
    public async Task GetAllEtudiantsAsync_Should_Return_Empty_When_None()
    {
        _etudiantRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Etudiant>());

        var result = await _service.GetAllEtudiantsAsync();

        Assert.Empty(result);
    }

    // ─── UpdateEtudiantAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task UpdateEtudiantAsync_Should_Update_Fields()
    {
        var etudiant   = CreateEtudiant(1, 1);
        var utilisateur = CreateUtilisateur(1);
        var request = new EtudiantCreateDto
        {
            Nom        = "NouveauNom",
            Prenom     = "NouveauPrenom",
            Email      = "nouveau@example.com",
            CodeApogee = "E99999"
        };

        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(etudiant);
        _utilisateurRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(utilisateur);
        _utilisateurRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Utilisateur>())).Returns(Task.CompletedTask);
        _etudiantRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Etudiant>())).Returns(Task.CompletedTask);

        await _service.UpdateEtudiantAsync(1, request);

        _utilisateurRepoMock.Verify(r => r.UpdateAsync(It.Is<Utilisateur>(u =>
            u.Nom    == "NouveauNom"        &&
            u.Prenom == "NouveauPrenom"     &&
            u.Email  == "nouveau@example.com"
        )), Times.Once);

        _etudiantRepoMock.Verify(r => r.UpdateAsync(It.Is<Etudiant>(e =>
            e.CodeApogee == "E99999"
        )), Times.Once);
    }

    [Fact]
    public async Task UpdateEtudiantAsync_Should_Throw_When_Etudiant_Not_Found()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Etudiant?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateEtudiantAsync(99, CreateEtudiantDto())
        );
    }

    [Fact]
    public async Task UpdateEtudiantAsync_Should_Throw_When_Utilisateur_Not_Found()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEtudiant());
        _utilisateurRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Utilisateur?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateEtudiantAsync(1, CreateEtudiantDto())
        );
    }

    // ─── DeleteEtudiantAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task DeleteEtudiantAsync_Should_Call_DeleteAsync()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEtudiant());
        _etudiantRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _service.DeleteEtudiantAsync(1);

        _etudiantRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteEtudiantAsync_Should_Throw_When_Not_Found()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Etudiant?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.DeleteEtudiantAsync(99)
        );
    }

    // ─── GetProfilEtudiantAsync ───────────────────────────────────────────────

    [Fact]
    public async Task GetProfilEtudiantAsync_Should_Return_Null_When_Not_Found()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Etudiant?)null);

        var result = await _service.GetProfilEtudiantAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetProfilEtudiantAsync_Should_Return_Dto_When_Found()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEtudiant(1));

        var result = await _service.GetProfilEtudiantAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdEtudiant);
        Assert.Equal("E12341", result.CodeApogee);
        Assert.Equal("L3", result.Niveau);
        Assert.Equal("Informatique", result.Filiere);
        Assert.Equal("Alice Martin", result.NomComplet);
        Assert.Equal("alice.martin1@example.com", result.Email);
        Assert.Equal("Actif", result.Statut);
    }

    [Fact]
    public async Task GetProfilEtudiantAsync_Should_Handle_Null_Navigation()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Etudiant
        {
            IdEtudiant              = 1,
            CodeApogee              = "E00001",
            IdUtilisateurNavigation = null
        });

        var result = await _service.GetProfilEtudiantAsync(1);

        Assert.Equal("Inconnu", result!.Email);
        Assert.Equal("Inconnu", result.Statut);
        Assert.Equal("Non spécifié", result.Niveau);
        Assert.Equal("Non spécifiée", result.Filiere);
    }

    // ─── GetHistoriqueAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetHistoriqueAsync_Should_Return_Null_When_Etudiant_Not_Found()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Etudiant?)null);

        var result = await _service.GetHistoriqueAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetHistoriqueAsync_Should_Return_Contributions()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEtudiant(1));

        _contributionRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1),
                CreateContribution(2, 1)
            });

        _promptRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Prompt>());

        _evaluationRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Evaluation>());

        var result = await _service.GetHistoriqueAsync(1);

        Assert.NotNull(result);
        Assert.Equal(2, result.ContributionsGit.Count);
        Assert.Equal("Commit 1", result.ContributionsGit[0].MessageCommit);
        Assert.Equal(10, result.ContributionsGit[0].LignesAjoutees);
        Assert.Equal("Projet Test", result.ContributionsGit[0].NomProjet);
    }

    [Fact]
    public async Task GetHistoriqueAsync_Should_Return_Prompts()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEtudiant(1));

        _contributionRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Contribution>());

        _promptRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1),
                CreatePrompt(2, 1)
            });

        _evaluationRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Evaluation>());

        var result = await _service.GetHistoriqueAsync(1);

        Assert.Equal(2, result!.InteractionsIa.Count);
        Assert.Equal("Question 1", result.InteractionsIa[0].Question);
        Assert.Equal(150, result.InteractionsIa[0].TokensConsommes); // 100 + 50
        Assert.Equal("Projet Test", result.InteractionsIa[0].NomProjet);
    }

    [Fact]
    public async Task GetHistoriqueAsync_Should_Return_Evaluations()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEtudiant(1));

        _contributionRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Contribution>());

        _promptRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Prompt>());

        _evaluationRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Evaluation> { CreateEvaluation(1, 1) });

        var result = await _service.GetHistoriqueAsync(1);

        Assert.Single(result!.Evaluations);
        Assert.Equal(15, result.Evaluations[0].Note);
        Assert.Equal("Bon travail", result.Evaluations[0].Commentaire);
        Assert.Equal("Projet Test", result.Evaluations[0].NomProjet);
        Assert.Equal("Prof Dupont", result.Evaluations[0].NomEnseignant);
    }

    [Fact]
    public async Task GetHistoriqueAsync_Should_Return_Empty_Lists_When_No_Data()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEtudiant(1));

        _contributionRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Contribution>());

        _promptRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Prompt>());

        _evaluationRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Evaluation>());

        var result = await _service.GetHistoriqueAsync(1);

        Assert.NotNull(result);
        Assert.Empty(result.ContributionsGit);
        Assert.Empty(result.InteractionsIa);
        Assert.Empty(result.Evaluations);
    }

    [Fact]
    public async Task GetHistoriqueAsync_Should_Handle_Null_Navigation_In_Evaluations()
    {
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEtudiant(1));

        _contributionRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Contribution>());

        _promptRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Prompt>());

        _evaluationRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Evaluation>
            {
                new Evaluation
                {
                    IdEvaluation           = 1,
                    Note                   = 12,
                    Commentaire            = null,
                    DateEvaluation         = DateTime.UtcNow,
                    IdProjetNavigation     = null, // navigation null
                    IdEnseignantNavigation = null  // navigation null
                }
            });

        var result = await _service.GetHistoriqueAsync(1);

        Assert.Single(result!.Evaluations);
        Assert.Equal("Inconnu", result.Evaluations[0].NomProjet);
        Assert.Equal("Inconnu", result.Evaluations[0].NomEnseignant);
        Assert.Equal(string.Empty, result.Evaluations[0].Commentaire);
    }
}