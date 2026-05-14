using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Services;

public class GroupeProjetServiceTests
{
    private readonly Mock<IGroupeRepository> _groupeRepoMock;
    private readonly Mock<IAffectationRepository> _affectationRepoMock;
    private readonly Mock<IProjetRepository> _projetRepoMock;
    private readonly GroupeProjetService _service;

    public GroupeProjetServiceTests()
    {
        _groupeRepoMock      = new Mock<IGroupeRepository>();
        _affectationRepoMock = new Mock<IAffectationRepository>();
        _projetRepoMock      = new Mock<IProjetRepository>();

        _service = new GroupeProjetService(
            _groupeRepoMock.Object,
            _affectationRepoMock.Object,
            _projetRepoMock.Object
        );
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private Projet CreateProjet(int id = 1) => new Projet
    {
        IdProjet = id,
        Titre    = "Projet " + id
    };

    private AssignationProjetRequestDto CreateRequest(int nbEtudiants = 2) => new AssignationProjetRequestDto
    {
        IdProjet  = 1,
        NomGroupe = "Equipe Alpha",
        Etudiants = Enumerable.Range(1, nbEtudiants)
            .Select(i => new EtudiantRoleDto
            {
                IdEtudiant = i,
                IdRole     = 1
            })
            .ToList()
    };

    // ─── CreerEquipeEtAssignerAsync : projet introuvable ──────────────────────

    [Fact]
    public async Task CreerEquipeEtAssignerAsync_Should_Throw_When_Projet_Not_Found()
    {
        _projetRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Projet?)null);

