using Domain.Entities;

namespace Application.Interfaces;

public interface IEvaluationRepository
{
    Task<IEnumerable<Evaluation>> GetAllAsync();
    Task<IEnumerable<Evaluation>> GetByEtudiantIdAsync(int idEtudiant);
    Task<Evaluation> AddAsync(Evaluation evaluation);
}