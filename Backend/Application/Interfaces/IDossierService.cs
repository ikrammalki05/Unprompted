using Application.DTOs;

namespace Application.Interfaces;

public interface IDossierService
{
    Task<DossierDto> CreateAsync(DossierCreateDto dto);
    Task DeleteAsync(int id);
}