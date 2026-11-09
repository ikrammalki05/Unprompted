namespace Application.DTOs;

public class AnalyticsEtudiantDto
{
    public int IdEtudiant { get; set; }
    public string NomEtudiant { get; set; } = string.Empty;
    
    // Métriques Git
    public int NombreCommits { get; set; }
    public int LignesModifiees { get; set; }
    
    // Métriques IA
    public int NombrePrompts { get; set; }
    public int TokensConsommes { get; set; }
    
    // Le verdict de l'algorithme
    public string NiveauDependance { get; set; } = string.Empty; // "Autonome", "Modéré", "Critique"
}