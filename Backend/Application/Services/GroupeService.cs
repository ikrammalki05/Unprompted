using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class GroupeService : IGroupeService
{
    private readonly IGroupeRepository _groupeRepo;
    private readonly IProjetRepository _projetRepo;
    private readonly IEtudiantRepository _etudiantRepo;

    public GroupeService(
        IGroupeRepository groupeRepo,
        IProjetRepository projetRepo,
        IEtudiantRepository etudiantRepo)
    {
        _groupeRepo = groupeRepo;
        _projetRepo = projetRepo;
        _etudiantRepo = etudiantRepo;
    }

    public async Task<IEnumerable<GroupeDto>> GetAllGroupesAsync()
    {
        var groupes = await _groupeRepo.GetAllAsync();
        return groupes.Select(MapToDto);
    }

    public async Task<GroupeDto?> GetGroupeByIdAsync(int id)
    {
        var groupe = await _groupeRepo.GetByIdAsync(id);
        return groupe == null ? null : MapToDto(groupe);
    }

    public async Task<GroupeDto> CreateGroupeAsync(GroupeCreateDto dto)
    {
        var groupe = new Groupe
        {
            NomGroupe = dto.NomGroupe,
            IdProjet = dto.IdProjet ?? 0 // On gère le cas où IdProjet est null si la DB l'autorise ou on met une valeur par défaut
        };

        await _groupeRepo.AddAsync(groupe);

        foreach (var etudiantDto in dto.Etudiants)
        {
            var affectation = new Affectation
            {
                IdGroupe = groupe.IdGroupe,
                IdEtudiant = etudiantDto.IdEtudiant,
                IdRole = etudiantDto.IdRole,
                DateAffectation = DateTime.UtcNow
            };
            await _groupeRepo.AddAffectationAsync(affectation);
        }

        // Re-récupérer avec les inclusions
        var createdGroupe = await _groupeRepo.GetByIdAsync(groupe.IdGroupe);
        return MapToDto(createdGroupe!);
    }

    public async Task DeleteGroupeAsync(int id)
    {
        // Supprimer d'abord toutes les affectations liées à ce groupe
        await _groupeRepo.RemoveAllAffectationsForGroupeAsync(id);
        
        // Puis supprimer le groupe
        await _groupeRepo.DeleteAsync(id);
    }

    public async Task AddEtudiantToGroupeAsync(int idGroupe, EtudiantRoleDto dto)
    {
        var affectation = new Affectation
        {
            IdGroupe = idGroupe,
            IdEtudiant = dto.IdEtudiant,
            IdRole = dto.IdRole,
            DateAffectation = DateTime.UtcNow
        };
        await _groupeRepo.AddAffectationAsync(affectation);
    }

    public async Task RemoveEtudiantFromGroupeAsync(int idGroupe, int idEtudiant)
    {
        await _groupeRepo.RemoveAffectationAsync(idEtudiant, idGroupe);
    }

    private GroupeDto MapToDto(Groupe g)
    {
        return new GroupeDto
        {
            IdGroupe = g.IdGroupe,
            NomGroupe = g.NomGroupe,
            IdProjet = g.IdProjet,
            Etudiants = g.Affectations.Select(a => new EtudiantGroupeDto
            {
                IdEtudiant = a.IdEtudiant,
                NomComplet = a.IdEtudiantNavigation?.IdUtilisateurNavigation != null 
                    ? $"{a.IdEtudiantNavigation.IdUtilisateurNavigation.Prenom} {a.IdEtudiantNavigation.IdUtilisateurNavigation.Nom}"
                    : "Etudiant inconnu",
                Role = a.IdRoleNavigation?.NomRole ?? "Aucun rôle"
            }).ToList()
        };
    }
}
