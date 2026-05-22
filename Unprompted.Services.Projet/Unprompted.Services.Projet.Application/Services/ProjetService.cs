using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Application.DTOs;
using Unprompted.Services.Projet.Application.Interfaces;
using Unprompted.Services.Projet.Domain.Entities;

namespace Unprompted.Services.Projet.Application.Services;

public class ProjetService
{
    private readonly IProjetRepository _projetRepository;

    public ProjetService(IProjetRepository projetRepository)
    {
        _projetRepository = projetRepository;
    }

    public async Task<ProjetDto?> GetProjetByIdAsync(int id)
    {
        var projet = await _projetRepository.GetByIdAsync(id);
        if (projet == null) return null;

        return new ProjetDto
        {
            IdProjet = projet.IdProjet,
            Titre = projet.Titre,
            Description = projet.Description,
            DateDebut = projet.DateDebut,
            DateFin = projet.DateFin,
            Statut = projet.Statut,
            CahierDesChargesPath = projet.CahierDesChargesPath,
            IdEnseignant = projet.IdEnseignant
        };
    }

    public async Task<IEnumerable<ProjetDto>> GetProjetsByEnseignantAsync(int enseignantId)
    {
        var projets = await _projetRepository.GetByEnseignantIdAsync(enseignantId);
        return projets.Select(p => new ProjetDto
        {
            IdProjet = p.IdProjet,
            Titre = p.Titre,
            Description = p.Description,
            DateDebut = p.DateDebut,
            DateFin = p.DateFin,
            Statut = p.Statut,
            CahierDesChargesPath = p.CahierDesChargesPath,
            IdEnseignant = p.IdEnseignant
        });
    }

    public async Task<ProjetDto> CreateProjetAsync(ProjetCreateDto dto)
    {
        var projet = new Domain.Entities.Projet
        {
            Titre = dto.Titre,
            Description = dto.Description,
            DateDebut = dto.DateDebut,
            DateFin = dto.DateFin,
            Statut = dto.Statut ?? "En Cours",
            IdEnseignant = dto.IdEnseignant
        };

        await _projetRepository.AddAsync(projet);
        await _projetRepository.SaveChangesAsync();

        return new ProjetDto
        {
            IdProjet = projet.IdProjet,
            Titre = projet.Titre,
            Description = projet.Description,
            DateDebut = projet.DateDebut,
            DateFin = projet.DateFin,
            Statut = projet.Statut,
            IdEnseignant = projet.IdEnseignant
        };
    }
}