using Application.DTOs;

namespace Application.Interfaces;

public interface IProjetService
{
    Task<IEnumerable<ProjetDto>> GetAllProjectsAsync(); // Retrait du 'h'
    Task<ProjetDto?> GetProjectByIdAsync(int id);
    Task<IEnumerable<ProjetDto>> GetProjectsByEnseignantIdAsync(int idEnseignant);
    Task<ProjetDto> CreateProjetAsync(int idEnseignant, ProjetCreateDto dto);
    Task UpdateProjetAsync(int id, int idEnseignant, ProjetCreateDto dto);
    Task UpdateProjetSuiviAsync(int id, ProjetSuiviDto dto);
    Task<ContributionDto> CreateContributionAsync(int idProjet, int idEtudiant, ContributionCreateDto dto);
    Task<IEnumerable<GroupeDto>> GetGroupesProjetAsync(int idProjet);
    Task DeleteProjetAsync(int id, int idEnseignant);
    Task AssignerEtudiantAsync(AssignerEtudiantProjetDto dto , int idEnseignant);
    Task<IEnumerable<ContributionDto>> GetContributionsByProjectIdAsync(int idProjet);
    Task<IEnumerable<ContributionDto>> GetContributionsByProjectAndEtudiantAsync(int idProjet, int idEtudiant);
    Task<int> GetProjetsCountAsync();
    Task AssignerProjetAuGroupeAsync(AssignerProjetGroupeDto dto);
    Task<EtudiantActiviteDto> GetEtudiantActiviteAsync(int idProjet, int idEtudiant);
    Task<PromptDto> CreatePromptAsync(int idProjet, PromptCreateDto dto);
    Task<IEnumerable<ProjetDto>> GetProjectsByEtudiantIdAsync(int idEtudiant);
    Task<IEnumerable<EtudiantGroupeDto>> GetColleguesAsync(int idProjet, int idEtudiant);
    Task SaveCahierAsync(int idProjet, byte[] contenu);
Task<byte[]?> GetCahierAsync(int idProjet);
}