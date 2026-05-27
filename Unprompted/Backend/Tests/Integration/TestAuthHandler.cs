// using System.Security.Claims;
// using System.Text.Encodings.Web;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.Extensions.Options;

// namespace Tests.Integration;
// public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
// {
//     public TestAuthHandler(
//         IOptionsMonitor<AuthenticationSchemeOptions> options,
//         UrlEncoder encoder,
//         ISystemClock clock)
//         : base(options, logger, encoder, clock)
//     {
//     }

//     protected override Task<AuthenticateResult> HandleAuthenticateAsync()
//     {
//         var claims = new[]
//         {
//             new Claim(ClaimTypes.Name, "TestUser"),
//             new Claim(ClaimTypes.Role, "Admin") // IMPORTANT
//         };

//         var identity = new ClaimsIdentity(claims, "Test");
//         var principal = new ClaimsPrincipal(identity);
//         var ticket = new AuthenticationTicket(principal, "Test");

//         return Task.FromResult(AuthenticateResult.Success(ticket));
//     }
// }