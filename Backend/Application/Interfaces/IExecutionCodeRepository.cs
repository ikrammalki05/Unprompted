using Domain.Entities;

namespace Application.Interfaces;

public interface IExecutionCodeRepository
{
    Task AddAsync(ExecutionCode execution);

    Task<ExecutionCode?> GetByIdAsync(Guid id);

    Task UpdateAsync(ExecutionCode execution);
}