using System.Text.Json;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExecutionController : ControllerBase
{
    private readonly IExecutionCodeService _service;

    public ExecutionController(IExecutionCodeService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Executer(
        ExecutionProjetDto dto)
    {
        var idUtilisateur =
            User.FindFirst("sub")?.Value ?? "system";

        var result =
            await _service.DemarrerExecutionAsync(
                dto,
                idUtilisateur);

        return Ok(result);
    }

    [HttpGet("{idExecution}")]
    public async Task<IActionResult> Obtenir(
        Guid idExecution)
    {
        var result =
            await _service.GetExecutionAsync(
                idExecution);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("{idExecution}/stop")]
    public async Task<IActionResult> Arreter(
        Guid idExecution)
    {
        await _service.ArreterExecutionAsync(idExecution);

        return Ok();
    }

    [HttpGet("{id}/stream")]
    public async Task StreamExecution(Guid id, CancellationToken ct)
    {
        Response.Headers["Content-Type"] = "text/event-stream";
        Response.Headers["Cache-Control"] = "no-cache";

        while (!ct.IsCancellationRequested)
        {
            var execution = await _service.GetExecutionAsync(id);

            if (execution == null)
            {
                await Response.WriteAsync("data: {\"error\": \"Execution introuvable\"}\n\n");
                await Response.Body.FlushAsync();
                break;
            }

            await Response.WriteAsync($"data: {JsonSerializer.Serialize(execution)}\n\n");
            await Response.Body.FlushAsync();

            if (execution.Statut == "completed" || execution.Statut == "stopped")
                break;

            await Task.Delay(500, ct);
        }
    }
}