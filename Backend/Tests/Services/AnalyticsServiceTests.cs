using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Services;

public class AnalyticsServiceTests
{
    private readonly Mock<IPromptRepository> _promptRepoMock;
    private readonly Mock<IContributionRepository> _contributionRepoMock;
    private readonly AnalyticsService _service;

    public AnalyticsServiceTests()
    {
        _promptRepoMock       = new Mock<IPromptRepository>();
        _contributionRepoMock = new Mock<IContributionRepository>();

        _service = new AnalyticsService(
            _promptRepoMock.Object,
            _contributionRepoMock.Object
        );
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private Utilisateur CreateUtilisateur(string prenom, string nom) => new Utilisateur
    {
        Prenom = prenom,
        Nom = nom,
        Email = $"{prenom}.{nom}@example.com"
    };

    private Etudiant CreateEtudiant(int id, string prenom = "Alice", string nom = "Martin") => new Etudiant
    {
        IdEtudiant = id,
        IdUtilisateurNavigation = CreateUtilisateur(prenom, nom)
    };

    private Contribution CreateContribution(int id, int idEtudiant, int lignesAjoutees = 10, int lignesSupprimees = 5, Etudiant? nav = null) => new Contribution
    {
        IdContribution = id,
        IdEtudiant = idEtudiant,
        LignesAjoutees = lignesAjoutees,
        LignesSupprimees = lignesSupprimees,
        IdEtudiantNavigation = nav
    };

    private Prompt CreatePrompt(int id, int idEtudiant, int tokensEntree = 100, int tokensSortie = 50, Etudiant? nav = null) => new Prompt
    {
        IdPrompt = id,
        IdEtudiant = idEtudiant,
        NbTokensEntree = tokensEntree,
        NbTokensSortie = tokensSortie,
        IdEtudiantNavigation = nav
    };

    // ─── Cas : aucune donnée ──────────────────────────────────────────────────

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Return_Empty_When_No_Data()
    {
        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>());

        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>());

        var result = await _service.GenererRapportProjetAsync(1);

