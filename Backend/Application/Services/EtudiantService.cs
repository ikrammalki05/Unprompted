using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class EtudiantService : IEtudiantService
{
    private readonly IEtudiantRepository _etudiantRepo;
    private readonly IUtilisateurRepository _utilisateurRepo;

   public EtudiantService(IEtudiantRepository etudiantRepo, IUtilisateurRepository utilisateurRepo)
    {
        _etudiantRepo = etudiantRepo;
        _utilisateurRepo = utilisateurRepo;
    }

    public async Task<EtudiantDto> CreateEtudiantAsync(EtudiantCreateDto request)
    {
        // 1. Vérifier si l'email existe déjà
        var existingUser = await _utilisateurRepo.GetByEmailAsync(request.Email);
        if (existingUser != null)
            throw new Exception("Un utilisateur avec cet email existe déjà.");

        // 2. Créer l'Utilisateur parent
        var nouvelUtilisateur = new Domain.Entities.Utilisateur
        {
            Nom = request.Nom,
            Prenom = request.Prenom,
            Email = request.Email,
            MotDePasse = request.MotDePasse, 
            Statut = "Actif" // Par défaut
        };
        await _utilisateurRepo.AddAsync(nouvelUtilisateur);

        // 3. Créer le profil Etudiant lié
        var nouvelEtudiant = new Domain.Entities.Etudiant
        {
            IdUtilisateur = nouvelUtilisateur.IdUtilisateur, // L'ID a été généré par la BDD
            CodeApogee = request.CodeApogee
        };
        await _etudiantRepo.AddAsync(nouvelEtudiant);

        // 4. Retourner le DTO pour le Frontend
        return new EtudiantDto
        {
            Id = nouvelEtudiant.IdEtudiant,
            NomComplet = $"{nouvelUtilisateur.Prenom} {nouvelUtilisateur.Nom}",
            Email = nouvelUtilisateur.Email,
            CodeApogee = nouvelEtudiant.CodeApogee,
            Statut = nouvelUtilisateur.Statut,
            ClasseNom = "Non assigné" // Vient d'être créé
        };
    }


    public async Task<IEnumerable<EtudiantDto>> GetAllEtudiantsAsync()
    {
        var etudiants = await _etudiantRepo.GetAllAsync();
        
        // On transforme (Map) l'entité de la BDD vers le DTO attendu par Figma
        return etudiants.Select(e => new EtudiantDto
        {
            Id = e.IdEtudiant,
            CodeApogee = e.CodeApogee,
            // On suppose que le Repository inclut la navigation vers Utilisateur
            NomComplet = $"{e.IdUtilisateurNavigation?.Prenom} {e.IdUtilisateurNavigation?.Nom}",
            Email = e.IdUtilisateurNavigation?.Email ?? "Email inconnu",
            Statut = e.IdUtilisateurNavigation?.Statut ?? "Inactif",
            // On récupère le nom du premier groupe (classe) auquel il est affecté
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

    
}