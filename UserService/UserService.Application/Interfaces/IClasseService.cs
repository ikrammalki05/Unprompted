using Application.DTOs;

namespace Application.Interfaces;

public interface IClasseService
{
    Task<ClasseDto?> GetClasseByIdAsync(int id);
    Task<IEnumerable<ClasseDto>> GetAllClassesAsync();
    Task<ClasseDto> CreateClasseAsync(ClasseCreateDto request);
    Task DeleteClasseAsync(int id);
    Task AffecterEtudiantAsync(int idClasse, int idEtudiant);
    Task AffecterEnseignantAsync(int idClasse, int idEnseignant);
}
