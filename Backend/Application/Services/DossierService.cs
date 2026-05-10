using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class DossierService : IDossierService
{
    private readonly IDossierRepository _dossierRepo;
    private readonly ILogger<DossierService> _logger;

    public DossierService(IDossierRepository dossierRepo, ILogger<DossierService> logger)
    {
        _dossierRepo = dossierRepo;
        _logger = logger;
    }

    public async Task<DossierDto> CreateAsync(DossierCreateDto dto)
    {
        var exists = await _dossierRepo.ExistsAsync(dto.Nom, dto.DossierParentId, dto.IdProjet);

        if (exists)
            throw new Exception("Un dossier avec le même nom existe déjà.");

        var dossier = new Dossier
        {
            Nom = dto.Nom,
            IdProjet = dto.IdProjet,
            DossierParentId = dto.DossierParentId
        };

        await _dossierRepo.AddAsync(dossier);

        return new DossierDto
        {
            Id = dossier.IdDossier,
            Nom = dossier.Nom,
            DossierParentId = dossier.DossierParentId
        };
    }

    public async Task DeleteAsync(int id)
    {
        await _dossierRepo.DeleteAsync(id);
    }

    public async Task<DossierDto?> GetByIdAsync(int id)
    {
        var dossier = await _dossierRepo.GetByIdAsync(id);
        if (dossier == null) return null;

        return new DossierDto
        {
            Id = dossier.IdDossier,
            Nom = dossier.Nom,
            IdProjet = dossier.IdProjet,
            DossierParentId = dossier.DossierParentId,
            CreatedAt = dossier.CreatedAt
        };
    }

    public async Task<IEnumerable<DossierDto>> GetByProjetIdAsync(int projetId)
    {
        var dossiers = await _dossierRepo.GetByProjectIdAsync(projetId);

        return dossiers.Select(d => new DossierDto
        {
            Id = d.IdDossier,
            Nom = d.Nom,
            IdProjet = d.IdProjet,
            DossierParentId = d.DossierParentId,
            CreatedAt = d.CreatedAt
        });
    }

    public async Task<DossierDto> RenameAsync(int id, DossierRenameDto dto)
    {
        var dossier = await _dossierRepo.GetByIdAsync(id)
            ?? throw new Exception("Dossier introuvable.");

        var exists = await _dossierRepo.ExistsAsync(dto.Nom, dossier.DossierParentId, dossier.IdProjet);
        if (exists)
            throw new Exception("Un dossier avec ce nom existe déjà.");

        dossier.Nom = dto.Nom;
        await _dossierRepo.UpdateAsync(dossier);

        return new DossierDto
        {
            Id = dossier.IdDossier,
            Nom = dossier.Nom,
            IdProjet = dossier.IdProjet,
            DossierParentId = dossier.DossierParentId,
            CreatedAt = dossier.CreatedAt
        };
    }
}