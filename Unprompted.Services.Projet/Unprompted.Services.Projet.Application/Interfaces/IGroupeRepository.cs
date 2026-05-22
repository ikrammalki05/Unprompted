using System.Collections.Generic;
using System.Threading.Tasks;
using Unprompted.Services.Projet.Domain.Entities;

namespace Unprompted.Services.Projet.Application.Interfaces;

public interface IGroupeRepository
{
    Task<Groupe?> GetByIdAsync(int id);
    Task<IEnumerable<Groupe>> GetByProjetIdAsync(int projetId);
    Task AddAsync(Groupe groupe);
    void Delete(Groupe groupe);
    Task<bool> SaveChangesAsync();
}