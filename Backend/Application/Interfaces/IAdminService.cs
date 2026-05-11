using Application.DTOs;

namespace Application.Interfaces;

public interface IAdminService
{
    // Pour les 3 cartes en haut du Dashboard
    Task<object> GetDashboardStatsAsync(); 
    
    // Pour le panneau bleu à droite "Assigner un enseignant"
    Task<bool> AssignerEnseignantAClasseAsync(AffectationEnseignantRequestDto request);
}