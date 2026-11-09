// Application/Interfaces/IEtudiantRepository.cs
namespace Application.Interfaces;
using Domain.Entities;
public interface IEtudiantRepository
{
    Task<IEnumerable<Etudiant>> GetAllAsync();
    Task<Etudiant?> GetByIdAsync(int id);
    Task<int> CountAsync();
    Task AddAsync(Etudiant etudiant);
    Task UpdateAsync(Etudiant etudiant);
    Task DeleteAsync(int id);
}