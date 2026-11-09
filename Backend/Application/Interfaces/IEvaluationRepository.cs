using Domain.Entities;

namespace Application.Interfaces;

public interface IEvaluationRepository
{
    Task<IEnumerable<Evaluation>> GetAllAsync();
    Task<Evaluation> AddAsync(Evaluation evaluation);
    Task<IEnumerable<Evaluation>> GetByEtudiantIdAsync(int idEtudiant);
}
