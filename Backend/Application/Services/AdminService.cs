using Application.DTOs;
using Application.Interfaces;
using Infrastructure.Entities; // Assure-toi que c'est le bon namespace pour tes entités

namespace Application.Services;

public class AdminService : IAdminService
{
    private readonly IEtudiantRepository _etudiantRepo;
    private readonly IEnseignantRepository _enseignantRepo;
    private readonly IClasseRepository _classeRepo;
    private readonly IAffectationRepository _affectationRepo; // Parfait, on utilise le repo

    // On injecte bien IAffectationRepository ici, et on retire AppDbContext
    public AdminService(
        IEtudiantRepository etudiantRepo, 
        IEnseignantRepository enseignantRepo, 
        IClasseRepository classeRepo,
        IAffectationRepository affectationRepo)
    {
        _etudiantRepo = etudiantRepo;
        _enseignantRepo = enseignantRepo;
        _classeRepo = classeRepo;
        _affectationRepo = affectationRepo;
    }

    public async Task<object> GetDashboardStatsAsync()
    {
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

        int idRoleEnseignant = 2; 

        var nouvelleAffectation = new Domain.Entities.Affectation // Fais attention au namespace
        {
            IdEnseignant = request.IdEnseignant,
            IdGroupe = request.IdClasse,
            IdRole = idRoleEnseignant,
            DateAffectation = DateTime.UtcNow
        };

        // ON GARDE UNIQUEMENT CELLE-CI :
        await _affectationRepo.AddAsync(nouvelleAffectation);

        return true;
    }
}