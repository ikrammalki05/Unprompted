using Domain.Entities;

namespace Application.Interfaces;

public interface IEnseignantClasseRepository
{
    Task AddAsync(EnseignantClasse enseignantClasse);
}