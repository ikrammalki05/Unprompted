using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Data;
using Application.Interfaces; 
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. BASE DE DONNÉES
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// 2. SÉCURITÉ KEYCLOAK
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.Audience = builder.Configuration["Keycloak:Audience"];
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            NameClaimType = "preferred_username",
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var claimsIdentity = context.Principal?.Identity as System.Security.Claims.ClaimsIdentity;
                if (claimsIdentity == null) return Task.CompletedTask;

                var realmAccess = context.Principal?.FindFirst("realm_access")?.Value;
                if (realmAccess == null) return Task.CompletedTask;

                var parsed = System.Text.Json.JsonDocument.Parse(realmAccess);
                if (parsed.RootElement.TryGetProperty("roles", out var roles))
                {
                    foreach (var role in roles.EnumerateArray())
                    {
                        claimsIdentity.AddClaim(new System.Security.Claims.Claim(
                            System.Security.Claims.ClaimsIdentity.DefaultRoleClaimType,
                            role.GetString() ?? ""
                        ));
                    }
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// 3. CONFIGURATION CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddHttpClient<IKeycloakAdminService, Application.Services.KeycloakAdminService>();

// 4. REPOSITORIES
builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
builder.Services.AddScoped<IEtudiantRepository, EtudiantRepository>();
builder.Services.AddScoped<IEnseignantRepository, EnseignantRepository>();
builder.Services.AddScoped<IClasseRepository, ClasseRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAffectationRepository, AffectationRepository>();
builder.Services.AddScoped<IProjetRepository, ProjetRepository>();
builder.Services.AddScoped<IConfigurationIumRepository, ConfigurationIumRepository>();
builder.Services.AddScoped<IGroupeRepository, GroupeRepository>();
builder.Services.AddScoped<IPromptRepository, PromptRepository>();
builder.Services.AddScoped<IContributionRepository, Infrastructure.Repositories.ContributionRepository>();

builder.Services.AddScoped<IEnseignantClasseRepository, EnseignantClasseRepository>();
builder.Services.AddScoped<IProjetRepository, ProjetRepository>();
builder.Services.AddScoped<IGroupeRepository, GroupeRepository>();

// 5. SERVICES
builder.Services.AddScoped<IEtudiantService, Application.Services.EtudiantService>();
builder.Services.AddScoped<IEnseignantService, Application.Services.EnseignantService>();
builder.Services.AddScoped<IClasseService, Application.Services.ClasseService>();
builder.Services.AddScoped<IAdminService, Application.Services.AdminService>();
builder.Services.AddScoped<IProjetService, Application.Services.ProjetService>();
builder.Services.AddScoped<IConfigurationIumService, Application.Services.ConfigurationIumService>();
builder.Services.AddScoped<IGroupeProjetService, Application.Services.GroupeProjetService>();
builder.Services.AddScoped<IPromptService, Application.Services.PromptService>();
builder.Services.AddScoped<IAnalyticsService, Application.Services.AnalyticsService>();

builder.Services.AddScoped<IGroupeService, Application.Services.GroupeService>();

// 6. SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new() { Title = "Unprompted API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Entrez 'Bearer' [espace] et votre token JWT."
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 7. MIDDLEWARES (L'ordre est crucial !)
app.UseCors("AllowReactApp"); 
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

// 8. MIGRATIONS AUTO
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); 
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Une erreur est survenue lors de la migration de la base de données.");
    }
}

app.Run();