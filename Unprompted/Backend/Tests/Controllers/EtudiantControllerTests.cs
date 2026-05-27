using API.Controllers;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.Controllers;

public class EtudiantControllerTests
{
    private readonly Mock<IEtudiantService> _serviceMock;
    private readonly EtudiantController _controller;

    public EtudiantControllerTests()
    {
        _serviceMock = new Mock<IEtudiantService>();
        _controller = new EtudiantController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetEtudiants_Should_Return_Ok_With_List()
    {
        // Arrange
        var fakeData = new List<EtudiantDto>
        {
            new EtudiantDto { Id = 1, NomComplet = "Test User", Email = "test@test.com" }
        };

        _serviceMock
            .Setup(s => s.GetAllEtudiantsAsync())
            .ReturnsAsync(fakeData);

        // Act
        var result = await _controller.GetEtudiants();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<EtudiantDto>>(okResult.Value);

        Assert.Single(returnValue);
    }

    [Fact]
    public async Task CreateEtudiant_Should_Return_CreatedAtAction_When_Success()
    {
        // Arrange
        var request = new EtudiantCreateDto
        {
            Nom = "Test",
            Prenom = "User",
            Email = "test@test.com",
            CodeApogee = "A123"
        };

        var response = new EtudiantDto
        {
            Id = 1,
            NomComplet = "Test User",
            Email = "test@test.com",
            CodeApogee = "A123"
        };

        _serviceMock
            .Setup(s => s.CreateEtudiantAsync(request))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.CreateEtudiant(request);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);

        Assert.Equal(nameof(_controller.GetEtudiants), createdResult.ActionName);

        var value = Assert.IsType<EtudiantDto>(createdResult.Value);
        Assert.Equal(1, value.Id);
    }

    [Fact]
    public async Task CreateEtudiant_Should_Return_BadRequest_When_Exception()
    {
        // Arrange
        var request = new EtudiantCreateDto
        {
            Nom = "Test",
            Prenom = "User",
            Email = "test@test.com",
            CodeApogee = "A123"
        };

        _serviceMock
            .Setup(s => s.CreateEtudiantAsync(request))
            .ThrowsAsync(new Exception("Email existe déjà"));

        // Act
        var result = await _controller.CreateEtudiant(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequest.Value);
    }
}