using Xunit;
using Moq;
using API.Controllers;
using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ClasseControllerTests
{
    private readonly Mock<IClasseService> _classeServiceMock;
    private readonly ClasseController _controller;

    public ClasseControllerTests()
    {
        _classeServiceMock = new Mock<IClasseService>();
        _controller = new ClasseController(_classeServiceMock.Object);
    }

    [Fact]
public async Task GetClasses_Should_Return_Ok_With_List()
{
    // Arrange
    var fakeClasses = new List<ClasseDto>
    {
        new ClasseDto
        {
            Id = 1,
            NomClasse = "Classe A",
            AnneeAcademique = "2023 - 2024",
            EffectifActuel = 25,
            EffectifMax = 40,
            EnseignantReferent = "John Doe"
        }
    };

    _classeServiceMock
        .Setup(s => s.GetAllClassesAsync())
        .ReturnsAsync(fakeClasses);

    // Act
    var result = await _controller.GetClasses();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var data = Assert.IsAssignableFrom<IEnumerable<ClasseDto>>(okResult.Value);

    Assert.Single(data);
}

[Fact]
public async Task GetClasses_Should_Return_Empty_List()
{
    // Arrange
    _classeServiceMock
        .Setup(s => s.GetAllClassesAsync())
        .ReturnsAsync(new List<ClasseDto>());

    // Act
    var result = await _controller.GetClasses();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var data = Assert.IsAssignableFrom<IEnumerable<ClasseDto>>(okResult.Value);

    Assert.Empty(data);
}


}