        Assert.Empty(result);
    }

    // ─── Cas : un étudiant avec commits et prompts ─────────────────────────────

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Return_One_Entry_Per_Etudiant()
    {
        var etudiant = CreateEtudiant(1);

        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, nav: etudiant),
                CreateContribution(2, 1, nav: etudiant)
            });

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1, nav: etudiant)
            });

        var result = await _service.GenererRapportProjetAsync(1);

        Assert.Single(result);
        Assert.Equal(1, result.First().IdEtudiant);
    }

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Aggregate_Commits_Correctly()
    {
        var etudiant = CreateEtudiant(1);

        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, lignesAjoutees: 10, lignesSupprimees: 5, nav: etudiant),
                CreateContribution(2, 1, lignesAjoutees: 20, lignesSupprimees: 10, nav: etudiant)
            });

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>());

        var result = await _service.GenererRapportProjetAsync(1);
        var dto = result.First();

        Assert.Equal(2, dto.NombreCommits);
        Assert.Equal(45, dto.LignesModifiees); // (10+5) + (20+10)
    }

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Aggregate_Tokens_Correctly()
    {
        var etudiant = CreateEtudiant(1);

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1, tokensEntree: 100, tokensSortie: 50, nav: etudiant),
                CreatePrompt(2, 1, tokensEntree: 200, tokensSortie: 100, nav: etudiant)
            });

        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, nav: etudiant)
            });

        var result = await _service.GenererRapportProjetAsync(1);
        var dto = result.First();

        Assert.Equal(2, dto.NombrePrompts);
        Assert.Equal(450, dto.TokensConsommes); // (100+50) + (200+100)
    }

    // ─── Cas : nom de l'étudiant ──────────────────────────────────────────────

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Use_Nom_From_Contribution()
    {
        var etudiant = CreateEtudiant(1, "Alice", "Martin");

        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, nav: etudiant)
            });

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>());

        var result = await _service.GenererRapportProjetAsync(1);

        Assert.Equal("Alice Martin", result.First().NomEtudiant);
    }

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Use_Nom_From_Prompt_When_No_Contribution()
    {
        var etudiant = CreateEtudiant(1, "Bob", "Dupont");

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1, nav: etudiant)
            });

        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>());

        var result = await _service.GenererRapportProjetAsync(1);

        Assert.Equal("Bob Dupont", result.First().NomEtudiant);
    }

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Use_Inconnu_When_No_Navigation()
    {
        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1, nav: null) // pas de navigation
            });

        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>());

        var result = await _service.GenererRapportProjetAsync(1);

        Assert.Equal("Étudiant Inconnu", result.First().NomEtudiant);
    }

    // ─── Algorithme de dépendance ─────────────────────────────────────────────

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Return_Critique_When_Prompts_But_No_Commits()
    {
        var etudiant = CreateEtudiant(1);

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1, tokensEntree: 500, tokensSortie: 500, nav: etudiant)
            });

        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>()); // 0 commits

        var result = await _service.GenererRapportProjetAsync(1);

        Assert.Equal("Critique", result.First().NiveauDependance);
    }

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Return_Critique_When_Ratio_Above_500()
    {
        var etudiant = CreateEtudiant(1);

        // 1 ligne modifiée, 600 tokens => ratio = 600 > 500 => Critique
        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, lignesAjoutees: 1, lignesSupprimees: 0, nav: etudiant)
            });

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1, tokensEntree: 400, tokensSortie: 200, nav: etudiant) // 600 tokens
            });

        var result = await _service.GenererRapportProjetAsync(1);

        Assert.Equal("Critique", result.First().NiveauDependance);
    }

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Return_Modere_When_Ratio_Between_150_And_500()
    {
        var etudiant = CreateEtudiant(1);

        // 10 lignes modifiées, 2000 tokens => ratio = 200 => Modéré
        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, lignesAjoutees: 10, lignesSupprimees: 0, nav: etudiant)
            });

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1, tokensEntree: 1000, tokensSortie: 1000, nav: etudiant) // 2000 tokens
            });

        var result = await _service.GenererRapportProjetAsync(1);

        Assert.Equal("Modéré", result.First().NiveauDependance);
    }

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Return_Autonome_When_Ratio_Below_150()
    {
        var etudiant = CreateEtudiant(1);

        // 100 lignes modifiées, 100 tokens => ratio = 1 => Autonome
        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, lignesAjoutees: 100, lignesSupprimees: 0, nav: etudiant)
            });

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1, tokensEntree: 50, tokensSortie: 50, nav: etudiant) // 100 tokens
            });

        var result = await _service.GenererRapportProjetAsync(1);

        Assert.Equal("Autonome", result.First().NiveauDependance);
    }

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Return_Autonome_When_No_Prompts_And_Has_Commits()
    {
        var etudiant = CreateEtudiant(1);

        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, nav: etudiant)
            });

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>()); // 0 prompts => ratio = 0 => Autonome

        var result = await _service.GenererRapportProjetAsync(1);

        Assert.Equal("Autonome", result.First().NiveauDependance);
    }

    // ─── Tri du rapport ───────────────────────────────────────────────────────

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Order_Critique_First()
    {
        var etudiant1 = CreateEtudiant(1, "Alice", "Martin");  // Autonome
        var etudiant2 = CreateEtudiant(2, "Bob", "Dupont");    // Critique

        // etudiant1 : 100 lignes, 100 tokens => Autonome
        // etudiant2 : 0 commits, des prompts => Critique
        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, lignesAjoutees: 100, lignesSupprimees: 0, nav: etudiant1)
            });

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1, tokensEntree: 50, tokensSortie: 50, nav: etudiant1),
                CreatePrompt(2, 2, tokensEntree: 500, tokensSortie: 500, nav: etudiant2) // Critique
            });

        var result = (await _service.GenererRapportProjetAsync(1)).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("Critique", result[0].NiveauDependance);
        Assert.Equal("Autonome", result[1].NiveauDependance);
    }

    // ─── Multi-étudiants ──────────────────────────────────────────────────────

    [Fact]
    public async Task GenererRapportProjetAsync_Should_Return_One_Entry_Per_Unique_Etudiant()
    {
        var etudiant1 = CreateEtudiant(1, "Alice", "Martin");
        var etudiant2 = CreateEtudiant(2, "Bob", "Dupont");

        _contributionRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Contribution>
            {
                CreateContribution(1, 1, nav: etudiant1),
                CreateContribution(2, 2, nav: etudiant2)
            });

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(new List<Prompt>
            {
                CreatePrompt(1, 1, nav: etudiant1) // étudiant1 apparaît dans les deux
            });

        var result = await _service.GenererRapportProjetAsync(1);

        // 2 étudiants distincts, pas 3 entrées
        Assert.Equal(2, result.Count());
    }
}