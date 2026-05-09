using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class ProjetService : IProjetService
{
    private readonly IProjetRepository _projetRepo;
    private readonly IEnseignantRepository _enseignantRepo;
    private readonly IEtudiantRepository _etudiantRepo;

    public ProjetService(
        IProjetRepository projetRepo,
        IEnseignantRepository enseignantRepo,
        IEtudiantRepository etudiantRepo)
    {
        _projetRepo = projetRepo;
        _enseignantRepo = enseignantRepo;
        _etudiantRepo = etudiantRepo;
    }

    public async Task<IEnumerable<ProjetDto>> GetAllProjectsAsync()
    {
        var projets = await _projetRepo.GetAllAsync();
        return projets.Select(MapToDto);
    }

    // Nom aligné : GetProjectByIdAsync
    public async Task<ProjetDto?> GetProjectByIdAsync(int id)
    {
        var projet = await _projetRepo.GetByIdAsync(id);
        return projet == null ? null : MapToDto(projet);
    }

    // Nom aligné : GetProjectsByEnseignantIdAsync
    public async Task<IEnumerable<ProjetDto>> GetProjectsByEnseignantIdAsync(int idEnseignant)
    {
        var projets = await _projetRepo.GetByEnseignantIdAsync(idEnseignant);
        return projets.Select(MapToDto);
    }

    public async Task<ProjetDto> CreateProjetAsync(int idEnseignant, ProjetCreateDto dto)
    {
        var enseignant = await _enseignantRepo.GetByIdAsync(idEnseignant);
        if (enseignant == null)
            throw new ArgumentException("Enseignant introuvable.");

        var projet = new Projet
        {
            Titre = dto.Titre,
            Description = dto.Description,
            DateDebut = dto.DateDebut,
            DateFin = dto.DateFin,
            UrlGit = dto.UrlGit,
            Duree = dto.Duree,
            Statut = "En cours",
            IdEnseignant = idEnseignant,
            Progression = 0,
            NotesEnseignant = null,
            Objectifs = dto.Objectifs,
            Livrables = dto.Livrables,
            CriteresEvaluation = dto.CriteresEvaluation,
            TechnologiesRequises = dto.TechnologiesRequises,
            Contraintes = dto.Contraintes,
            RessourcesDisponibles = dto.RessourcesDisponibles
        };

        await _projetRepo.AddAsync(projet);
        return MapToDto(projet);
    }

    public async Task UpdateProjetSuiviAsync(int id, ProjetSuiviDto dto)
    {
        var projet = await _projetRepo.GetByIdAsync(id);
        if (projet == null)
            throw new ArgumentException($"Projet {id} introuvable.");

        if (dto.Progression.HasValue)
        {
            if (dto.Progression < 0 || dto.Progression > 100)
                throw new ArgumentException("La progression doit être comprise entre 0 et 100.");
            projet.Progression = dto.Progression;
        }

        if (dto.NotesEnseignant != null)
            projet.NotesEnseignant = dto.NotesEnseignant;

        await _projetRepo.UpdateAsync(projet);
    }

    public async Task<ContributionDto> CreateContributionAsync(int idProjet, int idEtudiant, ContributionCreateDto dto)
    {
        var projet = await _projetRepo.GetByIdAsync(idProjet);
        if (projet == null)
            throw new ArgumentException("Projet introuvable.");

        var etudiant = await _etudiantRepo.GetByIdAsync(idEtudiant);
        if (etudiant == null)
            throw new ArgumentException("Etudiant introuvable.");

        var contribution = new Contribution
        {
            MessageCommit = dto.MessageCommit,
            DateCommit = dto.DateCommit ?? DateTime.UtcNow,
            HashCommit = dto.HashCommit,
            IdEtudiant = idEtudiant,
            IdProjet = idProjet,
            LignesAjoutees = dto.LignesAjoutees,
            LignesSupprimees = dto.LignesSupprimees
        };

        await _projetRepo.AddContributionAsync(contribution);

        return new ContributionDto
        {
            IdContribution = contribution.IdContribution,
            MessageCommit = contribution.MessageCommit,
            DateCommit = contribution.DateCommit,
            HashCommit = contribution.HashCommit,
            IdEtudiant = contribution.IdEtudiant,
            IdProjet = contribution.IdProjet,
            LignesAjoutees = contribution.LignesAjoutees,
            LignesSupprimees = contribution.LignesSupprimees
        };
    }

    public async Task<IEnumerable<ContributionDto>> GetContributionsByProjectIdAsync(int idProjet)
    {
        var contributions = await _projetRepo.GetContributionsByProjectIdAsync(idProjet);
        return contributions.Select(c => new ContributionDto
        {
            IdContribution = c.IdContribution,
            MessageCommit = c.MessageCommit,
            DateCommit = c.DateCommit,
            HashCommit = c.HashCommit,
            IdEtudiant = c.IdEtudiant,
            IdProjet = c.IdProjet,
            LignesAjoutees = c.LignesAjoutees,
            LignesSupprimees = c.LignesSupprimees
        });
    }

    public async Task<IEnumerable<ContributionDto>> GetContributionsByProjectAndEtudiantAsync(int idProjet, int idEtudiant)
    {
        var contributions = await _projetRepo.GetContributionsByProjectAndEtudiantAsync(idProjet, idEtudiant);
        return contributions.Select(c => new ContributionDto
        {
            IdContribution = c.IdContribution,
            MessageCommit = c.MessageCommit,
            DateCommit = c.DateCommit,
            HashCommit = c.HashCommit,
            IdEtudiant = c.IdEtudiant,
            IdProjet = c.IdProjet,
            LignesAjoutees = c.LignesAjoutees,
            LignesSupprimees = c.LignesSupprimees
        });
    }

    public async Task UpdateProjetAsync(int id, int idEnseignant, ProjetCreateDto dto)
    {
        var projet = await _projetRepo.GetByIdAsync(id);
        if (projet == null)
            throw new ArgumentException($"Projet {id} introuvable.");
        
        if (projet.IdEnseignant != idEnseignant)
            throw new ArgumentException("Vous n'êtes pas autorisé à modifier ce projet.");

        projet.Titre = dto.Titre;
        projet.Description = dto.Description;
        projet.DateDebut = dto.DateDebut;
        projet.DateFin = dto.DateFin;
        projet.UrlGit = dto.UrlGit;
        projet.Duree = dto.Duree;
        projet.Objectifs = dto.Objectifs;
        projet.Livrables = dto.Livrables;
        projet.CriteresEvaluation = dto.CriteresEvaluation;
        projet.TechnologiesRequises = dto.TechnologiesRequises;
        projet.Contraintes = dto.Contraintes;
        projet.RessourcesDisponibles = dto.RessourcesDisponibles;

        await _projetRepo.UpdateAsync(projet);
    }

    public async Task DeleteProjetAsync(int id, int idEnseignant)
    {
        var projet = await _projetRepo.GetByIdAsync(id);
        if (projet == null)
            throw new ArgumentException($"Projet {id} introuvable.");

        if (projet.IdEnseignant != idEnseignant)
            throw new ArgumentException("Vous n'êtes pas autorisé à supprimer ce projet.");

        await _projetRepo.DeleteAsync(id);
    }

    // AssignerEtudiantAsync
    public async Task AssignerEtudiantAsync(AssignerEtudiantProjetDto dto , int idEnseignant)
    {
        var projet = await _projetRepo.GetByIdAsync(dto.IdProjet);
        if (projet == null) throw new ArgumentException("Projet introuvable.");

        var etudiant = await _etudiantRepo.GetByIdAsync(dto.IdEtudiant);
        if (etudiant == null) throw new ArgumentException("Etudiant introuvable.");

        // On récupère le premier groupe du projet pour y mettre l'étudiant
        var groupe = projet.Groupes?.FirstOrDefault();
        if (groupe == null)
            throw new ArgumentException("Ce projet n'a pas encore de groupe configuré.");

        var affectation = new Affectation
        {
            IdEtudiant = dto.IdEtudiant,
            IdGroupe = groupe.IdGroupe,
            IdRole = 1, // Membre par défaut
            IdEnseignant = idEnseignant,
            DateAffectation = DateTime.UtcNow
        };

        await _projetRepo.AddAffectationAsync(affectation);
    }
    public async Task<int> GetProjetsCountAsync()
    {
        return await _projetRepo.CountAsync();
    }

    public async Task<IEnumerable<GroupeDto>> GetGroupesProjetAsync(int idProjet)
    {
        var projet = await _projetRepo.GetByIdAsync(idProjet);
        if (projet == null)
            throw new ArgumentException("Projet introuvable.");

        if (projet.Groupes == null)
            return new List<GroupeDto>();

        return projet.Groupes.Select(g => new GroupeDto
        {
            IdGroupe = g.IdGroupe,
            NomGroupe = g.NomGroupe,
            IdProjet = g.IdProjet,
            Etudiants = g.Affectations?.Where(a => a.IdEtudiantNavigation?.IdUtilisateurNavigation != null && a.IdRoleNavigation != null)
                .Select(a => new EtudiantGroupeDto
                {
                    IdEtudiant = a.IdEtudiant,
                    NomComplet = $"{a.IdEtudiantNavigation.IdUtilisateurNavigation.Prenom} {a.IdEtudiantNavigation.IdUtilisateurNavigation.Nom}",
                    Role = a.IdRoleNavigation.NomRole
                }).ToList() ?? new List<EtudiantGroupeDto>()
        });
    }

    public async Task<IEnumerable<EtudiantGroupeDto>> GetColleguesAsync(int idProjet, int idEtudiant)
    {
        var projet = await _projetRepo.GetByIdAsync(idProjet);
        if (projet == null) throw new ArgumentException("Projet introuvable.");

        var groupe = projet.Groupes?.FirstOrDefault(g => g.Affectations.Any(a => a.IdEtudiant == idEtudiant));
        if (groupe == null) return new List<EtudiantGroupeDto>();

        return groupe.Affectations
            .Where(a => a.IdEtudiantNavigation?.IdUtilisateurNavigation != null && a.IdRoleNavigation != null)
            .Select(a => new EtudiantGroupeDto
            {
                IdEtudiant = a.IdEtudiant,
                NomComplet = $"{a.IdEtudiantNavigation.IdUtilisateurNavigation.Prenom} {a.IdEtudiantNavigation.IdUtilisateurNavigation.Nom}",
                Role = a.IdRoleNavigation.NomRole
            }).ToList();
    }

    public async Task AssignerProjetAuGroupeAsync(AssignerProjetGroupeDto dto)
    {
        var projet = await _projetRepo.GetByIdAsync(dto.IdProjet);
        if (projet == null) throw new ArgumentException("Projet introuvable.");

        // On peut ajouter une vérification si le groupe existe via le repo projet si on veut, 
        // ou laisser le repo s'en occuper.
        await _projetRepo.AssignProjectToGroupAsync(dto.IdProjet, dto.IdGroupe);
    }

    public async Task<EtudiantActiviteDto> GetEtudiantActiviteAsync(int idProjet, int idEtudiant)
    {
        var prompts = await _projetRepo.GetPromptsByProjectAndEtudiantAsync(idProjet, idEtudiant);
        var contributions = await _projetRepo.GetContributionsByProjectAndEtudiantAsync(idProjet, idEtudiant);

        return new EtudiantActiviteDto
        {
            Prompts = prompts.Select(p => new PromptDto
            {
                IdPrompt = p.IdPrompt,
                Contenu = p.Contenu,
                DatePrompt = p.DatePrompt,
                NbTokensEntree = p.NbTokensEntree,
                NbTokensSortie = p.NbTokensSortie,
                IdEtudiant = p.IdEtudiant,
                IdProjet = p.IdProjet
            }),
            Contributions = contributions.Select(c => new ContributionDto
            {
                IdContribution = c.IdContribution,
                MessageCommit = c.MessageCommit,
                DateCommit = c.DateCommit,
                HashCommit = c.HashCommit,
                IdEtudiant = c.IdEtudiant,
                IdProjet = c.IdProjet,
                LignesAjoutees = c.LignesAjoutees,
                LignesSupprimees = c.LignesSupprimees
            })
        };
    }

    public async Task<PromptDto> CreatePromptAsync(int idProjet, PromptCreateDto dto)
    {
        var prompt = new Prompt
        {
            Contenu = dto.Contenu,
            IdEtudiant = dto.IdEtudiant,
            IdProjet = idProjet,
            DatePrompt = DateTime.UtcNow,
            NbTokensEntree = dto.NbTokensEntree ?? 0,
            NbTokensSortie = dto.NbTokensSortie ?? 0
        };

        await _projetRepo.AddPromptAsync(prompt);

        return new PromptDto
        {
            IdPrompt = prompt.IdPrompt,
            Contenu = prompt.Contenu,
            DatePrompt = prompt.DatePrompt,
            NbTokensEntree = prompt.NbTokensEntree,
            NbTokensSortie = prompt.NbTokensSortie,
            IdEtudiant = prompt.IdEtudiant,
            IdProjet = prompt.IdProjet
        };
    }

    public async Task<IEnumerable<ProjetDto>> GetProjectsByEtudiantIdAsync(int idEtudiant)
    {
        var projets = await _projetRepo.GetByEtudiantIdAsync(idEtudiant);
        return projets.Select(MapToDto);
    }

    private static ProjetDto MapToDto(Projet p) => new ProjetDto
    {
        IDProjet = p.IdProjet,
        Titre = p.Titre,
        Description = p.Description,
        Status = p.Statut,
        DateDebut = p.DateDebut,
        DateFin = p.DateFin,
        Duree = p.Duree,
        UrlGit = p.UrlGit,
        Progression = p.Progression,
        NotesEnseignant = p.NotesEnseignant,
        IdEnseignant = p.IdEnseignant,
        Membres = p.Groupes?.SelectMany(g => g.Affectations ?? new List<Affectation>())
            .Where(a => a.IdEtudiantNavigation != null && a.IdEtudiantNavigation.IdUtilisateurNavigation != null && a.IdRoleNavigation != null)
            .Select(a => new EtudiantGroupeDto
            {
                IdEtudiant = a.IdEtudiant,
                NomComplet = $"{a.IdEtudiantNavigation.IdUtilisateurNavigation.Prenom} {a.IdEtudiantNavigation.IdUtilisateurNavigation.Nom}",
                Role = a.IdRoleNavigation.NomRole
            }).ToList() ?? new List<EtudiantGroupeDto>(),
        Objectifs = p.Objectifs,
    Livrables = p.Livrables,
    CriteresEvaluation = p.CriteresEvaluation,
    TechnologiesRequises = p.TechnologiesRequises,
    Contraintes = p.Contraintes,
    RessourcesDisponibles = p.RessourcesDisponibles,
    HasCahierDesCharges = p.CahierDesCharges != null
    };
    public async Task SaveCahierAsync(int idProjet, byte[] contenu)
{
    var projet = await _projetRepo.GetByIdAsync(idProjet);
    if (projet == null)
        throw new ArgumentException("Projet introuvable.");

    projet.CahierDesCharges = contenu;
    await _projetRepo.UpdateAsync(projet);
}

public async Task<byte[]?> GetCahierAsync(int idProjet)
{
    var projet = await _projetRepo.GetByIdAsync(idProjet);
    return projet?.CahierDesCharges;
}
}