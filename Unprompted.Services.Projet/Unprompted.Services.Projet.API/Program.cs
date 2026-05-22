using Microsoft.EntityFrameworkCore;
using Unprompted.Services.Projet.Infrastructure.Data;
using Unprompted.Services.Projet.Application.Interfaces;
using Unprompted.Services.Projet.Infrastructure.Repositories;
using Unprompted.Services.Projet.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURATION DES SERVICES (DANS LE CONTAINER) ---

builder.Services.AddControllers();

// Enregistrement de ton DbContext isolé pour le microservice Projet
builder.Services.AddDbContext<ProjetDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ProjetConnection")
    ));
// Enregistrement des Repositories
builder.Services.AddScoped<IProjetRepository, ProjetRepository>();
builder.Services.AddScoped<IGroupeRepository, GroupeRepository>();
builder.Services.AddScoped<IAffectationRepository, AffectationRepository>();
builder.Services.AddScoped<IConfigurationIumRepository, ConfigurationIumRepository>();

// Enregistrement des Services Applicatifs
builder.Services.AddScoped<ProjetService>();
// Enregistrement du nouveau Service Applicatif
builder.Services.AddScoped<GroupeService>();
// Enregistrement du Service de Configuration IA
builder.Services.AddScoped<ConfigurationIumService>();

// Configuration de Swagger pour le test des routes
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. CONFIGURATION DU MIDDLEWARE (PIPELINE HTTP) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();