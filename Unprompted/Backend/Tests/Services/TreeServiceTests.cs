using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Unitaires.Services;

public class TreeServiceTests
{
    private readonly Mock<IDossierRepository> _dossierRepoMock;
    private readonly Mock<IFichierRepository> _fichierRepoMock;
    private readonly TreeService _service;

    public TreeServiceTests()
    {
        _dossierRepoMock = new Mock<IDossierRepository>();
        _fichierRepoMock = new Mock<IFichierRepository>();

        _service = new TreeService(
            _dossierRepoMock.Object,
            _fichierRepoMock.Object
        );
    }

    [Fact]
    public async Task GetProjectTreeAsync_ShouldReturnEmptyTree_WhenNoFoldersAndFiles()
    {
        // Arrange
        _dossierRepoMock
            .Setup(r => r.GetByProjectIdAsync(1))
            .ReturnsAsync(new List<Dossier>());

        _fichierRepoMock
            .Setup(r => r.GetByProjectIdAsync(1))
            .ReturnsAsync(new List<Fichier>());

        // Act
        var result = await _service.GetProjectTreeAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _dossierRepoMock.Verify(r => r.GetByProjectIdAsync(1), Times.Once);
        _fichierRepoMock.Verify(r => r.GetByProjectIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetProjectTreeAsync_ShouldReturnFoldersAndFilesHierarchy()
    {
        // Arrange
        var dossiers = new List<Dossier>
        {
            new Dossier
            {
                IdDossier = 1,
                Nom = "src",
                DossierParentId = null
            },
            new Dossier
            {
                IdDossier = 2,
                Nom = "components",
                DossierParentId = 1
            }
        };

        var fichiers = new List<Fichier>
        {
            new Fichier
            {
                IdFichier = 10,
                Nom = "Program",
                Extension = ".cs",
                CreatedBy = "Alice",
                IdDossier = null
            },
            new Fichier
            {
                IdFichier = 11,
                Nom = "Button",
                Extension = ".jsx",
                CreatedBy = "Bob",
                IdDossier = 2
            }
        };

        _dossierRepoMock
            .Setup(r => r.GetByProjectIdAsync(1))
            .ReturnsAsync(dossiers);

        _fichierRepoMock
            .Setup(r => r.GetByProjectIdAsync(1))
            .ReturnsAsync(fichiers);

        // Act
        var result = await _service.GetProjectTreeAsync(1);

        // Assert
        Assert.Equal(2, result.Count);

        // dossier racine
        var srcFolder = result.FirstOrDefault(n => n.Name == "src");

        Assert.NotNull(srcFolder);
        Assert.Equal("folder", srcFolder!.Type);

        // sous dossier
        var componentsFolder = srcFolder.Children
            .FirstOrDefault(c => c.Name == "components");

        Assert.NotNull(componentsFolder);
        Assert.Equal("folder", componentsFolder!.Type);

        // fichier dans components
        var buttonFile = componentsFolder.Children
            .FirstOrDefault(c => c.Name == "Button.jsx");

        Assert.NotNull(buttonFile);
        Assert.Equal("file", buttonFile!.Type);

        // fichier racine
        var programFile = result
            .FirstOrDefault(n => n.Name == "Program.cs");

        Assert.NotNull(programFile);
        Assert.Equal("file", programFile!.Type);
    }

    [Fact]
    public async Task GetProjectTreeAsync_ShouldReturnOnlyRootFiles_WhenNoFolders()
    {
        // Arrange
        var fichiers = new List<Fichier>
        {
            new Fichier
            {
                IdFichier = 1,
                Nom = "index",
                Extension = ".html",
                CreatedBy = "Alice",
                IdDossier = null
            },
            new Fichier
            {
                IdFichier = 2,
                Nom = "style",
                Extension = ".css",
                CreatedBy = "Bob",
                IdDossier = null
            }
        };

        _dossierRepoMock
            .Setup(r => r.GetByProjectIdAsync(1))
            .ReturnsAsync(new List<Dossier>());

        _fichierRepoMock
            .Setup(r => r.GetByProjectIdAsync(1))
            .ReturnsAsync(fichiers);

        // Act
        var result = await _service.GetProjectTreeAsync(1);

        // Assert
        Assert.Equal(2, result.Count);

        Assert.All(result, node =>
        {
            Assert.Equal("file", node.Type);
        });

        Assert.Contains(result, n => n.Name == "index.html");
        Assert.Contains(result, n => n.Name == "style.css");
    }

    [Fact]
    public async Task GetProjectTreeAsync_ShouldReturnNestedFoldersCorrectly()
    {
        // Arrange
        var dossiers = new List<Dossier>
        {
            new Dossier
            {
                IdDossier = 1,
                Nom = "backend",
                DossierParentId = null
            },
            new Dossier
            {
                IdDossier = 2,
                Nom = "services",
                DossierParentId = 1
            },
            new Dossier
            {
                IdDossier = 3,
                Nom = "helpers",
                DossierParentId = 2
            }
        };

        _dossierRepoMock
            .Setup(r => r.GetByProjectIdAsync(1))
            .ReturnsAsync(dossiers);

        _fichierRepoMock
            .Setup(r => r.GetByProjectIdAsync(1))
            .ReturnsAsync(new List<Fichier>());

        // Act
        var result = await _service.GetProjectTreeAsync(1);

        // Assert
        var backend = result.First();

        Assert.Equal("backend", backend.Name);

        var services = backend.Children.First();

        Assert.Equal("services", services.Name);

        var helpers = services.Children.First();

        Assert.Equal("helpers", helpers.Name);
        Assert.Equal("folder", helpers.Type);
    }
}