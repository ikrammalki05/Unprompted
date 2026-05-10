using Domain.Entities;

namespace Application.Interfaces;

public interface IPromptRepository
{
    Task<Prompt> AddAsync(Prompt prompt);
    Task<IEnumerable<Prompt>> GetByProjetIdAsync(int idProjet);
    Task<IEnumerable<Prompt>> GetByEtudiantIdAsync(int idEtudiant);
}