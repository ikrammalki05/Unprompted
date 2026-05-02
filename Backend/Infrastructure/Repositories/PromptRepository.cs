using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PromptRepository : IPromptRepository
{
    private readonly AppDbContext _context;

    public PromptRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Prompt> AddAsync(Prompt prompt)
    {
        // EF Core va automatiquement insérer le Prompt ET la ReponseIum liée
        await _context.Prompts.AddAsync(prompt);
        await _context.SaveChangesAsync();
        return prompt;
    }

    public async Task<IEnumerable<Prompt>> GetByProjetIdAsync(int idProjet)
    {
        return await _context.Prompts
            .Include(p => p.IdEtudiantNavigation)
                .ThenInclude(e => e!.IdUtilisateurNavigation) // Le '!' pour éviter le warning
            .Include(p => p.ReponseIa) // On charge la réponse de l'IA
            .Where(p => p.IdProjet == idProjet)
            .OrderByDescending(p => p.DatePrompt) // Du plus récent au plus ancien
            .ToListAsync();
    }

    public async Task<IEnumerable<Prompt>> GetByEtudiantIdAsync(int idEtudiant)
    {
        return await _context.Prompts
            .Include(p => p.IdProjetNavigation)
            .Where(p => p.IdEtudiant == idEtudiant)
            .OrderByDescending(p => p.DatePrompt)
            .ToListAsync();
    }
    
}