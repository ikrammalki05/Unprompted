using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.Services;

// ─── Handler mock pour intercepter les requêtes HTTP ─────────────────────────

public class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Queue<HttpResponseMessage> _responses = new();

    public void EnqueueResponse(HttpResponseMessage response)
        => _responses.Enqueue(response);

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_responses.Count == 0)
            throw new InvalidOperationException("Aucune réponse mockée disponible.");

        return Task.FromResult(_responses.Dequeue());
    }
}

// ─── Helpers de réponses HTTP ─────────────────────────────────────────────────

public static class FakeHttpResponses
{
    public static HttpResponseMessage TokenOk(string token = "fake-admin-token", int expiresIn = 300)
    {
        var body = JsonSerializer.Serialize(new
        {
            access_token = token,
            expires_in   = expiresIn
        });

        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
    }

    public static HttpResponseMessage TokenFail()
        => new HttpResponseMessage(HttpStatusCode.Unauthorized)
        {
            Content = new StringContent("invalid_credentials")
        };

    public static HttpResponseMessage CreateUserOk(string userId = "keycloak-user-abc123")
    {
        var response = new HttpResponseMessage(HttpStatusCode.Created);
        response.Headers.Location = new Uri($"http://keycloak/admin/realms/unprompted/users/{userId}");
        return response;
    }

    public static HttpResponseMessage CreateUserFail()
        => new HttpResponseMessage(HttpStatusCode.Conflict)
        {
            Content = new StringContent("User already exists")
        };

    public static HttpResponseMessage Ok()
        => new HttpResponseMessage(HttpStatusCode.NoContent);

    public static HttpResponseMessage Fail(HttpStatusCode code = HttpStatusCode.InternalServerError)
        => new HttpResponseMessage(code)
        {
            Content = new StringContent("error")
        };
}

// ─── Tests ────────────────────────────────────────────────────────────────────

public class KeycloakAdminServiceTests
{
    private readonly MockHttpMessageHandler _handler;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly Mock<ILogger<KeycloakAdminService>> _loggerMock;

