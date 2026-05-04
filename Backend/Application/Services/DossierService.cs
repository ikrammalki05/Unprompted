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
}