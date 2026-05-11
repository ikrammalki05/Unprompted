using Application.DTOs;

namespace Application.Interfaces;

public interface IGroupeService
{
    Task<IEnumerable<GroupeDto>> GetAllGroupesAsync();
    Task<GroupeDto?> GetGroupeByIdAsync(int id);
    Task<GroupeDto> CreateGroupeAsync(GroupeCreateDto dto);
    Task DeleteGroupeAsync(int id);
    Task AddEtudiantToGroupeAsync(int idGroupe, EtudiantRoleDto dto);
    Task RemoveEtudiantFromGroupeAsync(int idGroupe, int idEtudiant);
}
