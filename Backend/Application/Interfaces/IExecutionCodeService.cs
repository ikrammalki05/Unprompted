using Application.DTOs;

namespace Application.Interfaces;

public interface IExecutionCodeService
{
    Task<ExecutionCodeDto> DemarrerExecutionAsync(ExecutionProjetDto dto, string idUtilisateur);
    Task<StatutExecutionDto?> GetExecutionAsync(Guid idExecution);
    Task ArreterExecutionAsync(Guid idExecution);
}