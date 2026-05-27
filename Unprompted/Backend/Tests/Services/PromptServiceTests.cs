using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Unitaires.Services;

public class PromptServiceTests
{
    private readonly Mock<IPromptRepository> _promptRepoMock;
    private readonly PromptService _service;

    public PromptServiceTests()
    {
        _promptRepoMock = new Mock<IPromptRepository>();
        _service = new PromptService(_promptRepoMock.Object);
    }

    [Fact]
    public async Task LogInteractionAsync_ShouldCreatePromptAndReturnTrue()
    {
        // Arrange
        var request = new PromptLogRequestDto
        {
            IdEtudiant = 1,
            IdProjet = 2,
            ContenuPrompt = "Explique LINQ",
            NbTokensEntree = 100,
            NbTokensSortie = 200,
            ContenuReponse = "LINQ est une technologie...",
            ModeleIa = "GPT-4"
        };

        // Act
        var result = await _service.LogInteractionAsync(request);

        // Assert
        Assert.True(result);

        _promptRepoMock.Verify(r => r.AddAsync(It.Is<Prompt>(p =>
            p.IdEtudiant == request.IdEtudiant &&
            p.IdProjet == request.IdProjet &&
            p.Contenu == request.ContenuPrompt &&
            p.NbTokensEntree == request.NbTokensEntree &&
            p.NbTokensSortie == request.NbTokensSortie &&
            p.ReponseIa.Count == 1 &&
            p.ReponseIa.First().ContenuReponse == request.ContenuReponse &&
            p.ReponseIa.First().ModeleIa == request.ModeleIa
        )), Times.Once);
    }

    [Fact]
    public async Task GetHistoriqueProjetAsync_ShouldReturnMappedPromptDtos()
    {
        // Arrange
        var prompts = new List<Prompt>
        {
            new Prompt
            {
                IdPrompt = 1,
                Contenu = "Question IA",
                DatePrompt = DateTime.UtcNow,
                NbTokensEntree = 50,
                NbTokensSortie = 120,

                IdEtudiantNavigation = new Etudiant
                {
                    IdUtilisateurNavigation = new Utilisateur
                    {
                        Prenom = "Ahmed",
                        Nom = "Benali"
                    }
                },

                ReponseIa = new List<ReponseIum>
                {
                    new ReponseIum
                    {
                        IdReponse = 10,
                        ContenuReponse = "Réponse IA",
                        DateReponse = DateTime.UtcNow,
                        ModeleIa = "GPT-4"
                    }
                }
            }
        };

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(prompts);

        // Act
        var result = await _service.GetHistoriqueProjetAsync(1);

        // Assert
        var historique = result.ToList();

        Assert.Single(historique);

        var promptDto = historique.First();

        Assert.Equal(1, promptDto.IdPrompt);
        Assert.Equal("Question IA", promptDto.Contenu);
        Assert.Equal(50, promptDto.NbTokensEntree);
        Assert.Equal(120, promptDto.NbTokensSortie);

        Assert.Equal("Ahmed Benali", promptDto.NomEtudiant);

        Assert.Single(promptDto.Reponses);

        var reponse = promptDto.Reponses.First();

        Assert.Equal(10, reponse.IdReponse);
        Assert.Equal("Réponse IA", reponse.ContenuReponse);
        Assert.Equal("GPT-4", reponse.ModeleIa);

        _promptRepoMock.Verify(r => r.GetByProjetIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetHistoriqueProjetAsync_ShouldReturnInconnu_WhenStudentIsNull()
    {
        // Arrange
        var prompts = new List<Prompt>
        {
            new Prompt
            {
                IdPrompt = 1,
                Contenu = "Test",
                ReponseIa = new List<ReponseIum>()
            }
        };

        _promptRepoMock
            .Setup(r => r.GetByProjetIdAsync(1))
            .ReturnsAsync(prompts);

        // Act
        var result = await _service.GetHistoriqueProjetAsync(1);

        // Assert
        var promptDto = result.First();

        Assert.Equal("Inconnu", promptDto.NomEtudiant);
    }
}