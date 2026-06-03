using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace Tests.Services;

public class ExecutionCodeServiceTests
{
    private readonly Mock<IExecutionCodeRepository> _repoMock;
    private readonly ExecutionCodeService _service;

    public ExecutionCodeServiceTests()
    {
        _repoMock = new Mock<IExecutionCodeRepository>();
        _service  = new ExecutionCodeService(_repoMock.Object);
    }

    // ─── Helpers ───────────────────────────────────────────────────────────────

    private ExecutionCode CreateExecution(
        Guid? id      = null,
        string statut = "running",
        string sortie = "") => new ExecutionCode
    {
        IdExecution       = id ?? Guid.NewGuid(),
        Langage           = "python",
        IdConteneurDocker = "container-abc123",
        Statut            = statut,
        Sortie            = sortie,
        IdUtilisateur     = "user-test-id"
    };

    // ─── GetExecutionAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task GetExecutionAsync_Should_Return_Null_When_Not_Found()
    {
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((ExecutionCode?)null);

        var result = await _service.GetExecutionAsync(id);

        Assert.Null(result);
    }

   /* [Fact]
     public async Task GetExecutionAsync_Should_Return_Dto_With_Correct_Id_And_Statut()
    {
        var id        = Guid.NewGuid();
        var execution = CreateExecution(id, statut: "completed", sortie: "Hello World");

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(execution);

        var result = await _service.GetExecutionAsync(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.IdExecution);
    } */

    // ─── ArreterExecutionAsync ────────────────────────────────────────────────

    [Fact]
    public async Task ArreterExecutionAsync_Should_Throw_When_Not_Found()
    {
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((ExecutionCode?)null);

        await Assert.ThrowsAsync<Exception>(
            () => _service.ArreterExecutionAsync(id)
        );
    }

    [Fact]
    public async Task ArreterExecutionAsync_Should_Not_Call_UpdateAsync_When_Not_Found()
    {
        var id = Guid.NewGuid();

        _repoMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync((ExecutionCode?)null);

        try { await _service.ArreterExecutionAsync(id); } catch { }

        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<ExecutionCode>()), Times.Never);
    }

    // ─── GetDockerCommand (via méthode privée testée indirectement) ────────────

    [Theory]
    [InlineData("python",     "python:3.9")]
    [InlineData("javascript", "node:18")]
    [InlineData("csharp",     "dotnet")]
    [InlineData("java",       "openjdk:17")]
    [InlineData("cpp",        "gcc:latest")]
    public void GetDockerCommand_Should_Contain_Correct_Image(string langage, string expectedImage)
    {
        // On teste GetDockerCommand via réflexion car c'est une méthode privée
        var method = typeof(ExecutionCodeService)
            .GetMethod("GetDockerCommand",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

        var result = (string)method!.Invoke(_service, new object[] { langage, "temp/test" })!;

        Assert.Contains(expectedImage, result);
    }

    [Fact]
    public void GetDockerCommand_Should_Return_Empty_For_Unknown_Language()
    {
        var method = typeof(ExecutionCodeService)
            .GetMethod("GetDockerCommand",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

        var result = (string)method!.Invoke(_service, new object[] { "ruby", "temp/test" })!;

        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("python",     "main.py")]
    [InlineData("javascript", "main.js")]
    [InlineData("java",       "Main")]
    [InlineData("cpp",        "*.cpp")]
    public void GetDockerCommand_Should_Contain_Correct_Entrypoint(string langage, string expectedEntrypoint)
    {
        var method = typeof(ExecutionCodeService)
            .GetMethod("GetDockerCommand",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

        var result = (string)method!.Invoke(_service, new object[] { langage, "temp/test" })!;

        Assert.Contains(expectedEntrypoint, result);
    }

    [Theory]
    [InlineData("python")]
    [InlineData("javascript")]
    [InlineData("java")]
    [InlineData("cpp")]
    public void GetDockerCommand_Should_Contain_Network_None_For_Sandboxed_Languages(string langage)
    {
        var method = typeof(ExecutionCodeService)
            .GetMethod("GetDockerCommand",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

        var result = (string)method!.Invoke(_service, new object[] { langage, "temp/test" })!;

        Assert.Contains("--network none", result);
    }

    [Theory]
    [InlineData("python",     "--memory=100m")]
    [InlineData("javascript", "--memory=100m")]
    [InlineData("csharp",     "--memory=200m")]
    [InlineData("java",       "--memory=100m")]
    [InlineData("cpp",        "--memory=100m")]
    public void GetDockerCommand_Should_Contain_Memory_Limit(string langage, string expectedMemory)
    {
        var method = typeof(ExecutionCodeService)
            .GetMethod("GetDockerCommand",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

        var result = (string)method!.Invoke(_service, new object[] { langage, "temp/test" })!;

        Assert.Contains(expectedMemory, result);
    }

    [Fact]
    public void GetDockerCommand_Should_Be_Case_Insensitive()
    {
        var method = typeof(ExecutionCodeService)
            .GetMethod("GetDockerCommand",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

        var lower = (string)method!.Invoke(_service, new object[] { "python", "temp/test" })!;
        var upper = (string)method!.Invoke(_service, new object[] { "PYTHON", "temp/test" })!;

        Assert.Equal(lower, upper);
    }

    [Fact]
    public void GetDockerCommand_Should_Include_Folder_Path()
    {
        var method = typeof(ExecutionCodeService)
            .GetMethod("GetDockerCommand",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

        var result = (string)method!.Invoke(_service, new object[] { "python", "temp/myproject" })!;

        Assert.Contains("myproject", result);
    }

    [Fact]
    public void GetDockerCommand_Should_Run_Detached()
    {
        var method = typeof(ExecutionCodeService)
            .GetMethod("GetDockerCommand",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

        var result = (string)method!.Invoke(_service, new object[] { "python", "temp/test" })!;

        // docker run -d = mode détaché, obligatoire pour récupérer le containerId
        Assert.Contains("docker run -d", result);
    }
}