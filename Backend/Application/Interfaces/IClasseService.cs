using Application.DTOs;

namespace Application.Interfaces;

public interface IClasseService
{
    Task<IEnumerable<ClasseDto>> GetAllClassesAsync();
    Task<ClasseDto?> GetClasseByIdAsync(int id);
    Task<ClasseDto> CreateClasseAsync(ClasseCreateDto dto);
    Task UpdateClasseAsync(int id, ClasseCreateDto dto);
    Task DeleteClasseAsync(int id);
}