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
            ValidateIssuer = false, // Désactivé pour gérer le double accès localhost/keycloak
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            NameClaimType = "preferred_username",
            RoleClaimType = System.Security.Claims.ClaimsIdentity.DefaultRoleClaimType
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var claimsIdentity = context.Principal?.Identity as System.Security.Claims.ClaimsIdentity;
                if (claimsIdentity == null) return Task.CompletedTask;

                // Debug: Afficher tous les claims reçus
                foreach (var claim in claimsIdentity.Claims)
                {
                    Console.WriteLine($"CLAIM RECEIVED: {claim.Type} = {claim.Value}");
                }

                // Récupérer les rôles de Keycloak (souvent dans realm_access)
                var realmAccessClaim = claimsIdentity.FindFirst("realm_access");
                if (realmAccessClaim != null)
                {
                    var realmAccess = System.Text.Json.JsonDocument.Parse(realmAccessClaim.Value);
                    if (realmAccess.RootElement.TryGetProperty("roles", out var rolesElement))
                    {
                        foreach (var role in rolesElement.EnumerateArray())
                        {
                            var roleName = role.GetString();
                            if (!string.IsNullOrEmpty(roleName))
                            {
                                // Normalisation : enseignant -> Enseignant
                                var normalizedRole = char.ToUpper(roleName[0]) + roleName.Substring(1).ToLower();
                                claimsIdentity.AddClaim(new System.Security.Claims.Claim(claimsIdentity.RoleClaimType, normalizedRole));
                            }
                        }
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
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:3000",
                "http://localhost:3001"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.AddHttpClient<IKeycloakAdminService,
    Application.Services.KeycloakAdminService>();

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
builder.Services.AddScoped<IContributionRepository,
    Infrastructure.Repositories.ContributionRepository>();

builder.Services.AddScoped<IEnseignantClasseRepository,
    EnseignantClasseRepository>();

builder.Services.AddScoped<IProjetRepository, ProjetRepository>();
builder.Services.AddScoped<IGroupeRepository, GroupeRepository>();

// 5. SERVICES
builder.Services.AddScoped<IEtudiantService,
    Application.Services.EtudiantService>();

builder.Services.AddScoped<IEnseignantService,
    Application.Services.EnseignantService>();

builder.Services.AddScoped<IClasseService,
    Application.Services.ClasseService>();

builder.Services.AddScoped<IAdminService,
    Application.Services.AdminService>();

builder.Services.AddScoped<IProjetService,
    Application.Services.ProjetService>();

builder.Services.AddScoped<IConfigurationIumService,
    Application.Services.ConfigurationIumService>();

builder.Services.AddScoped<IGroupeProjetService,
    Application.Services.GroupeProjetService>();

builder.Services.AddScoped<IPromptService,
    Application.Services.PromptService>();

builder.Services.AddScoped<IAnalyticsService,
    Application.Services.AnalyticsService>();

builder.Services.AddScoped<IEvaluationRepository,
    EvaluationRepository>();

builder.Services.AddScoped<IGroupeService,
    Application.Services.GroupeService>();

// 6. SWAGGER
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new()
        {
            Title = "Unprompted API",
            Version = "v1"
        });

    c.AddSecurityDefinition("Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Entrez 'Bearer' [espace] et votre token JWT."
        });

    c.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference =
                        new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type =
                                Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                new string[] { }
            }
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 7. MIDDLEWARES
app.UseCors("AllowReactApp");
app.UseStaticFiles();

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

        logger.LogError(
            ex,
            "Une erreur est survenue lors de la migration de la base de données."
        );
    }
}

app.Run();