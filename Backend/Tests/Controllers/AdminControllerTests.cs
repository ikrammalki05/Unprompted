using Xunit;
using Moq;
using API.Controllers;
using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace Tests.Controllers;

public class AdminControllerTests
{
    private readonly Mock<IAdminService> _adminServiceMock;
    private readonly AdminController _controller;

    public AdminControllerTests()
    {
        _adminServiceMock = new Mock<IAdminService>();
        _controller = new AdminController(_adminServiceMock.Object);
    }

    [Fact]
    public async Task GetDashboardStats_Should_Return_Ok_With_Data()
    {
        // Arrange
        var fakeStats = new DashboardStatsDto
        {
            TotalEtudiants = 10,
            TotalEnseignants = 5,
            TotalClasses = 3
        };

        _adminServiceMock
            .Setup(s => s.GetDashboardStatsAsync())
            .ReturnsAsync(fakeStats);

        // Act
        var result = await _controller.GetDashboardStats();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var returnedStats = Assert.IsType<DashboardStatsDto>(okResult.Value);

        Assert.Equal(10, returnedStats.TotalEtudiants);
        Assert.Equal(5, returnedStats.TotalEnseignants);
        Assert.Equal(3, returnedStats.TotalClasses);
    }

    [Fact]
    public async Task GetDashboardStats_Should_Return_500_On_Exception()
    {
        // Arrange
        _adminServiceMock
            .Setup(s => s.GetDashboardStatsAsync())
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _controller.GetDashboardStats();

        // Assert
        var objResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objResult.StatusCode);
    }

    [Fact]
    public async Task AssignerEnseignant_Should_Return_Ok()
    {
        // Arrange
        var request = new AffectationEnseignantRequestDto
        {
            IdEnseignant = 1,
            IdClasse = 2
        };

        _adminServiceMock
            .Setup(s => s.AssignerEnseignantAClasseAsync(request))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.AssignerEnseignant(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task AssignerEnseignant_Should_Return_BadRequest_When_Invalid()
    {
        // Arrange
        var request = new AffectationEnseignantRequestDto
        {
            IdEnseignant = 1,
            IdClasse = 2
        };

        _adminServiceMock
            .Setup(s => s.AssignerEnseignantAClasseAsync(request))
            .ThrowsAsync(new ArgumentException("Erreur données"));

        // Act
        var result = await _controller.AssignerEnseignant(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequest.StatusCode);
    }
}