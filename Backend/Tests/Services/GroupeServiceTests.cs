using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Services;

public class GroupeServiceTests
{
    private readonly Mock<IGroupeRepository> _groupeRepoMock;
    private readonly Mock<IProjetRepository> _projetRepoMock;
    private readonly Mock<IEtudiantRepository> _etudiantRepoMock;
    private readonly GroupeService _service;

    public GroupeServiceTests()
    {
        _groupeRepoMock   = new Mock<IGroupeRepository>();
        _projetRepoMock   = new Mock<IProjetRepository>();
        _etudiantRepoMock = new Mock<IEtudiantRepository>();

        _service = new GroupeService(
            _groupeRepoMock.Object,
            _projetRepoMock.Object,
            _etudiantRepoMock.Object
        );
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private Groupe CreateGroupe(int id = 1, int idProjet = 1) => new Groupe
    {
        IdGroupe  = id,
        NomGroupe = "Groupe " + id,
        IdProjet  = idProjet,
        Affectations = new List<Affectation>()
    };

    private Groupe CreateGroupeWithEtudiants(int id = 1) => new Groupe
    {
        IdGroupe  = id,
        NomGroupe = "Groupe " + id,
        IdProjet  = 1,
        Affectations = new List<Affectation>
        {
            new Affectation
            {
                IdEtudiant = 1,
                IdGroupe   = id,
                IdEtudiantNavigation = new Etudiant
                {
                    IdEtudiant = 1,
                    IdUtilisateurNavigation = new Utilisateur
                    {
                        Prenom = "Alice",
                        Nom    = "Martin"
                    }
                },
                IdRoleNavigation = new Role { NomRole = "Développeur" }
            }
        }
    };

    private GroupeCreateDto CreateGroupeDto(int nbEtudiants = 1) => new GroupeCreateDto
    {
        NomGroupe = "Nouvelle Equipe",
        IdProjet  = 1,
        Etudiants = Enumerable.Range(1, nbEtudiants)
            .Select(i => new EtudiantRoleDto { IdEtudiant = i, IdRole = 1 })
            .ToList()
    };

    // ─── GetAllGroupesAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetAllGroupesAsync_Should_Return_All_Groupes()
    {
        _groupeRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Groupe>
            {
                CreateGroupe(1),
                CreateGroupe(2)
            });

        var result = await _service.GetAllGroupesAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllGroupesAsync_Should_Return_Empty_When_None()
    {
        _groupeRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Groupe>());

        var result = await _service.GetAllGroupesAsync();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllGroupesAsync_Should_Map_Fields_Correctly()
    {
        _groupeRepoMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Groupe> { CreateGroupeWithEtudiants(1) });

        var result = await _service.GetAllGroupesAsync();
        var dto = result.First();

        Assert.Equal(1, dto.IdGroupe);
        Assert.Equal("Groupe 1", dto.NomGroupe);
        Assert.Equal(1, dto.IdProjet);
        Assert.Single(dto.Etudiants);
        Assert.Equal("Alice Martin", dto.Etudiants[0].NomComplet);
        Assert.Equal("Développeur", dto.Etudiants[0].Role);
    }

