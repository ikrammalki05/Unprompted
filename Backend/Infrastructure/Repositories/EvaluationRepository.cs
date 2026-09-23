using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EvaluationRepository : IEvaluationRepository
{
    private readonly AppDbContext _context;

    public EvaluationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Evaluation>> GetAllAsync()
    {
        return await _context.Evaluations
            .Include(e => e.IdEtudiantNavigation)
            .Include(e => e.IdProjetNavigation)
            .ToListAsync();
    }

    public async Task<IEnumerable<Evaluation>> GetByEtudiantIdAsync(int idEtudiant)
    {
        return await _context.Evaluations
            .Include(e => e.IdProjetNavigation)
            .Include(e => e.IdEnseignantNavigation)
                .ThenInclude(ens => ens!.IdUtilisateurNavigation) // ! pour éviter l'avertissement
            .Where(e => e.IdEtudiant == idEtudiant)
            .OrderByDescending(e => e.DateEvaluation)
            .ToListAsync();
    }

    public async Task<Evaluation> AddAsync(Evaluation evaluation)
    {
        _context.Evaluations.Add(evaluation);
        await _context.SaveChangesAsync();
        return evaluation;
    }
}