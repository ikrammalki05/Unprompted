namespace Application.DTOs;

// 1. DTO pour la lecture (Dashboard Enseignant)
public class PromptDto
{
    public int IdPrompt { get; set; }
    public string Contenu { get; set; } = string.Empty;
    public DateTime? DatePrompt { get; set; }
    public int? NbTokensEntree { get; set; }
    public int? NbTokensSortie { get; set; }
    public string NomEtudiant { get; set; } = string.Empty;
    
    // On inclut directement la/les réponse(s) associée(s)
    public List<ReponseIumDto> Reponses { get; set; } = new();

    public int IdEtudiant { get; set; }
    public int IdProjet { get; set; }
}

public class ReponseIumDto
{
    public int IdReponse { get; set; }
    public string? ContenuReponse { get; set; }
    public DateTime? DateReponse { get; set; }
    public string? ModeleIa { get; set; }
}

// 2. DTO pour l'écriture (Quand un étudiant utilise l'IA)
public class PromptLogRequestDto
{
    public int IdEtudiant { get; set; }
    public int IdProjet { get; set; }
    public string ContenuPrompt { get; set; } = string.Empty;
    public string ContenuReponse { get; set; } = string.Empty;
    public int NbTokensEntree { get; set; }
    public int NbTokensSortie { get; set; }
    public string ModeleIa { get; set; } = "gpt-4"; // Par défaut
}