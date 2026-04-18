using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class AdminService : IAdminService
{
    private readonly IEtudiantRepository _etudiantRepo;
    private readonly IEnseignantRepository _enseignantRepo;
    private readonly IClasseRepository _classeRepo;
    private readonly IAffectationRepository _affectationRepo; // Parfait, on utilise le repo

    // On injecte bien IAffectationRepository ici, et on retire AppDbContext
    private readonly IEnseignantClasseRepository _enseignantClasseRepo;

    public AdminService(
    IEtudiantRepository etudiantRepo,
    IEnseignantRepository enseignantRepo,
    IClasseRepository classeRepo,
    IAffectationRepository affectationRepo,
    IEnseignantClasseRepository enseignantClasseRepo) // ← ajoute
      {
    _etudiantRepo = etudiantRepo;
    _enseignantRepo = enseignantRepo;
    _classeRepo = classeRepo;
    _affectationRepo = affectationRepo;
    _enseignantClasseRepo = enseignantClasseRepo; // ← ajoute
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

    var affectation = new EnseignantClasse
    {
        IdEnseignant = request.IdEnseignant,
        IdClasse = request.IdClasse,
        DateAffectation = DateTime.UtcNow
    };

    await _enseignantClasseRepo.AddAsync(affectation); // ← utilise le repo
    return true;
}
}