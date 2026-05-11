using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class ClasseService : IClasseService
{
    private readonly IClasseRepository _classeRepo;
    private readonly IEtudiantRepository _etudiantRepo;
    private readonly IEnseignantRepository _enseignantRepo;

    public ClasseService(IClasseRepository classeRepo, IEtudiantRepository etudiantRepo, IEnseignantRepository enseignantRepo)
    {
        _classeRepo = classeRepo;
        _etudiantRepo = etudiantRepo;
        _enseignantRepo = enseignantRepo;
    }

    public async Task<ClasseDto?> GetClasseByIdAsync(int id)
    {
        var classe = await _classeRepo.GetByIdAsync(id);
        if (classe == null) return null;

        return new ClasseDto
        {
            IdClasse = classe.IdClasse,
            NomClasse = classe.NomClasse,
            AnneeAcademique = classe.AnneeAcademique,
            EffectifMax = classe.EffectifMax,
            DateCreation = classe.DateCreation
        };
    }

    public async Task<IEnumerable<ClasseDto>> GetAllClassesAsync()
    {
        var classes = await _classeRepo.GetAllAsync();
        return classes.Select(c => new ClasseDto
        {
            IdClasse = c.IdClasse,
            NomClasse = c.NomClasse,
            AnneeAcademique = c.AnneeAcademique,
            EffectifMax = c.EffectifMax,
            DateCreation = c.DateCreation
        });
    }

    public async Task<ClasseDto> CreateClasseAsync(ClasseCreateDto request)
    {
        var classe = new Classe
        {
            NomClasse = request.NomClasse,
            AnneeAcademique = request.AnneeAcademique,
            EffectifMax = request.EffectifMax,
            DateCreation = DateTime.UtcNow
        };

        await _classeRepo.AddAsync(classe);

        return new ClasseDto
        {
            IdClasse = classe.IdClasse,
            NomClasse = classe.NomClasse,
            AnneeAcademique = classe.AnneeAcademique,
            EffectifMax = classe.EffectifMax,
            DateCreation = classe.DateCreation
        };
    }

    public async Task DeleteClasseAsync(int id)
    {
        await _classeRepo.DeleteAsync(id);
    }

    public async Task AffecterEtudiantAsync(int idClasse, int idEtudiant)
    {
        var etudiant = await _etudiantRepo.GetByIdAsync(idEtudiant);
        if (etudiant == null) throw new Exception("Étudiant non trouvé");

        var classe = await _classeRepo.GetByIdAsync(idClasse);
        if (classe == null) throw new Exception("Classe non trouvée");

        etudiant.IdClasse = idClasse;
        await _etudiantRepo.UpdateAsync(etudiant);
    }

    public async Task AffecterEnseignantAsync(int idClasse, int idEnseignant)
    {
        var enseignant = await _enseignantRepo.GetByIdAsync(idEnseignant);
        if (enseignant == null) throw new Exception("Enseignant non trouvé");

        var classe = await _classeRepo.GetByIdAsync(idClasse);
        if (classe == null) throw new Exception("Classe non trouvée");

        // Utilisation d'une méthode dans le repository pour gérer l'entité de liaison
        await _classeRepo.AddEnseignantToClasseAsync(idClasse, idEnseignant);
    }
}
