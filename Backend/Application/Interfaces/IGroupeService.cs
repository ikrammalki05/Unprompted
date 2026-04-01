using Application.DTOs;

namespace Application.Interfaces;

public interface IGroupeService
{
    Task<IEnumerable<GroupeDto>> GetAllGroupesAsync();
    Task<GroupeDto?> GetGroupeByIdAsync(int id);
}