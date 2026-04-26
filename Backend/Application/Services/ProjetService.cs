using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class ProjetService : IProjetService
{
    private readonly IProjetRepository _projetRepo;

    public ProjetService(IProjetRepository projetRepo)
    {
        _projetRepo = projetRepo;
    }

    public async Task<IEnumerable<ProjetDto>> GetAllProjetsAsync()
    {
        var projets = await _projetRepo.GetAllAsync();
        return projets.Select(MapToDto);
    }

    public async Task<IEnumerable<ProjetDto>> GetProjetsByEnseignantAsync(int idEnseignant)
    {
        var projets = await _projetRepo.GetByEnseignantIdAsync(idEnseignant);
        return projets.Select(MapToDto);
    }

    public async Task<ProjetDto> CreateProjetAsync(ProjetCreateDto request)
    {
        var nouveauProjet = new Projet
        {
            Titre = request.Titre,
            Description = request.Description,
            DateDebut = request.DateDebut, // Plus d'erreur de conversion !
            DateFin = request.DateFin,
            Statut = "Nouveau", 
            IdEnseignant = request.IdEnseignant
        };

        await _projetRepo.AddAsync(nouveauProjet);

        var projetCree = await _projetRepo.GetByIdAsync(nouveauProjet.IdProjet);
        return MapToDto(projetCree ?? nouveauProjet);
    }

    private ProjetDto MapToDto(Projet p)
    {
        var nomEnseignant = p.IdEnseignantNavigation?.IdUtilisateurNavigation != null 
            ? $"{p.IdEnseignantNavigation.IdUtilisateurNavigation.Prenom} {p.IdEnseignantNavigation.IdUtilisateurNavigation.Nom}"
            : "Inconnu";

        return new ProjetDto
        {
            Id = p.IdProjet,
            Titre = p.Titre ?? string.Empty, // Corrige le warning CS8601
            Description = p.Description ?? string.Empty,
            DateDebut = p.DateDebut,
            DateFin = p.DateFin,
            Statut = p.Statut ?? "Non défini",
            NomEnseignant = nomEnseignant
        };
    }
}