using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class ClasseService : IClasseService
{
    private readonly IClasseRepository _classeRepo;

    public ClasseService(IClasseRepository classeRepo)
    {
        _classeRepo = classeRepo;
    }

    public async Task<IEnumerable<ClasseDto>> GetAllClassesAsync()
    {
        var classes = await _classeRepo.GetAllAsync();
        return classes.Select(MapToDto);
    }

    public async Task<ClasseDto?> GetClasseByIdAsync(int id)
    {
        var classe = await _classeRepo.GetByIdAsync(id);
        if (classe == null) return null;
        return MapToDto(classe);
    }

    public async Task<ClasseDto> CreateClasseAsync(ClasseCreateDto dto)
    {
        var classe = new Classe
        {
            NomClasse = dto.NomClasse,
            AnneeAcademique = dto.AnneeAcademique,
            EffectifMax = dto.EffectifMax,
            DateCreation = DateTime.Now
        };
        await _classeRepo.AddAsync(classe);
        return MapToDto(classe);
    }

    public async Task UpdateClasseAsync(int id, ClasseCreateDto dto)
    {
        var classe = await _classeRepo.GetByIdAsync(id);
        if (classe == null)
            throw new ArgumentException($"Classe avec l'id {id} introuvable.");

        classe.NomClasse = dto.NomClasse;
        classe.AnneeAcademique = dto.AnneeAcademique;
        classe.EffectifMax = dto.EffectifMax;
        await _classeRepo.UpdateAsync(classe);
    }

    public async Task DeleteClasseAsync(int id)
    {
        var classe = await _classeRepo.GetByIdAsync(id);
        if (classe == null)
            throw new ArgumentException($"Classe avec l'id {id} introuvable.");

        await _classeRepo.DeleteAsync(id);
    }

    private static ClasseDto MapToDto(Classe c) => new ClasseDto
    {
        Id = c.IdClasse,
        NomClasse = c.NomClasse,
        AnneeAcademique = c.AnneeAcademique,
        EffectifMax = c.EffectifMax,
        EffectifActuel = 0,
        EnseignantReferent = "Aucun"
    };
}