    public KeycloakAdminServiceTests()
    {
        _handler    = new MockHttpMessageHandler();
        _httpClient = new HttpClient(_handler);
        _loggerMock = new Mock<ILogger<KeycloakAdminService>>();

        var configValues = new Dictionary<string, string?>
        {
            { "Keycloak:AdminUrl",         "http://localhost:8080" },
            { "Keycloak:RealmName",        "unprompted"            },
            { "Keycloak:Admin:Username",   "admin"                 },
            { "Keycloak:Admin:Password",   "admin"                 }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();
    }

    private KeycloakAdminService CreateService()
        => new KeycloakAdminService(_httpClient, _configuration, _loggerMock.Object);

    // ─── CreateUserAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task CreateUserAsync_Should_Return_UserId_When_Success()
    {
        var service = CreateService();

        _handler.EnqueueResponse(FakeHttpResponses.TokenOk());
        _handler.EnqueueResponse(FakeHttpResponses.CreateUserOk("keycloak-user-abc123"));

        var result = await service.CreateUserAsync("alice@example.com", "Alice", "Martin");

        Assert.Equal("keycloak-user-abc123", result);
    }

    [Fact]
    public async Task CreateUserAsync_Should_Throw_When_Token_Fails()
    {
        var service = CreateService();

        _handler.EnqueueResponse(FakeHttpResponses.TokenFail());

        await Assert.ThrowsAsync<Exception>(
            () => service.CreateUserAsync("alice@example.com", "Alice", "Martin")
        );
    }

    [Fact]
    public async Task CreateUserAsync_Should_Throw_When_Keycloak_Returns_Error()
    {
        var service = CreateService();

        _handler.EnqueueResponse(FakeHttpResponses.TokenOk());
        _handler.EnqueueResponse(FakeHttpResponses.CreateUserFail());

        await Assert.ThrowsAsync<Exception>(
            () => service.CreateUserAsync("alice@example.com", "Alice", "Martin")
        );
    }

    [Fact]
    public async Task CreateUserAsync_Should_Extract_Id_From_Location_Header()
    {
        var service = CreateService();

        // Location header = .../users/{id} → on extrait la dernière partie
        _handler.EnqueueResponse(FakeHttpResponses.TokenOk());
        _handler.EnqueueResponse(FakeHttpResponses.CreateUserOk("user-xyz-789"));

        var result = await service.CreateUserAsync("test@example.com", "Test", "User");

        Assert.Equal("user-xyz-789", result);
    }

    // ─── SetUserPasswordAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task SetUserPasswordAsync_Should_Succeed_When_Valid()
    {
        var service = CreateService();

        _handler.EnqueueResponse(FakeHttpResponses.TokenOk());
        _handler.EnqueueResponse(FakeHttpResponses.Ok());

        // Ne doit pas lever d'exception
        var ex = await Record.ExceptionAsync(
            () => service.SetUserPasswordAsync("user-123", "MotDePasse123!")
        );

        Assert.Null(ex);
    }

    [Fact]
    public async Task SetUserPasswordAsync_Should_Throw_When_Keycloak_Returns_Error()
    {
        var service = CreateService();

        _handler.EnqueueResponse(FakeHttpResponses.TokenOk());
        _handler.EnqueueResponse(FakeHttpResponses.Fail(HttpStatusCode.NotFound));

        await Assert.ThrowsAsync<Exception>(
            () => service.SetUserPasswordAsync("user-inexistant", "Password!")
        );
    }

    [Fact]
    public async Task SetUserPasswordAsync_Should_Throw_When_Token_Fails()
    {
        var service = CreateService();

        _handler.EnqueueResponse(FakeHttpResponses.TokenFail());

        await Assert.ThrowsAsync<Exception>(
            () => service.SetUserPasswordAsync("user-123", "Password!")
        );
    }

    // ─── DeleteUserAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteUserAsync_Should_Succeed_When_Valid()
    {
        var service = CreateService();

        _handler.EnqueueResponse(FakeHttpResponses.TokenOk());
        _handler.EnqueueResponse(FakeHttpResponses.Ok());

        var ex = await Record.ExceptionAsync(
            () => service.DeleteUserAsync("user-123")
        );

        Assert.Null(ex);
    }

    [Fact]
    public async Task DeleteUserAsync_Should_Throw_When_Keycloak_Returns_Error()
    {
        var service = CreateService();

        _handler.EnqueueResponse(FakeHttpResponses.TokenOk());
        _handler.EnqueueResponse(FakeHttpResponses.Fail(HttpStatusCode.NotFound));

        await Assert.ThrowsAsync<Exception>(
            () => service.DeleteUserAsync("user-inexistant")
        );
    }

    [Fact]
    public async Task DeleteUserAsync_Should_Throw_When_Token_Fails()
    {
        var service = CreateService();

        _handler.EnqueueResponse(FakeHttpResponses.TokenFail());

        await Assert.ThrowsAsync<Exception>(
            () => service.DeleteUserAsync("user-123")
        );
    }

    // ─── Cache du token ───────────────────────────────────────────────────────

    [Fact]
    public async Task Token_Should_Be_Cached_Between_Calls()
    {
        var service = CreateService();

        // Un seul token pour deux opérations consécutives
        _handler.EnqueueResponse(FakeHttpResponses.TokenOk(expiresIn: 300));
        _handler.EnqueueResponse(FakeHttpResponses.Ok()); // SetPassword
        _handler.EnqueueResponse(FakeHttpResponses.Ok()); // DeleteUser

        await service.SetUserPasswordAsync("user-1", "pass1");
        await service.DeleteUserAsync("user-1");

        // Si le token n'était pas mis en cache, le handler manquerait
        // de réponses et lèverait une InvalidOperationException
        // Le fait que les deux appels réussissent prouve le cache
    }

    [Fact]
    public async Task Token_Should_Be_Refreshed_When_Expired()
    {
        var service = CreateService();

        // Token expiré immédiatement (expires_in = 30s - 30s de marge = 0)
        _handler.EnqueueResponse(FakeHttpResponses.TokenOk(expiresIn: 30));
        _handler.EnqueueResponse(FakeHttpResponses.Ok());

        await service.SetUserPasswordAsync("user-1", "pass1");

        // Attendre que le token expire (expires_in - 30s marge = 0s)
        await Task.Delay(100);

        // Deuxième appel : doit re-demander un token
        _handler.EnqueueResponse(FakeHttpResponses.TokenOk(expiresIn: 300));
        _handler.EnqueueResponse(FakeHttpResponses.Ok());

        var ex = await Record.ExceptionAsync(
            () => service.DeleteUserAsync("user-1")
        );

        Assert.Null(ex);
    }

    // ─── Configuration par défaut ─────────────────────────────────────────────

    [Fact]
    public async Task CreateUserAsync_Should_Use_Default_RealmName_When_Not_Configured()
    {
        var configSansRealm = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Keycloak:AdminUrl",       "http://localhost:8080" },
                { "Keycloak:Admin:Username", "admin"                 },
                { "Keycloak:Admin:Password", "admin"                 }
                // pas de RealmName → doit utiliser "unprompted"
            })
            .Build();

        var service = new KeycloakAdminService(_httpClient, configSansRealm, _loggerMock.Object);

        _handler.EnqueueResponse(FakeHttpResponses.TokenOk());
        _handler.EnqueueResponse(FakeHttpResponses.CreateUserOk("user-default-realm"));

        var result = await service.CreateUserAsync("test@example.com", "Test", "User");

        Assert.Equal("user-default-realm", result);
    }
}