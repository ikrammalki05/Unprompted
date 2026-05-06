using Domain.Entities;

namespace Application.Interfaces;

public interface IEvaluationRepository
{
    Task<IEnumerable<Evaluation>> GetByEtudiantIdAsync(int idEtudiant);
}