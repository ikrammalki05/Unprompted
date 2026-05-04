using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class FichierService : IFichierService
{
    private readonly IFichierRepository _fichierRepo;
    private readonly IFichierVersionRepository _versionRepo;
    private readonly ILogger<FichierService> _logger;

    public FichierService(IFichierRepository fichierRepo, IFichierVersionRepository versionRepo, ILogger<FichierService> logger)
    {
        _fichierRepo = fichierRepo;
        _versionRepo = versionRepo;
        _logger = logger;

    }

    public async Task<FichierDto> CreateAsync(FichierCreateDto dto, string userId)
    {
        var exists = await _fichierRepo.ExistsAsync(dto.Nom, dto.IdDossier, dto.IdProjet);

        if (exists)
            throw new Exception("Un fichier avec le même nom existe déjà.");

        var fichier = new Fichier
        {
            Nom = dto.Nom,
            Extension = dto.Extension,
            IdDossier = dto.IdDossier,
            IdProjet = dto.IdProjet,
            Contenu = dto.Contenu,
            Size = dto.Size,
            CreatedBy = userId
        };

        await _fichierRepo.AddAsync(fichier);

        return new FichierDto
        {
            Id = fichier.IdFichier,
            Nom = fichier.Nom,
            Extension = fichier.Extension,
            Contenu = fichier.Contenu,
            Version = fichier.Version,
            DerniereModification = fichier.DerniereModification
        };
    }

    public async Task<FichierDto?> GetByIdAsync(int id)
    {
        var fichier = await _fichierRepo.GetByIdAsync(id);

        if (fichier == null) return null;

        return new FichierDto
        {
            Id = fichier.IdFichier,
            Nom = fichier.Nom,
            Extension = fichier.Extension,
            Contenu = fichier.Contenu,
            Version = fichier.Version,
            DerniereModification = fichier.DerniereModification
        };
    }

    public async Task UpdateAsync(int id, FichierUpdateDto dto)
    {
        var fichier = await _fichierRepo.GetByIdAsync(id);

        if (fichier == null)
            throw new ArgumentException("Fichier introuvable");

        fichier.Contenu = dto.Contenu;
        fichier.Size = dto.Contenu?.Length ?? 0;
        fichier.Version += 1;
        fichier.DerniereModification = DateTime.UtcNow;

        await _fichierRepo.UpdateAsync(fichier);
    }

    public async Task DeleteAsync(int id)
    {
        await _fichierRepo.DeleteAsync(id);
    }

    public async Task AutosaveAsync(int fichierId, string contenu, string userId)
    {
        var fichier = await _fichierRepo.GetByIdAsync(fichierId);

        if (fichier == null)
            throw new Exception("Fichier introuvable");

        // éviter sauvegarde inutile
        if (fichier.Contenu == contenu)
            return;

        fichier.Contenu = contenu;
        fichier.Size = contenu.Length;
        fichier.Version += 1;
        fichier.DerniereModification = DateTime.UtcNow;

        await _fichierRepo.UpdateAsync(fichier);

        // historique
        var version = new FichierVersion
        {
            IdFichier = fichier.IdFichier,
            Contenu = contenu,
            Version = fichier.Version,
            CreatedBy = userId
        };

        await _versionRepo.AddAsync(version);
    }
}
