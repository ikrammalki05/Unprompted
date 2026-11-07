// Application/DTOs/TreeNodeDto.cs

namespace Application.DTOs;

public class TreeNodeDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty; // "folder" | "file"

    public List<TreeNodeDto> Children { get; set; } = new();
}