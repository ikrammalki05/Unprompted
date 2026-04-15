using Application.DTOs;
using Application.Interfaces;
using Infrastructure; // Pour l'entité Affectation
using Infrastructure.Data; // Uniquement si l'Affectation n'a pas de Repository propre

namespace Application.Services;

public class AdminService : IAdminService
{
    private readonly IEtudiantRepository _etudiantRepo;
    private readonly IEnseignantRepository _enseignantRepo;
    private readonly IClasseRepository _classeRepo;
    // Si ton collègue n'a pas créé de IAffectationRepository, 
    // on injecte le DbContext juste pour cette action spécifique.
    private readonly IAffectationRepository _affectationRepo; 

    public AdminService(
        IEtudiantRepository etudiantRepo, 
        IEnseignantRepository enseignantRepo, 
        IClasseRepository classeRepo,
        AppDbContext context)
    {
        _etudiantRepo = etudiantRepo;
        _enseignantRepo = enseignantRepo;
        _classeRepo = classeRepo;
        _context = context;
    }

    public async Task<object> GetDashboardStatsAsync()
    {
        // On récupère les compteurs pour les 3 cartes Figma
        var totalEtudiants = await _etudiantRepo.CountAsync();
        var totalEnseignants = await _enseignantRepo.CountAsync();
        var totalClasses = await _classeRepo.CountAsync();

        return new 
        {
            TotalEtudiants = totalEtudiants,
            TotalEnseignants = totalEnseignants,
            TotalClasses = totalClasses
        };
    }

    public async Task<bool> AssignerEnseignantAClasseAsync(AffectationEnseignantRequestDto request)
    {
        var enseignant = await _enseignantRepo.GetByIdAsync(request.IdEnseignant);
        var classe = await _classeRepo.GetByIdAsync(request.IdClasse);

        if (enseignant == null || classe == null)
            throw new ArgumentException("Enseignant ou Classe introuvable.");

        // On cherche le rôle "Enseignant Réfèrent" (IdRole = 2 par exemple, selon ton DB seed)
        int idRoleEnseignant = 2; 

        var nouvelleAffectation = new Affectation
        {
            IdEnseignant = request.IdEnseignant,
            IdGroupe = request.IdClasse, // Groupe = Classe
            IdRole = idRoleEnseignant,
            DateAffectation = DateTime.UtcNow
        };

        _context.Affectations.Add(nouvelleAffectation);
        await _affectationRepo.AddAsync(nouvelleAffectation);

        return true;
    }
}