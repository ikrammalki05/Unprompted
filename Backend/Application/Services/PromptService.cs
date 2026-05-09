using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class PromptService : IPromptService
{
    private readonly IPromptRepository _promptRepo;

    public PromptService(IPromptRepository promptRepo)
    {
        _promptRepo = promptRepo;
    }

    public async Task<bool> LogInteractionAsync(PromptLogRequestDto request)
    {
        // On construit l'objet complet : Question + Réponse
        var nouveauPrompt = new Prompt
        {
            IdEtudiant = request.IdEtudiant,
            IdProjet = request.IdProjet,
            Contenu = request.ContenuPrompt,
            NbTokensEntree = request.NbTokensEntree,
            NbTokensSortie = request.NbTokensSortie,
            DatePrompt = DateTime.UtcNow,
            // Magie d'EF Core : On attache la réponse directement
            ReponseIa = new List<ReponseIum>
            {
                new ReponseIum
                {
                    ContenuReponse = request.ContenuReponse,
                    ModeleIa = request.ModeleIa,
                    DateReponse = DateTime.UtcNow
                }
            }
        };

        await _promptRepo.AddAsync(nouveauPrompt);
        return true;
    }

    public async Task<IEnumerable<PromptDto>> GetHistoriqueProjetAsync(int idProjet)
    {
        var prompts = await _promptRepo.GetByProjetIdAsync(idProjet);

        return prompts.Select(p => new PromptDto
        {
            IdPrompt = p.IdPrompt,
            Contenu = p.Contenu,
            DatePrompt = p.DatePrompt,
            NbTokensEntree = p.NbTokensEntree,
            NbTokensSortie = p.NbTokensSortie,
            // Extraction du nom de l'étudiant
            NomEtudiant = p.IdEtudiantNavigation?.IdUtilisateurNavigation != null 
                ? $"{p.IdEtudiantNavigation.IdUtilisateurNavigation.Prenom} {p.IdEtudiantNavigation.IdUtilisateurNavigation.Nom}" 
                : "Inconnu",
            // Mapping des réponses
            Reponses = p.ReponseIa.Select(r => new ReponseIumDto
            {
                IdReponse = r.IdReponse,
                ContenuReponse = r.ContenuReponse,
                DateReponse = r.DateReponse,
                ModeleIa = r.ModeleIa
            }).ToList()
        });
    }
}