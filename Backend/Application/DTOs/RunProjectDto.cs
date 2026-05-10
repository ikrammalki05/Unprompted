namespace Application.DTOs;

public class RunProjectDto
{
    public string Language { get; set; } = string.Empty;

    public List<ProjectFileDto> Files { get; set; } = new();
}

public class ProjectFileDto
{
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}