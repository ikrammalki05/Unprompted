using Xunit;
using Moq;
using API.Controllers;
using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EnseignantControllerTests
{
    private readonly Mock<IEnseignantService> _serviceMock;
    private readonly EnseignantController _controller;

    public EnseignantControllerTests()
    {
        _serviceMock = new Mock<IEnseignantService>();
        _controller = new EnseignantController(_serviceMock.Object);
    }

    [Fact]
public async Task GetEnseignants_Should_Return_Ok_With_List()
{
    // Arrange
    var fakeData = new List<EnseignantDto>
    {
        new EnseignantDto
        {
            Id = 1,
            NomComplet = "John Doe",
            Email = "john@test.com",
            Specialite = "Math",
            Statut = "Actif",
            ClassesAssignees = new List<string> { "Classe A" }
        }
    };

    _serviceMock
        .Setup(s => s.GetAllEnseignantsAsync())
        .ReturnsAsync(fakeData);

    // Act
    var result = await _controller.GetEnseignants();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var data = Assert.IsAssignableFrom<IEnumerable<EnseignantDto>>(okResult.Value);

    Assert.Single(data);
}

[Fact]
public async Task GetEnseignants_Should_Return_Empty_List()
{
    // Arrange
    _serviceMock
        .Setup(s => s.GetAllEnseignantsAsync())
        .ReturnsAsync(new List<EnseignantDto>());

    // Act
    var result = await _controller.GetEnseignants();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var data = Assert.IsAssignableFrom<IEnumerable<EnseignantDto>>(okResult.Value);

    Assert.Empty(data);
}

[Fact]
public async Task CreateEnseignant_Should_Return_Created()
{
    // Arrange
    var request = new EnseignantCreateDto
    {
        Nom = "Doe",
        Prenom = "John",
        Email = "john@test.com",
        Specialite = "Math"
    };

    var response = new EnseignantDto
    {
        Id = 1,
        NomComplet = "John Doe",
        Email = "john@test.com",
        Specialite = "Math",
        Statut = "Actif",
        ClassesAssignees = new List<string>()
    };

    _serviceMock
        .Setup(s => s.CreateEnseignantAsync(request))
        .ReturnsAsync(response);

    // Act
    var result = await _controller.CreateEnseignant(request);

    // Assert
    var created = Assert.IsType<CreatedAtActionResult>(result);
    var data = Assert.IsType<EnseignantDto>(created.Value);

    Assert.Equal(1, data.Id);
}

[Fact]
public async Task CreateEnseignant_Should_Return_BadRequest_On_Error()
{
    // Arrange
    var request = new EnseignantCreateDto();

    _serviceMock
        .Setup(s => s.CreateEnseignantAsync(request))
        .ThrowsAsync(new Exception("Email déjà existant"));

    // Act
    var result = await _controller.CreateEnseignant(request);

    // Assert
    var badRequest = Assert.IsType<BadRequestObjectResult>(result);
    Assert.Equal(400, badRequest.StatusCode);
}
}