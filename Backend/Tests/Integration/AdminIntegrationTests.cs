// using System.Net;
// using System.Net.Http.Json;
// using Application.DTOs;
// using Application.Interfaces;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.Extensions.DependencyInjection;
// using Moq;
// using Xunit;

// namespace Tests.Integration;

// public class AdminIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
// {
//     private readonly HttpClient _client;
//     private readonly Mock<IAdminService> _adminServiceMock = new();

//     public AdminIntegrationTests(WebApplicationFactory<Program> factory)
// {
//     _client = factory.WithWebHostBuilder(builder =>
//     {
//         builder.ConfigureServices(services =>
//         {
//             var descriptor = services.SingleOrDefault(
//                 d => d.ServiceType == typeof(IAdminService));

//             if (descriptor != null)
//                 services.Remove(descriptor);

//             services.AddSingleton(_adminServiceMock.Object);

//             // 🔥 override AUTH
//             services.AddAuthentication("Test")
//                 .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
//                     "Test", options => { });

//             services.AddAuthorization(options =>
//             {
//                 options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder("Test")
//                     .RequireAuthenticatedUser()
//                     .Build();
//             });
//         });
//     }).CreateClient();
// }

//     [Fact]
//     public async Task GetDashboardStats_Should_Return_OK()
//     {
//         _adminServiceMock
//           .Setup(s => s.GetDashboardStatsAsync())
//           .ReturnsAsync(new
//           {
//               TotalEtudiants = 1,
//               TotalEnseignants = 2,
//               TotalClasses = 3
//           });
//     }
// }