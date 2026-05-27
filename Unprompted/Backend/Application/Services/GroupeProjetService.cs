using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class GroupeProjetService : IGroupeProjetService
{
    private readonly IGroupeRepository _groupeRepo;
    private readonly IAffectationRepository _affectationRepo;
    private readonly IProjetRepository _projetRepo;

    public GroupeProjetService(
        IGroupeRepository groupeRepo, 
        IAffectationRepository affectationRepo,
        IProjetRepository projetRepo)
    {
        _groupeRepo = groupeRepo;
        _affectationRepo = affectationRepo;
        _projetRepo = projetRepo;
    }

    public async Task<bool> CreerEquipeEtAssignerAsync(AssignationProjetRequestDto request)
    {
        // 1. On vérifie que le projet existe bien
        var projet = await _projetRepo.GetByIdAsync(request.IdProjet);
        if (projet == null)
            throw new Exception("Le projet spécifié est introuvable.");

        // 2. On crée le Groupe (l'équipe)
        var nouveauGroupe = new Groupe
        {
            NomGroupe = request.NomGroupe,
            IdProjet = request.IdProjet
        };

        // On n'affecte plus à une variable, on attend juste la fin de l'opération
        await _groupeRepo.AddAsync(nouveauGroupe); 

        // 3. On crée les affectations pour chaque étudiant
        foreach (var etu in request.Etudiants)
        {
            var affectation = new Affectation
            {
                IdEtudiant = etu.IdEtudiant,
                // EF Core a automatiquement rempli l'ID dans nouveauGroupe !
                IdGroupe = nouveauGroupe.IdGroupe, 
                IdRole = etu.IdRole,
                DateAffectation = DateTime.UtcNow
            };

            await _affectationRepo.AddAsync(affectation);
        }

        return true;
    }
}