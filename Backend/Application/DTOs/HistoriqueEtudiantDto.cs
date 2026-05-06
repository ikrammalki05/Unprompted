namespace Application.DTOs;

public class HistoriqueEtudiantDto
{
    public int IdEtudiant { get; set; }
    
    // Les 3 listes qui composent la Timeline
    public List<ContributionItemDto> ContributionsGit { get; set; } = new();
    public List<PromptItemDto> InteractionsIa { get; set; } = new();
    public List<EvaluationItemDto> Evaluations { get; set; } = new();
}

// --- Les sous-modèles ---

public class ContributionItemDto
{
    public string MessageCommit { get; set; } = string.Empty;
    public DateTime? DateCommit { get; set; }
    public string NomProjet { get; set; } = string.Empty;
    public int LignesAjoutees { get; set; }
    public int LignesSupprimees { get; set; }
}

public class PromptItemDto
{
    public string Question { get; set; } = string.Empty;
    public DateTime? DateQuestion { get; set; }
    public string NomProjet { get; set; } = string.Empty;
    public int TokensConsommes { get; set; }
}

public class EvaluationItemDto
{
    public decimal? Note { get; set; }
    public string Commentaire { get; set; } = string.Empty;
    public DateTime? DateEvaluation { get; set; }
    public string NomProjet { get; set; } = string.Empty;
    public string NomEnseignant { get; set; } = string.Empty;
}