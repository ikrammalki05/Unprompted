using Application.DTOs;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class EtudiantService : IEtudiantService
{
    private readonly IEtudiantRepository _etudiantRepo;
    private readonly IUtilisateurRepository _utilisateurRepo;
    private readonly IKeycloakAdminService _keycloakService;
    private readonly ILogger<EtudiantService> _logger;
    private readonly IContributionRepository _contributionRepo;
    private readonly IPromptRepository _promptRepo;
    private readonly IEvaluationRepository _evaluationRepo; // Remplacement propre du DbContext !

    public EtudiantService(
        IEtudiantRepository etudiantRepo,
        IUtilisateurRepository utilisateurRepo,
        IKeycloakAdminService keycloakService,
        ILogger<EtudiantService> logger,
        IContributionRepository contributionRepo,
        IPromptRepository promptRepo,
        IEvaluationRepository evaluationRepo) // Injection du nouveau repo
    {
        _etudiantRepo = etudiantRepo;
        _utilisateurRepo = utilisateurRepo;
        _keycloakService = keycloakService;
        _logger = logger;
        _contributionRepo = contributionRepo;
        _promptRepo = promptRepo;
        _evaluationRepo = evaluationRepo;
    }

    public async Task<EtudiantDto> CreateEtudiantAsync(EtudiantCreateDto request)
    {
        // 1. Vérifier si l'email existe déjà
        var existingUser = await _utilisateurRepo.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new Exception("Un utilisateur avec cet email existe déjà.");

        // 2. Créer l'utilisateur dans Keycloak d'abord
        string keycloakUserId = string.Empty;
        try
        {
            keycloakUserId = await _keycloakService.CreateUserAsync(request.Email, request.Prenom, request.Nom);

            // Mot de passe temporaire : Prenom + CodeApogee (ex: Mohamed12345)
            var tempPassword = $"{request.Prenom}{request.CodeApogee}";
            await _keycloakService.SetUserPasswordAsync(keycloakUserId, tempPassword);

            _logger.LogInformation($"Etudiant créé dans Keycloak avec ID: {keycloakUserId}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Échec de la création dans Keycloak: {ex.Message}");
            throw new Exception($"Impossible de créer l'utilisateur dans Keycloak: {ex.Message}");
        }

        // 3. Créer l'Utilisateur en base de données
        Domain.Entities.Utilisateur nouvelUtilisateur;
        try
        {
            nouvelUtilisateur = new Domain.Entities.Utilisateur
            {
                Nom = request.Nom,
                Prenom = request.Prenom,
                Email = request.Email,
                Statut = "Actif"
            };
            await _utilisateurRepo.AddAsync(nouvelUtilisateur);
        }
        catch (Exception ex)
        {
            // Rollback : supprimer l'utilisateur Keycloak si la BD échoue
            _logger.LogError($"Échec de la création en BD, rollback Keycloak: {ex.Message}");
            await _keycloakService.DeleteUserAsync(keycloakUserId);
            throw;
        }

        // 4. Créer le profil Etudiant lié
        var nouvelEtudiant = new Domain.Entities.Etudiant
        {
            IdUtilisateur = nouvelUtilisateur.IdUtilisateur,
            CodeApogee = request.CodeApogee
        };
        await _etudiantRepo.AddAsync(nouvelEtudiant);

        // 5. Retourner le DTO pour le Frontend
        return new EtudiantDto
        {
            Id = nouvelEtudiant.IdEtudiant,
            NomComplet = $"{nouvelUtilisateur.Prenom} {nouvelUtilisateur.Nom}",
            Email = nouvelUtilisateur.Email,
            CodeApogee = nouvelEtudiant.CodeApogee,
            Statut = nouvelUtilisateur.Statut,
            ClasseNom = "Non assigné"
        };
    }

    public async Task<IEnumerable<EtudiantDto>> GetAllEtudiantsAsync()
    {
        var etudiants = await _etudiantRepo.GetAllAsync();
        
        return etudiants.Select(e => new EtudiantDto
        {
            Id = e.IdEtudiant,
            CodeApogee = e.CodeApogee,
            NomComplet = $"{e.IdUtilisateurNavigation?.Prenom} {e.IdUtilisateurNavigation?.Nom}",
            Email = e.IdUtilisateurNavigation?.Email ?? "Email inconnu",
            Statut = e.IdUtilisateurNavigation?.Statut ?? "Inactif",
            ClasseNom = e.Affectations?.FirstOrDefault()?.IdGroupeNavigation?.NomGroupe ?? "Non assigné"
        });
    }

    public async Task UpdateEtudiantAsync(int id, EtudiantCreateDto request)
    {
        var etudiant = await _etudiantRepo.GetByIdAsync(id);
        if (etudiant == null)
            throw new ArgumentException($"Etudiant avec l'id {id} introuvable.");

        var utilisateur = await _utilisateurRepo.GetByIdAsync(etudiant.IdUtilisateur);
        if (utilisateur == null)
            throw new ArgumentException("Utilisateur introuvable.");

        utilisateur.Nom = request.Nom;
        utilisateur.Prenom = request.Prenom;
        utilisateur.Email = request.Email;
        etudiant.CodeApogee = request.CodeApogee;

        await _utilisateurRepo.UpdateAsync(utilisateur);
        await _etudiantRepo.UpdateAsync(etudiant);
    }

    public async Task DeleteEtudiantAsync(int id)
    {
        var etudiant = await _etudiantRepo.GetByIdAsync(id);
        if (etudiant == null)
            throw new ArgumentException($"Etudiant avec l'id {id} introuvable.");

        await _etudiantRepo.DeleteAsync(id);
    }

    public async Task<EtudiantProfilDto?> GetProfilEtudiantAsync(int idEtudiant)
    {
        // On récupère l'étudiant via le Repository de ton collègue
        var etudiant = await _etudiantRepo.GetByIdAsync(idEtudiant);
        
        if (etudiant == null) 
            return null;

        // On mappe les données de l'entité vers le DTO
        return new EtudiantProfilDto
        {
            IdEtudiant = etudiant.IdEtudiant,
            CodeApogee = etudiant.CodeApogee,
            Niveau = etudiant.Niveau ?? "Non spécifié",
            Filiere = etudiant.Filiere ?? "Non spécifiée",
            
            // La navigation vers Utilisateur a été incluse par le Repository
            NomComplet = $"{etudiant.IdUtilisateurNavigation?.Prenom} {etudiant.IdUtilisateurNavigation?.Nom}",
            Email = etudiant.IdUtilisateurNavigation?.Email ?? "Inconnu",
            Statut = etudiant.IdUtilisateurNavigation?.Statut ?? "Inconnu"
        };
    }

    public async Task<EtudiantProfilDto?> GetProfilByEmailAsync(string email)
    {
        var utilisateur = await _utilisateurRepo.GetByEmailAsync(email);
        if (utilisateur == null) return null;

        var etudiant = await _etudiantRepo.GetByUtilisateurIdAsync(utilisateur.IdUtilisateur);
        if (etudiant == null) return null;

        return await GetProfilEtudiantAsync(etudiant.IdEtudiant);
    }

    public async Task UpdateProfilByEmailAsync(string email, EtudiantCreateDto request)
    {
        var utilisateur = await _utilisateurRepo.GetByEmailAsync(email);
        if (utilisateur == null) throw new Exception("Utilisateur introuvable.");

        var etudiant = await _etudiantRepo.GetByUtilisateurIdAsync(utilisateur.IdUtilisateur);
        if (etudiant == null) throw new Exception("Profil étudiant introuvable.");

        await UpdateEtudiantAsync(etudiant.IdEtudiant, request);
    }

    public async Task<HistoriqueEtudiantDto?> GetHistoriqueAsync(int idEtudiant)
    {
        // On vérifie que l'étudiant existe
        var etudiant = await _etudiantRepo.GetByIdAsync(idEtudiant);
        if (etudiant == null) return null;

        // 1. Récupérer les Commits Git
        var contributions = await _contributionRepo.GetByEtudiantIdAsync(idEtudiant);
        
        // 2. Récupérer les Requêtes IA
        var prompts = await _promptRepo.GetByEtudiantIdAsync(idEtudiant);

        // 3. Récupérer les Notes via le nouveau Repository
        var evaluationsBrutes = await _evaluationRepo.GetByEtudiantIdAsync(idEtudiant);
        
        var evaluations = evaluationsBrutes.Select(e => new EvaluationItemDto
        {
            Note = e.Note,
            Commentaire = e.Commentaire ?? string.Empty,
            DateEvaluation = e.DateEvaluation,
            NomProjet = e.IdProjetNavigation?.Titre ?? "Inconnu",
            NomEnseignant = e.IdEnseignantNavigation?.IdUtilisateurNavigation != null
                ? $"{e.IdEnseignantNavigation.IdUtilisateurNavigation.Prenom} {e.IdEnseignantNavigation.IdUtilisateurNavigation.Nom}"
                : "Inconnu"
        }).ToList();

        // 4. On assemble le tout dans la boîte finale !
        return new HistoriqueEtudiantDto
        {
            IdEtudiant = idEtudiant,
            ContributionsGit = contributions.Select(c => new ContributionItemDto
            {
                MessageCommit = c.MessageCommit ?? "Sans message",
                DateCommit = c.DateCommit,
                NomProjet = c.IdProjetNavigation?.Titre ?? "Inconnu",
                LignesAjoutees = c.LignesAjoutees ?? 0,
                LignesSupprimees = c.LignesSupprimees ?? 0
            }).ToList(),
            
            InteractionsIa = prompts.Select(p => new PromptItemDto
            {
                Question = p.Contenu ?? string.Empty,
                DateQuestion = p.DatePrompt,
                NomProjet = p.IdProjetNavigation?.Titre ?? "Inconnu",
                TokensConsommes = (p.NbTokensEntree ?? 0) + (p.NbTokensSortie ?? 0)
            }).ToList(),
            
            Evaluations = evaluations
        };
    }
}