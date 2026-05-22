using System;

namespace Unprompted.Services.Projet.Application.DTOs;

public class ProjetCreateDto
{
    public string Titre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public string? Statut { get; set; }
    public int IdEnseignant { get; set; }
}