        await Assert.ThrowsAsync<Exception>(
            () => _service.CreerEquipeEtAssignerAsync(CreateRequest())
        );
    }

    [Fact]
    public async Task CreerEquipeEtAssignerAsync_Should_Not_Create_Groupe_When_Projet_Not_Found()
    {
        _projetRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Projet?)null);

        try { await _service.CreerEquipeEtAssignerAsync(CreateRequest()); } catch { }

        _groupeRepoMock.Verify(r => r.AddAsync(It.IsAny<Groupe>()), Times.Never);
    }

    [Fact]
    public async Task CreerEquipeEtAssignerAsync_Should_Not_Create_Affectations_When_Projet_Not_Found()
    {
        _projetRepoMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((Projet?)null);

        try { await _service.CreerEquipeEtAssignerAsync(CreateRequest()); } catch { }

        _affectationRepoMock.Verify(r => r.AddAsync(It.IsAny<Affectation>()), Times.Never);
    }

    // ─── CreerEquipeEtAssignerAsync : cas valide ──────────────────────────────

    [Fact]
    public async Task CreerEquipeEtAssignerAsync_Should_Return_True_When_Valid()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));
        _groupeRepoMock.Setup(r => r.AddAsync(It.IsAny<Groupe>())).Returns(Task.CompletedTask);
        _affectationRepoMock.Setup(r => r.AddAsync(It.IsAny<Affectation>())).Returns(Task.CompletedTask);

        var result = await _service.CreerEquipeEtAssignerAsync(CreateRequest());

        Assert.True(result);
    }

    [Fact]
    public async Task CreerEquipeEtAssignerAsync_Should_Create_Groupe_With_Correct_Fields()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));
        _groupeRepoMock.Setup(r => r.AddAsync(It.IsAny<Groupe>())).Returns(Task.CompletedTask);
        _affectationRepoMock.Setup(r => r.AddAsync(It.IsAny<Affectation>())).Returns(Task.CompletedTask);

        await _service.CreerEquipeEtAssignerAsync(CreateRequest());

        _groupeRepoMock.Verify(
            r => r.AddAsync(It.Is<Groupe>(g =>
                g.NomGroupe == "Equipe Alpha" &&
                g.IdProjet  == 1
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task CreerEquipeEtAssignerAsync_Should_Create_Affectation_For_Each_Etudiant()
    {
        var request = CreateRequest(nbEtudiants: 3);

        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));
        _groupeRepoMock.Setup(r => r.AddAsync(It.IsAny<Groupe>())).Returns(Task.CompletedTask);
        _affectationRepoMock.Setup(r => r.AddAsync(It.IsAny<Affectation>())).Returns(Task.CompletedTask);

        await _service.CreerEquipeEtAssignerAsync(request);

        _affectationRepoMock.Verify(
            r => r.AddAsync(It.IsAny<Affectation>()),
            Times.Exactly(3)
        );
    }

    [Fact]
    public async Task CreerEquipeEtAssignerAsync_Should_Assign_Correct_Role_Per_Etudiant()
    {
        var request = new AssignationProjetRequestDto
        {
            IdProjet  = 1,
            NomGroupe = "Equipe Beta",
            Etudiants = new List<EtudiantRoleDto>
            {
                new EtudiantRoleDto { IdEtudiant = 1, IdRole = 2 },
                new EtudiantRoleDto { IdEtudiant = 2, IdRole = 3 }
            }
        };

        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));
        _groupeRepoMock.Setup(r => r.AddAsync(It.IsAny<Groupe>())).Returns(Task.CompletedTask);
        _affectationRepoMock.Setup(r => r.AddAsync(It.IsAny<Affectation>())).Returns(Task.CompletedTask);

        await _service.CreerEquipeEtAssignerAsync(request);

        _affectationRepoMock.Verify(
            r => r.AddAsync(It.Is<Affectation>(a =>
                a.IdEtudiant == 1 && a.IdRole == 2
            )),
            Times.Once
        );

        _affectationRepoMock.Verify(
            r => r.AddAsync(It.Is<Affectation>(a =>
                a.IdEtudiant == 2 && a.IdRole == 3
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task CreerEquipeEtAssignerAsync_Should_Create_Groupe_Before_Affectations()
    {
        // Vérifie l'ordre : Groupe d'abord, puis les affectations
        var callOrder = new List<string>();

        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));

        _groupeRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Groupe>()))
            .Callback<Groupe>(_ => callOrder.Add("groupe"))
            .Returns(Task.CompletedTask);

        _affectationRepoMock
            .Setup(r => r.AddAsync(It.IsAny<Affectation>()))
            .Callback<Affectation>(_ => callOrder.Add("affectation"))
            .Returns(Task.CompletedTask);

        await _service.CreerEquipeEtAssignerAsync(CreateRequest(nbEtudiants: 2));

        Assert.Equal("groupe", callOrder[0]);
        Assert.Equal("affectation", callOrder[1]);
        Assert.Equal("affectation", callOrder[2]);
    }

    // ─── Cas limite : aucun étudiant ──────────────────────────────────────────

    [Fact]
    public async Task CreerEquipeEtAssignerAsync_Should_Create_Groupe_Even_With_No_Etudiants()
    {
        var request = new AssignationProjetRequestDto
        {
            IdProjet  = 1,
            NomGroupe = "Equipe Vide",
            Etudiants = new List<EtudiantRoleDto>() // liste vide
        };

        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));
        _groupeRepoMock.Setup(r => r.AddAsync(It.IsAny<Groupe>())).Returns(Task.CompletedTask);

        var result = await _service.CreerEquipeEtAssignerAsync(request);

        Assert.True(result);
        _groupeRepoMock.Verify(r => r.AddAsync(It.IsAny<Groupe>()), Times.Once);
        _affectationRepoMock.Verify(r => r.AddAsync(It.IsAny<Affectation>()), Times.Never);
    }

    [Fact]
    public async Task CreerEquipeEtAssignerAsync_Should_Create_Groupe_Once_Regardless_Of_Etudiant_Count()
    {
        _projetRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(CreateProjet(1));
        _groupeRepoMock.Setup(r => r.AddAsync(It.IsAny<Groupe>())).Returns(Task.CompletedTask);
        _affectationRepoMock.Setup(r => r.AddAsync(It.IsAny<Affectation>())).Returns(Task.CompletedTask);

        await _service.CreerEquipeEtAssignerAsync(CreateRequest(nbEtudiants: 5));

        // Le groupe est créé une seule fois, peu importe le nombre d'étudiants
        _groupeRepoMock.Verify(r => r.AddAsync(It.IsAny<Groupe>()), Times.Once);
    }
}