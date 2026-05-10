using Application.DTOs;

namespace Application.Interfaces;

public interface ICodeExecutionService
{
    Task<string> ExecuteAsync(RunProjectDto dto);
}