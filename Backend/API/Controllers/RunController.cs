using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RunController : ControllerBase
{
    private readonly ICodeExecutionService _executionService;

    public RunController(ICodeExecutionService executionService)
    {
        _executionService = executionService;
    }

    [HttpPost]
    public async Task<IActionResult> RunCode(RunProjectDto dto)
    {
        var result = await _executionService.ExecuteAsync(dto);
        return Ok(new { output = result });
    }
}