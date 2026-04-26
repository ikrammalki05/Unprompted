using Domain.Entities;

namespace Application.Interfaces;

public interface IGroupeRepository
{
    Task<Groupe> AddAsync(Groupe groupe);
}