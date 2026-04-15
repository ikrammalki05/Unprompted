using Application.DTOs;

namespace Application.Interfaces;

public interface IEtudiantService
{
    Task<IEnumerable<EtudiantDto>> GetAllEtudiantsAsync();
}