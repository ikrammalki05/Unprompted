using Application.DTOs;

namespace Application.Interfaces;

public interface IAffectationService
{
    Task<bool> CreerAffectationAsync(AffectationRequestDto request);
}