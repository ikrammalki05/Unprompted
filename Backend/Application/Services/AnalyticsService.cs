using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IPromptRepository _promptRepo;
    private readonly IContributionRepository _contributionRepo;

    public AnalyticsService(IPromptRepository promptRepo, IContributionRepository contributionRepo)
    {
        _promptRepo = promptRepo;
        _contributionRepo = contributionRepo;
    }

    public async Task<IEnumerable<AnalyticsEtudiantDto>> GenererRapportProjetAsync(int idProjet)
    {
        // 1. Récupérer toutes les données brutes
        var prompts = await _promptRepo.GetByProjetIdAsync(idProjet);
        var contributions = await _contributionRepo.GetByProjetIdAsync(idProjet);

        // 2. Extraire la liste unique de tous les étudiants ayant participé (Git ou IA)
        var etudiantsIds = prompts.Select(p => p.IdEtudiant)
            .Union(contributions.Select(c => c.IdEtudiant))
            .Distinct();

        var rapport = new List<AnalyticsEtudiantDto>();

        // 3. Calculer les statistiques pour chaque étudiant
        foreach (var idEtudiant in etudiantsIds)
        {
            // --- Agrégation Git ---
            var commitsEtudiant = contributions.Where(c => c.IdEtudiant == idEtudiant).ToList();
            int nbCommits = commitsEtudiant.Count;
            int lignesModifiees = commitsEtudiant.Sum(c => (c.LignesAjoutees ?? 0) + (c.LignesSupprimees ?? 0));
            
            // Récupération du nom (On le prend du premier commit ou prompt trouvé)
            var nomEtudiant = commitsEtudiant.FirstOrDefault()?.IdEtudiantNavigation?.IdUtilisateurNavigation != null
                ? $"{commitsEtudiant.First().IdEtudiantNavigation.IdUtilisateurNavigation.Prenom} {commitsEtudiant.First().IdEtudiantNavigation.IdUtilisateurNavigation.Nom}"
                : prompts.FirstOrDefault(p => p.IdEtudiant == idEtudiant)?.IdEtudiantNavigation?.IdUtilisateurNavigation != null
                    ? $"{prompts.First(p => p.IdEtudiant == idEtudiant).IdEtudiantNavigation.IdUtilisateurNavigation.Prenom} {prompts.First(p => p.IdEtudiant == idEtudiant).IdEtudiantNavigation.IdUtilisateurNavigation.Nom}"
                    : "Étudiant Inconnu";

            // --- Agrégation IA ---
            var promptsEtudiant = prompts.Where(p => p.IdEtudiant == idEtudiant).ToList();
            int nbPrompts = promptsEtudiant.Count;
            int tokens = promptsEtudiant.Sum(p => (p.NbTokensEntree ?? 0) + (p.NbTokensSortie ?? 0));

            // --- L'Algorithme de Dépendance ---
            string dependance = "Autonome";

            if (nbCommits == 0 && nbPrompts > 0)
            {
                // Beaucoup de questions IA, mais aucun code produit
                dependance = "Critique"; 
            }
            else if (nbCommits > 0)
            {
                // Ratio : Combien de tokens consommés pour écrire 1 ligne de code ?
                // Math.Max évite la division par zéro si un commit a 0 ligne modifiée
                double ratioTokensParLigne = (double)tokens / Math.Max(1, lignesModifiees);

                if (ratioTokensParLigne > 500)
                    dependance = "Critique"; // Plus de 500 tokens pour 1 ligne de code = Copier-Coller massif
                else if (ratioTokensParLigne > 150)
                    dependance = "Modéré";   // Utilisation régulière mais équilibrée
                else
                    dependance = "Autonome"; // Code principalement écrit à la main
            }

            rapport.Add(new AnalyticsEtudiantDto
            {
                IdEtudiant = idEtudiant,
                NomEtudiant = nomEtudiant,
                NombreCommits = nbCommits,
                LignesModifiees = lignesModifiees,
                NombrePrompts = nbPrompts,
                TokensConsommes = tokens,
                NiveauDependance = dependance
            });
        }

        // On trie par niveau de dépendance pour que le prof voie les cas critiques en premier
        return rapport.OrderByDescending(r => r.NiveauDependance == "Critique")
                      .ThenByDescending(r => r.NiveauDependance == "Modéré");
    }
}