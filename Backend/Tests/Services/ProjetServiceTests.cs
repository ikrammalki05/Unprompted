using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Services;

public class ProjetServiceTests
{
    private readonly Mock<IProjetRepository> _projetRepoMock;
    private readonly Mock<IEnseignantRepository> _enseignantRepoMock;
    private readonly Mock<IEtudiantRepository> _etudiantRepoMock;
    private readonly ProjetService _service;

    public ProjetServiceTests()
    {
        _projetRepoMock    = new Mock<IProjetRepository>();
        _enseignantRepoMock = new Mock<IEnseignantRepository>();
        _etudiantRepoMock  = new Mock<IEtudiantRepository>();

        _service = new ProjetService(
            _projetRepoMock.Object,
            _enseignantRepoMock.Object,
            _etudiantRepoMock.Object
        );
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private Enseignant CreateEnseignant(int id = 1) => new Enseignant
    {
        IdEnseignant = id,
        IdUtilisateurNavigation = new Utilisateur
        {
            Prenom = "Prof",
            Nom    = "Dupont"
        }
    };

    private Etudiant CreateEtudiant(int id = 1) => new Etudiant
    {
        IdEtudiant = id,
        IdUtilisateurNavigation = new Utilisateur
        {
            Prenom = "Alice",
            Nom    = "Martin"
        }
    };

    private Projet CreateProjet(int id = 1, int idEnseignant = 1) => new Projet
    {
        IdProjet     = id,
        Titre        = "Projet " + id,
        Description  = "Description " + id,
        Statut       = "En cours",
        IdEnseignant = idEnseignant,
        Progression  = 0,
        Groupes      = new List<Groupe>()
    };

    private Projet CreateProjetWithGroupe(int id = 1, int idEnseignant = 1)
    {
        var groupe = new Groupe
        {
            IdGroupe     = 1,
            NomGroupe    = "Groupe 1",
            IdProjet     = id,
            Affectations = new List<Affectation>()
        };

        return new Projet
        {
            IdProjet     = id,
            Titre        = "Projet " + id,
            Statut       = "En cours",
            IdEnseignant = idEnseignant,
            Groupes      = new List<Groupe> { groupe }
        };
    }

    private Projet CreateProjetWithGroupeAndEtudiant(int idEtudiant = 1)
    {
        var affectation = new Affectation
        {
            IdEtudiant = idEtudiant,
            IdGroupe   = 1,
            IdEtudiantNavigation = new Etudiant
            {
                IdEtudiant = idEtudiant,
                IdUtilisateurNavigation = new Utilisateur
                {
                    Prenom = "Alice",
                    Nom    = "Martin"
                }
            },
            IdRoleNavigation = new Role { NomRole = "Développeur" }
        };

        var groupe = new Groupe
        {
            IdGroupe     = 1,
            NomGroupe    = "Groupe 1",
            IdProjet     = 1,
            Affectations = new List<Affectation> { affectation }
        };

        return new Projet
        {
            IdProjet     = 1,
            Titre        = "Projet 1",
            Statut       = "En cours",
            IdEnseignant = 1,
            Groupes      = new List<Groupe> { groupe }
        };
    }

    private ProjetCreateDto CreateProjetDto() => new ProjetCreateDto
    {
        Titre       = "Nouveau Projet",
        Description = "Description",
        Duree       = 90
    };

    private Contribution CreateContribution(int id, int idProjet, int idEtudiant) => new Contribution
    {
        IdContribution   = id,
        MessageCommit    = "Commit " + id,
        HashCommit       = "abc" + id,
        IdProjet         = idProjet,
        IdEtudiant       = idEtudiant,
        LignesAjoutees   = 10,
        LignesSupprimees = 2
    };

    private Prompt CreatePrompt(int id, int idProjet, int idEtudiant) => new Prompt
    {
        IdPrompt       = id,
        Contenu        = "Question " + id,
        NbTokensEntree = 100,
        NbTokensSortie = 50,
        IdProjet       = idProjet,
        IdEtudiant     = idEtudiant
    };

    // ─── GetAllProjectsAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetAllProjectsAsync_Should_Return_All_Projets()
    {
        _projetRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Projet> { CreateProjet(1), CreateProjet(2) });

        var result = await _service.GetAllProjectsAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllProjectsAsync_Should_Return_Empty_When_None()
    {
        _projetRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Projet>());

        var result = await _service.GetAllProjectsAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllProjectsAsync_Should_Map_Fields_Correctly()
    {
        _projetRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Projet> { CreateProjet(1, 1) });

        var result = await _service.GetAllProjectsAsync();
        var dto = result.First();

        Assert.Equal(1, dto.IDProjet);
        Assert.Equal("Projet 1", dto.Titre);
        Assert.Equal("En cours", dto.Status);
        Assert.Equal(1, dto.IdEnseignant);
    }