    // ─── GetGroupeByIdAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task GetGroupeByIdAsync_Should_Return_Dto_When_Found()
    {
        _groupeRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(CreateGroupeWithEtudiants(1));

        var result = await _service.GetGroupeByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.IdGroupe);
        Assert.Equal("Groupe 1", result.NomGroupe);
    }

    [Fact]
    public async Task GetGroupeByIdAsync_Should_Return_Null_When_Not_Found()
    {
        _groupeRepoMock
            .Setup(r => r.GetByIdAsync(99))
            .ReturnsAsync((Groupe?)null);

        var result = await _service.GetGroupeByIdAsync(99);

        Assert.Null(result);
    }

    // ─── CreateGroupeAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task CreateGroupeAsync_Should_Return_Dto()
    {
        var dto            = CreateGroupeDto(nbEtudiants: 1);
        var createdGroupe  = CreateGroupeWithEtudiants(1);

        _groupeRepoMock.Setup(r => r.AddAsync(It.IsAny<Groupe>())).Returns(Task.CompletedTask);
        _groupeRepoMock.Setup(r => r.AddAffectationAsync(It.IsAny<Affectation>())).Returns(Task.CompletedTask);
        _groupeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(createdGroupe);

        var result = await _service.CreateGroupeAsync(dto);

        Assert.NotNull(result);
        Assert.Equal("Groupe 1", result.NomGroupe);
    }

    [Fact]
    public async Task CreateGroupeAsync_Should_Call_AddAsync_Once()
    {
        var dto = CreateGroupeDto();

        _groupeRepoMock.Setup(r => r.AddAsync(It.IsAny<Groupe>())).Returns(Task.CompletedTask);
        _groupeRepoMock.Setup(r => r.AddAffectationAsync(It.IsAny<Affectation>())).Returns(Task.CompletedTask);
        _groupeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(CreateGroupe());

        await _service.CreateGroupeAsync(dto);

        _groupeRepoMock.Verify(
            r => r.AddAsync(It.Is<Groupe>(g =>
                g.NomGroupe == "Nouvelle Equipe" &&
                g.IdProjet  == 1
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateGroupeAsync_Should_Create_Affectation_For_Each_Etudiant()
    {
        var dto = CreateGroupeDto(nbEtudiants: 3);

        _groupeRepoMock.Setup(r => r.AddAsync(It.IsAny<Groupe>())).Returns(Task.CompletedTask);
        _groupeRepoMock.Setup(r => r.AddAffectationAsync(It.IsAny<Affectation>())).Returns(Task.CompletedTask);
        _groupeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(CreateGroupe());

        await _service.CreateGroupeAsync(dto);

        _groupeRepoMock.Verify(
            r => r.AddAffectationAsync(It.IsAny<Affectation>()),
            Times.Exactly(3)
        );
    }

    [Fact]
    public async Task CreateGroupeAsync_Should_Create_No_Affectation_When_No_Etudiants()
    {
        var dto = new GroupeCreateDto
        {
            NomGroupe = "Groupe Vide",
            IdProjet  = 1,
            Etudiants = new List<EtudiantRoleDto>()
        };

        _groupeRepoMock.Setup(r => r.AddAsync(It.IsAny<Groupe>())).Returns(Task.CompletedTask);
        _groupeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(CreateGroupe());

        await _service.CreateGroupeAsync(dto);

        _groupeRepoMock.Verify(
            r => r.AddAffectationAsync(It.IsAny<Affectation>()),
            Times.Never
        );
    }

    [Fact]
    public async Task CreateGroupeAsync_Should_Use_Zero_When_IdProjet_Is_Null()
    {
        var dto = new GroupeCreateDto
        {
            NomGroupe = "Groupe Sans Projet",
            IdProjet  = null,
            Etudiants = new List<EtudiantRoleDto>()
        };

        _groupeRepoMock.Setup(r => r.AddAsync(It.IsAny<Groupe>())).Returns(Task.CompletedTask);
        _groupeRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(CreateGroupe());

        await _service.CreateGroupeAsync(dto);

        _groupeRepoMock.Verify(
            r => r.AddAsync(It.Is<Groupe>(g => g.IdProjet == 0)),
            Times.Once
        );
    }

    // ─── DeleteGroupeAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteGroupeAsync_Should_Remove_Affectations_Then_Groupe()
    {
        var callOrder = new List<string>();

        _groupeRepoMock
            .Setup(r => r.RemoveAllAffectationsForGroupeAsync(1))
            .Callback(() => callOrder.Add("affectations"))
            .Returns(Task.CompletedTask);

        _groupeRepoMock
            .Setup(r => r.DeleteAsync(1))
            .Callback(() => callOrder.Add("groupe"))
            .Returns(Task.CompletedTask);

        await _service.DeleteGroupeAsync(1);

        Assert.Equal("affectations", callOrder[0]);
        Assert.Equal("groupe", callOrder[1]);
    }

    [Fact]
    public async Task DeleteGroupeAsync_Should_Call_RemoveAllAffectations_Once()
    {
        _groupeRepoMock.Setup(r => r.RemoveAllAffectationsForGroupeAsync(1)).Returns(Task.CompletedTask);
        _groupeRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _service.DeleteGroupeAsync(1);

        _groupeRepoMock.Verify(r => r.RemoveAllAffectationsForGroupeAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteGroupeAsync_Should_Call_DeleteAsync_Once()
    {
        _groupeRepoMock.Setup(r => r.RemoveAllAffectationsForGroupeAsync(1)).Returns(Task.CompletedTask);
        _groupeRepoMock.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _service.DeleteGroupeAsync(1);

        _groupeRepoMock.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    // ─── AddEtudiantToGroupeAsync ─────────────────────────────────────────────

    [Fact]
    public async Task AddEtudiantToGroupeAsync_Should_Call_AddAffectation_With_Correct_Fields()
    {
        _groupeRepoMock
            .Setup(r => r.AddAffectationAsync(It.IsAny<Affectation>()))
            .Returns(Task.CompletedTask);

        await _service.AddEtudiantToGroupeAsync(1, new EtudiantRoleDto
        {
            IdEtudiant = 5,
            IdRole     = 2
        });

        _groupeRepoMock.Verify(
            r => r.AddAffectationAsync(It.Is<Affectation>(a =>
                a.IdGroupe   == 1 &&
                a.IdEtudiant == 5 &&
                a.IdRole     == 2
            )),
            Times.Once
        );
    }

    // ─── RemoveEtudiantFromGroupeAsync ────────────────────────────────────────

    [Fact]
    public async Task RemoveEtudiantFromGroupeAsync_Should_Call_RemoveAffectation_With_Correct_Ids()
    {
        _groupeRepoMock
            .Setup(r => r.RemoveAffectationAsync(3, 1))
            .Returns(Task.CompletedTask);

        await _service.RemoveEtudiantFromGroupeAsync(idGroupe: 1, idEtudiant: 3);

        _groupeRepoMock.Verify(
            r => r.RemoveAffectationAsync(3, 1),
            Times.Once
        );
    }

    // ─── MapToDto : navigations nulles ────────────────────────────────────────

    [Fact]
    public async Task GetGroupeByIdAsync_Should_Use_Fallback_When_Navigation_Null()
    {
        var groupe = new Groupe
        {
            IdGroupe  = 1,
            NomGroupe = "Groupe 1",
            IdProjet  = 1,
            Affectations = new List<Affectation>
            {
                new Affectation
                {
                    IdEtudiant           = 1,
                    IdEtudiantNavigation = null, // navigation null
                    IdRoleNavigation     = null  // navigation null
                }
            }
        };

        _groupeRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(groupe);

        var result = await _service.GetGroupeByIdAsync(1);

        Assert.Equal("Etudiant inconnu", result!.Etudiants[0].NomComplet);
        Assert.Equal("Aucun rôle",       result.Etudiants[0].Role);
    }
}