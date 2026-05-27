using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/projects")]
public class TreeController : ControllerBase
{
    private readonly ITreeService _treeService;

    public TreeController(ITreeService treeService)
    {
        _treeService = treeService;
    }

    // GET: api/projects/{projectId}/tree
    [HttpGet("{projectId}/tree")]
    public async Task<IActionResult> GetTree(int projectId)
    {
        var tree = await _treeService.GetProjectTreeAsync(projectId);
        return Ok(tree);
    }
}