    // ─── GetProjectByIdAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetProjectByIdAsync_Should_Return_Dto_When_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));

        var result = await _service.GetProjectByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.IDProjet);
    }

    [Fact]
    public async Task GetProjectByIdAsync_Should_Return_Null_When_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Projet?)null);

        var result = await _service.GetProjectByIdAsync(99);

        Assert.Null(result);
    }

    // ─── GetProjectsByEnseignantIdAsync ───────────────────────────────────────

    [Fact]
    public async Task GetProjectsByEnseignantIdAsync_Should_Return_Projets_Of_Enseignant()
    {
        _projetRepoMock
            .Setup(r => r.GetByEnseignantIdAsync(1))
            .ReturnsAsync(new List<Projet> { CreateProjet(1, 1), CreateProjet(2, 1) });

        var result = await _service.GetProjectsByEnseignantIdAsync(1);

        Assert.Equal(2, result.Count());
        Assert.All(result, p => Assert.Equal(1, p.IdEnseignant));
    }

    // ─── CreateProjetAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CreateProjetAsync_Should_Return_Dto_When_Valid()
    {
        _enseignantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEnseignant(1));
        _projetRepoMock.Setup(r => r.AddAsync(It.IsAny<Projet>())).Returns(Task.CompletedTask);

        var result = await _service.CreateProjetAsync(1, CreateProjetDto());

        Assert.NotNull(result);
        Assert.Equal("Nouveau Projet", result.Titre);
        Assert.Equal("En cours", result.Status);
        Assert.Equal(0, result.Progression);
    }

    [Fact]
    public async Task CreateProjetAsync_Should_Throw_When_Enseignant_Not_Found()
    {
        _enseignantRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Enseignant?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateProjetAsync(99, CreateProjetDto())
        );
    }

    [Fact]
    public async Task CreateProjetAsync_Should_Call_AddAsync_Once()
    {
        _enseignantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEnseignant(1));
        _projetRepoMock.Setup(r => r.AddAsync(It.IsAny<Projet>())).Returns(Task.CompletedTask);

        await _service.CreateProjetAsync(1, CreateProjetDto());

        _projetRepoMock.Verify(
            r => r.AddAsync(It.Is<Projet>(p =>
                p.Titre        == "Nouveau Projet" &&
                p.IdEnseignant == 1                &&
                p.Statut       == "En cours"       &&
                p.Progression  == 0
            )),
            Times.Once
        );
    }

    // ─── UpdateProjetSuiviAsync ───────────────────────────────────────────────

    [Fact]
    public async Task UpdateProjetSuiviAsync_Should_Update_Progression()
    {
        var projet = CreateProjet(1);
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(projet);
        _projetRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Projet>())).Returns(Task.CompletedTask);

        await _service.UpdateProjetSuiviAsync(1, new ProjetSuiviDto { Progression = 75 });

        Assert.Equal(75, projet.Progression);
    }

    [Fact]
    public async Task UpdateProjetSuiviAsync_Should_Update_NotesEnseignant()
    {
        var projet = CreateProjet(1);
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(projet);
        _projetRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Projet>())).Returns(Task.CompletedTask);

        await _service.UpdateProjetSuiviAsync(1, new ProjetSuiviDto { NotesEnseignant = "Bon travail" });

        Assert.Equal("Bon travail", projet.NotesEnseignant);
    }

    [Fact]
    public async Task UpdateProjetSuiviAsync_Should_Throw_When_Projet_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Projet?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateProjetSuiviAsync(99, new ProjetSuiviDto { Progression = 50 })
        );
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    [InlineData(150)]
    public async Task UpdateProjetSuiviAsync_Should_Throw_When_Progression_Out_Of_Range(int progression)
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateProjetSuiviAsync(1, new ProjetSuiviDto { Progression = progression })
        );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public async Task UpdateProjetSuiviAsync_Should_Accept_Valid_Progression(int progression)
    {
        var projet = CreateProjet(1);
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(projet);
        _projetRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Projet>())).Returns(Task.CompletedTask);

        var ex = await Record.ExceptionAsync(
            () => _service.UpdateProjetSuiviAsync(1, new ProjetSuiviDto { Progression = progression })
        );

        Assert.Null(ex);
    }

    // ─── UpdateProjetAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateProjetAsync_Should_Throw_When_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Projet?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateProjetAsync(99, 1, CreateProjetDto())
        );
    }

    [Fact]
    public async Task UpdateProjetAsync_Should_Throw_When_Wrong_Enseignant()
    {
        var projet = CreateProjet(1, idEnseignant: 1);
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(projet);

        // idEnseignant = 2 tente de modifier le projet de l'enseignant 1
        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.UpdateProjetAsync(1, idEnseignant: 2, CreateProjetDto())
        );
    }

    [Fact]
    public async Task UpdateProjetAsync_Should_Update_Fields_When_Authorized()
    {
        var projet = CreateProjet(1, idEnseignant: 1);
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(projet);
        _projetRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Projet>())).Returns(Task.CompletedTask);

        await _service.UpdateProjetAsync(1, idEnseignant: 1, new ProjetCreateDto
        {
            Titre       = "Titre Modifié",
            Description = "Nouvelle description"
        });

        _projetRepoMock.Verify(
            r => r.UpdateAsync(It.Is<Projet>(p =>
                p.Titre       == "Titre Modifié" &&
                p.Description == "Nouvelle description"
            )),
            Times.Once
        );
    }

    // ─── DeleteProjetAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteProjetAsync_Should_Call_DeleteAsync_When_Authorized()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1, idEnseignant: 1));
        _projetRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _service.DeleteProjetAsync(1, idEnseignant: 1);

        _projetRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteProjetAsync_Should_Throw_When_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Projet?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.DeleteProjetAsync(99, 1)
        );
    }

    [Fact]
    public async Task DeleteProjetAsync_Should_Throw_When_Wrong_Enseignant()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1, idEnseignant: 1));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.DeleteProjetAsync(1, idEnseignant: 2)
        );
    }

    [Fact]
    public async Task DeleteProjetAsync_Should_Not_Call_DeleteAsync_When_Wrong_Enseignant()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1, idEnseignant: 1));

        try { await _service.DeleteProjetAsync(1, idEnseignant: 2); } catch { }

        _projetRepoMock.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
    }

    // ─── CreateContributionAsync ──────────────────────────────────────────────

    [Fact]
    public async Task CreateContributionAsync_Should_Return_Dto_When_Valid()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEtudiant(1));
        _projetRepoMock.Setup(r => r.AddContributionAsync(It.IsAny<Contribution>())).Returns(Task.CompletedTask);

        var dto = new ContributionCreateDto
        {
            MessageCommit    = "Initial commit",
            HashCommit       = "abc123",
            LignesAjoutees   = 20,
            LignesSupprimees = 5
        };

        var result = await _service.CreateContributionAsync(1, 1, dto);

        Assert.NotNull(result);
        Assert.Equal("Initial commit", result.MessageCommit);
        Assert.Equal(1, result.IdProjet);
        Assert.Equal(1, result.IdEtudiant);
    }

    [Fact]
    public async Task CreateContributionAsync_Should_Throw_When_Projet_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Projet?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateContributionAsync(99, 1, new ContributionCreateDto())
        );
    }

    [Fact]
    public async Task CreateContributionAsync_Should_Throw_When_Etudiant_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Etudiant?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.CreateContributionAsync(1, 99, new ContributionCreateDto())
        );
    }

    // ─── GetContributionsByProjectIdAsync ─────────────────────────────────────

    [Fact]
    public async Task GetContributionsByProjectIdAsync_Should_Return_Contributions()
    {
        _projetRepoMock
            .Setup(r => r.GetContributionsByProjectIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, 1),
                CreateContribution(2, 1, 1)
            });

        var result = await _service.GetContributionsByProjectIdAsync(1);

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetContributionsByProjectIdAsync_Should_Return_Empty_When_None()
    {
        _projetRepoMock
            .Setup(r => r.GetContributionsByProjectIdAsync(99))
            .ReturnsAsync(new List<Contribution>());

        var result = await _service.GetContributionsByProjectIdAsync(99);

        Assert.Empty(result);
    }

    // ─── AssignerEtudiantAsync ────────────────────────────────────────────────

    [Fact]
    public async Task AssignerEtudiantAsync_Should_Throw_When_Projet_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Projet?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.AssignerEtudiantAsync(
                new AssignerEtudiantProjetDto { IdProjet = 99, IdEtudiant = 1 }, 1)
        );
    }

    [Fact]
    public async Task AssignerEtudiantAsync_Should_Throw_When_Etudiant_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjetWithGroupe(1));
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Etudiant?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.AssignerEtudiantAsync(
                new AssignerEtudiantProjetDto { IdProjet = 1, IdEtudiant = 99 }, 1)
        );
    }

    [Fact]
    public async Task AssignerEtudiantAsync_Should_Throw_When_No_Groupe()
    {
        var projetSansGroupe = CreateProjet(1); // Groupes = []
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(projetSansGroupe);
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEtudiant(1));

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.AssignerEtudiantAsync(
                new AssignerEtudiantProjetDto { IdProjet = 1, IdEtudiant = 1 }, 1)
        );
    }

    [Fact]
    public async Task AssignerEtudiantAsync_Should_Call_AddAffectationAsync()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjetWithGroupe(1));
        _etudiantRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateEtudiant(1));
        _projetRepoMock.Setup(r => r.AddAffectationAsync(It.IsAny<Affectation>())).Returns(Task.CompletedTask);

        await _service.AssignerEtudiantAsync(
            new AssignerEtudiantProjetDto { IdProjet = 1, IdEtudiant = 1 }, idEnseignant: 1);

        _projetRepoMock.Verify(
            r => r.AddAffectationAsync(It.Is<Affectation>(a =>
                a.IdEtudiant  == 1 &&
                a.IdGroupe    == 1 &&
                a.IdEnseignant == 1
            )),
            Times.Once
        );
    }

    // ─── GetGroupesProjetAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetGroupesProjetAsync_Should_Throw_When_Projet_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Projet?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.GetGroupesProjetAsync(99)
        );
    }

    [Fact]
    public async Task GetGroupesProjetAsync_Should_Return_Empty_When_No_Groupes()
    {
        var projet = CreateProjet(1);
        projet.Groupes = null;
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(projet);

        var result = await _service.GetGroupesProjetAsync(1);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetGroupesProjetAsync_Should_Return_Groupes_With_Etudiants()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(CreateProjetWithGroupeAndEtudiant(1));

        var result = (await _service.GetGroupesProjetAsync(1)).ToList();

        Assert.Single(result);
        Assert.Single(result[0].Etudiants);
        Assert.Equal("Alice Martin", result[0].Etudiants[0].NomComplet);
        Assert.Equal("Développeur", result[0].Etudiants[0].Role);
    }

    // ─── GetColleguesAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task GetColleguesAsync_Should_Throw_When_Projet_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Projet?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.GetColleguesAsync(99, 1)
        );
    }

    [Fact]
    public async Task GetColleguesAsync_Should_Return_Empty_When_Etudiant_Not_In_Any_Groupe()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(CreateProjetWithGroupe(1)); // groupe sans affectation

        var result = await _service.GetColleguesAsync(1, idEtudiant: 99);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetColleguesAsync_Should_Return_Collegues()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(CreateProjetWithGroupeAndEtudiant(1));

        var result = await _service.GetColleguesAsync(1, idEtudiant: 1);

        Assert.Single(result);
        Assert.Equal("Alice Martin", result.First().NomComplet);
    }

    // ─── AssignerProjetAuGroupeAsync ──────────────────────────────────────────

    [Fact]
    public async Task AssignerProjetAuGroupeAsync_Should_Throw_When_Projet_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Projet?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.AssignerProjetAuGroupeAsync(
                new AssignerProjetGroupeDto { IdProjet = 99, IdGroupe = 1 })
        );
    }

    [Fact]
    public async Task AssignerProjetAuGroupeAsync_Should_Call_AssignProjectToGroupAsync()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));
        _projetRepoMock.Setup(r => r.AssignProjectToGroupAsync(1, 2)).Returns(Task.CompletedTask);

        await _service.AssignerProjetAuGroupeAsync(
            new AssignerProjetGroupeDto { IdProjet = 1, IdGroupe = 2 });

        _projetRepoMock.Verify(r => r.AssignProjectToGroupAsync(1, 2), Times.Once);
    }

    // ─── GetProjetsCountAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetProjetsCountAsync_Should_Return_Count()
    {
        _projetRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(7);

        var result = await _service.GetProjetsCountAsync();

        Assert.Equal(7, result);
    }

    // ─── CreatePromptAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CreatePromptAsync_Should_Return_Dto()
    {
        _projetRepoMock.Setup(r => r.AddPromptAsync(It.IsAny<Prompt>())).Returns(Task.CompletedTask);

        var result = await _service.CreatePromptAsync(1, new PromptCreateDto
        {
            Contenu        = "Comment faire X ?",
            IdEtudiant     = 1,
            NbTokensEntree = 200,
            NbTokensSortie = 100
        });

        Assert.NotNull(result);
        Assert.Equal("Comment faire X ?", result.Contenu);
        Assert.Equal(1, result.IdProjet);
        Assert.Equal(200, result.NbTokensEntree);
        Assert.Equal(100, result.NbTokensSortie);
    }

    // ─── GetEtudiantActiviteAsync ─────────────────────────────────────────────

    [Fact]
    public async Task GetEtudiantActiviteAsync_Should_Return_Prompts_And_Contributions()
    {
        _projetRepoMock
            .Setup(r => r.GetPromptsByProjectAndEtudiantAsync(1, 1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1, 1),
                CreatePrompt(2, 1, 1)
            });

        _projetRepoMock
            .Setup(r => r.GetContributionsByProjectAndEtudiantAsync(1, 1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, 1)
            });

        var result = await _service.GetEtudiantActiviteAsync(1, 1);

        Assert.Equal(2, result.Prompts.Count());
        Assert.Single(result.Contributions);
    }

    // ─── GetProjectsByEtudiantIdAsync ─────────────────────────────────────────

    [Fact]
    public async Task GetProjectsByEtudiantIdAsync_Should_Return_Projets()
    {
        _projetRepoMock
            .Setup(r => r.GetByEtudiantIdAsync(1))
            .ReturnsAsync(new List<Projet> { CreateProjet(1), CreateProjet(2) });

        var result = await _service.GetProjectsByEtudiantIdAsync(1);

        Assert.Equal(2, result.Count());
    }

    // ─── SaveCahierAsync / GetCahierAsync ─────────────────────────────────────

    [Fact]
    public async Task SaveCahierAsync_Should_Throw_When_Projet_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Projet?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            () => _service.SaveCahierAsync(99, new byte[] { 1, 2, 3 })
        );
    }

    [Fact]
    public async Task SaveCahierAsync_Should_Update_CahierDesCharges()
    {
        var projet = CreateProjet(1);
        var contenu = new byte[] { 0x25, 0x50, 0x44, 0x46 }; // PDF header

        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(projet);
        _projetRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Projet>())).Returns(Task.CompletedTask);

        await _service.SaveCahierAsync(1, contenu);

        _projetRepoMock.Verify(
            r => r.UpdateAsync(It.Is<Projet>(p =>
                p.CahierDesCharges != null &&
                p.CahierDesCharges.SequenceEqual(contenu)
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task GetCahierAsync_Should_Return_Null_When_Projet_Not_Found()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Projet?)null);

        var result = await _service.GetCahierAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetCahierAsync_Should_Return_Bytes_When_Found()
    {
        var contenu = new byte[] { 0x25, 0x50, 0x44, 0x46 };
        var projet  = CreateProjet(1);
        projet.CahierDesCharges = contenu;

        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(projet);

        var result = await _service.GetCahierAsync(1);

        Assert.NotNull(result);
        Assert.True(result.SequenceEqual(contenu));
    }
}