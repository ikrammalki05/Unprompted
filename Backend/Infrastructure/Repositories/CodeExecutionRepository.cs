using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ExecutionCodeRepository : IExecutionCodeRepository
{
    private readonly AppDbContext _context;

    public ExecutionCodeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ExecutionCode execution)
    {
        await _context.CodeExecutions.AddAsync(execution);
        await _context.SaveChangesAsync();
    }

    public async Task<ExecutionCode?> GetByIdAsync(Guid id)
    {
        return await _context.CodeExecutions
            .FirstOrDefaultAsync(e => e.IdExecution == id);
    }

    public async Task UpdateAsync(ExecutionCode execution)
    {
        _context.CodeExecutions.Update(execution);
        await _context.SaveChangesAsync();
    }
}