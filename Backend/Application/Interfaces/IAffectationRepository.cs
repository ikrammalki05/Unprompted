using Domain.Entities;

namespace Application.Interfaces;

public interface IAffectationRepository
{
    Task AddAsync(Affectation affectation);
}