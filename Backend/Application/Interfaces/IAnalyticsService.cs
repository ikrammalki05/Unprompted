using Application.DTOs;

namespace Application.Interfaces;

public interface IAnalyticsService
{
    Task<IEnumerable<AnalyticsEtudiantDto>> GenererRapportProjetAsync(int idProjet);
}