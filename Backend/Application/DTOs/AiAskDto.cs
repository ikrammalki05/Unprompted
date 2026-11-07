namespace Application.DTOs;

public class AiAskRequestDto
{
    public int IdProjet { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class AiAskResponseDto
{
    public string Response { get; set; } = string.Empty;
    public int TokensUsed { get; set; }
    public double DependencyScore { get; set; }
    public string Status { get; set; } = "success";
    public string AccessLevel { get; set; } = string.Empty;
}

public class ProfilUpdateDto
{
    public string? Nom { get; set; }
    public string? Prenom { get; set; }
    public string? Email { get; set; }
    public string? CodeApogee { get; set; }
    public string? Niveau { get; set; }
    public string? Filiere { get; set; }
}
