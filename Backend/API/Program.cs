using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Interfaces; // Permet de trouver IGroupeService, etc.
using Application.Services;   // Permet de trouver GroupeService, etc.

var builder = WebApplication.CreateBuilder(args);

// 1. Configuration de la Base de Données (Ton code)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// 2. Activer les Contrôleurs
builder.Services.AddControllers();

// 3. Activer Swagger (L'interface pour tester l'API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 4. INJECTION DE DÉPENDANCES (Le lien Menu -> Cuisine)
// "À chaque fois qu'un contrôleur demande un IGroupeService, donne-lui un GroupeService"
builder.Services.AddScoped<IGroupeService, GroupeService>();
builder.Services.AddScoped<IAffectationService, AffectationService>();

var app = builder.Build();

// --- Configuration du pipeline HTTP (Ce qui se passe quand une requête arrive) ---

// 5. Activer l'interface web Swagger uniquement en mode développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 6. Sécurité de base
app.UseHttpsRedirection();
app.UseAuthorization();

// 7. Connecter les routes (URLs) aux Contrôleurs
app.MapControllers();

// Lancement de l'application
app.Run();