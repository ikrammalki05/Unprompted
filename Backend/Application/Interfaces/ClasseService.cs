using Application.DTOs;

namespace Application.Interfaces;

public interface IClasseService
{
    Task<IEnumerable<ClasseDto>> GetAllClassesAsync();
}