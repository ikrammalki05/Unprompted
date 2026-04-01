using Application.DTOs;
using Application.Interfaces;
using Infrastructure; // Pour accéder aux Entités comme Affectation
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AffectationService : IAffectationService
{
    private readonly AppDbContext _context;

    public AffectationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreerAffectationAsync(AffectationRequestDto request)
    {
        // 1. Vérifications de base (Est-ce que tout existe en base de données ?)
        var etudiantExiste = await _context.Etudiants.AnyAsync(e => e.IdEtudiant == request.IdEtudiant);
        var groupeExiste = await _context.Groupes.AnyAsync(g => g.IdGroupe == request.IdGroupe);
        var roleExiste = await _context.Roles.AnyAsync(r => r.IdRole == request.IdRole);

        if (!etudiantExiste || !groupeExiste || !roleExiste)
        {
            throw new ArgumentException("L'étudiant, le groupe ou le rôle spécifié n'existe pas.");
        }

        // 2. Règle métier : Vérifier que l'étudiant n'est pas DEJA dans ce groupe avec ce rôle
        var affectationExistante = await _context.Affectations
            .AnyAsync(a => a.IdEtudiant == request.IdEtudiant && 
                           a.IdGroupe == request.IdGroupe && 
                           a.IdRole == request.IdRole);

        if (affectationExistante)
        {
            throw new InvalidOperationException("Cet étudiant possède déjà ce rôle dans ce groupe.");
        }

        // 3. Création de l'entité Affectation
        var nouvelleAffectation = new Affectation
        {
            IdEtudiant = request.IdEtudiant,
            IdGroupe = request.IdGroupe,
            IdRole = request.IdRole,
            IdEnseignant = request.IdEnseignant, // Peut être null selon le DTO
            DateAffectation = DateTime.UtcNow
        };

        // 4. Ajout et sauvegarde
        _context.Affectations.Add(nouvelleAffectation);
        await _context.SaveChangesAsync();

        return true;
    }
}