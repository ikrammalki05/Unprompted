// Application/Interfaces/ITreeService.cs

using Application.DTOs;

public interface ITreeService
{
    Task<List<TreeNodeDto>> GetProjectTreeAsync(int projectId);
}