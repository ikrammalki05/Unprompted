using Application.DTOs;

namespace Application.Interfaces;

public interface IGroupeProjetService
{
    Task<bool> CreerEquipeEtAssignerAsync(AssignationProjetRequestDto request);
}