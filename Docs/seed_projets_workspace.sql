/* Seed projets + workspace pour etudiant@unprompted.local — idempotent */
SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @id_etudiant INT = (SELECT TOP 1 id_etudiant FROM Etudiant WHERE id_utilisateur = (SELECT id_utilisateur FROM Utilisateur WHERE email = N'etudiant@unprompted.local'));
DECLARE @id_enseignant INT = (SELECT TOP 1 id_enseignant FROM Enseignant WHERE id_utilisateur = (SELECT id_utilisateur FROM Utilisateur WHERE email = N'enseignant@unprompted.local'));
DECLARE @id_role_membre INT = (SELECT TOP 1 id_role FROM Role WHERE nom_role = N'Membre');

IF @id_etudiant IS NULL OR @id_enseignant IS NULL OR @id_role_membre IS NULL
BEGIN
  RAISERROR(N'Utilisateur étudiant/enseignant ou rôle Membre manquant.', 16, 1);
  RETURN;
END;

BEGIN TRAN;

-- Enrichir / assigner le projet 1 existant
IF EXISTS (SELECT 1 FROM Projet WHERE id_projet = 1)
BEGIN
  UPDATE Projet SET
    titre = N'Plateforme Unprompted — Fondations',
    description = N'Mise en place des bases de la plateforme : auth, rôles, et premiers écrans.',
    statut = N'En cours',
    progression = 35,
    duree = 8,
    date_debut = CAST(GETDATE() AS date),
    date_fin = DATEADD(week, 8, CAST(GETDATE() AS date)),
    notes_enseignant = N'Prioriser le workspace et le RAG.',
    id_enseignant = @id_enseignant,
    Objectifs = N'Comprendre l architecture full-stack et livrer un MVP navigable.',
    Livrables = N'Dashboard étudiant, login Keycloak, API projets.',
    TechnologiesRequises = N'["React","ASP.NET","Keycloak","SQL Server"]'
  WHERE id_projet = 1;

  IF NOT EXISTS (SELECT 1 FROM Groupe WHERE id_projet = 1)
    INSERT INTO Groupe (nom_groupe, id_projet) VALUES (N'Groupe principal', 1);

  DECLARE @g1 INT = (SELECT TOP 1 id_groupe FROM Groupe WHERE id_projet = 1);
  IF NOT EXISTS (SELECT 1 FROM Affectation WHERE id_etudiant = @id_etudiant AND id_groupe = @g1)
    INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant, date_affectation)
    VALUES (@id_etudiant, @g1, @id_role_membre, @id_enseignant, GETDATE());

  IF NOT EXISTS (SELECT 1 FROM Dossier WHERE id_projet = 1 AND nom = N'src')
    INSERT INTO Dossier (nom, id_projet, dossier_parent_id, created_at) VALUES (N'src', 1, NULL, GETDATE());

  DECLARE @d1 INT = (SELECT TOP 1 id_dossier FROM Dossier WHERE id_projet = 1 AND nom = N'src');

  IF NOT EXISTS (SELECT 1 FROM Fichier WHERE id_projet = 1 AND nom = N'main' AND extension = N'.py')
    INSERT INTO Fichier (nom, extension, language, contenu, size, version, last_modified, id_projet, id_dossier, created_by, created_at)
    VALUES (N'main', N'.py', N'python',
N'# Projet 1 — Fondations Unprompted
def welcome(name: str) -> str:
    return f"Bienvenue {name} sur Unprompted"

if __name__ == "__main__":
    print(welcome("Ali"))
    print("Workspace opérationnel.")
', 180, 1, GETDATE(), 1, @d1, N'seed', GETDATE());

  IF NOT EXISTS (SELECT 1 FROM Fichier WHERE id_projet = 1 AND nom = N'README' AND extension = N'.md')
    INSERT INTO Fichier (nom, extension, language, contenu, size, version, last_modified, id_projet, id_dossier, created_by, created_at)
    VALUES (N'README', N'.md', N'markdown',
N'# Fondations Unprompted

Éditez `src/main.py`, lancez le terminal, puis testez l''assistant IA.
', 120, 1, GETDATE(), 1, NULL, N'seed', GETDATE());
END;

-- Enrichir projet 2 (déjà assigné) + corriger le fichier cassé
IF EXISTS (SELECT 1 FROM Projet WHERE id_projet = 2)
BEGIN
  UPDATE Projet SET
    titre = N'Développement Web — Unprompted IDE',
    description = N'IDE collaboratif : fichiers, exécution Python, terminal et commits.',
    statut = N'En cours',
    progression = 55,
    duree = 12,
    date_debut = CAST(GETDATE() AS date),
    date_fin = DATEADD(week, 12, CAST(GETDATE() AS date)),
    notes_enseignant = N'Valider autosave + run + terminal.',
    id_enseignant = @id_enseignant,
    Objectifs = N'Rendre l espace de travail pleinement fonctionnel.',
    Livrables = N'Arborescence, éditeur, run, terminal.',
    TechnologiesRequises = N'["React","Monaco","FastAPI","Python"]'
  WHERE id_projet = 2;

  UPDATE Fichier SET
    contenu = N'# Développement Web — Unprompted IDE
from utils import add

def main():
    print("Unprompted IDE OK")
    print("2 + 3 =", add(2, 3))

if __name__ == "__main__":
    main()
',
    language = N'python',
    last_modified = GETDATE()
  WHERE id_projet = 2 AND nom = N'main' AND extension = N'.py';

  UPDATE Fichier SET
    contenu = N'def add(a, b):
    """Addition simple pour tester l''exécution."""
    return a + b
',
    language = N'python',
    last_modified = GETDATE()
  WHERE id_projet = 2 AND nom = N'utils' AND extension = N'.py';

  UPDATE Fichier SET
    contenu = N'# Développement Web — Unprompted IDE

Fichiers de démo pour tester :
- ouverture / édition
- autosave
- Lancer (run)
- terminal
',
    last_modified = GETDATE()
  WHERE id_projet = 2 AND nom = N'README' AND extension = N'.md';
END;

-- Projet 3 : RAG / Assistant IA
IF NOT EXISTS (SELECT 1 FROM Projet WHERE titre = N'Assistant IA RAG — Cahier des charges')
BEGIN
  INSERT INTO Projet (
    titre, description, date_debut, date_fin, statut, duree, url_git,
    id_enseignant, notes_enseignant, progression,
    Objectifs, Livrables, TechnologiesRequises
  ) VALUES (
    N'Assistant IA RAG — Cahier des charges',
    N'Indexer un PDF et poser des questions contextualisées via le service IA.',
    CAST(GETDATE() AS date),
    DATEADD(week, 6, CAST(GETDATE() AS date)),
    N'En cours',
    6,
    N'https://github.com/unprompted/rag-demo',
    @id_enseignant,
    N'Tester /ask après indexation du PDF.',
    20,
    N'Comprendre le pipeline RAG (PDF → embeddings → LLM).',
    N'Upload cahier, questions tracées, réponses ancrées.',
    N'["FastAPI","Chroma","Groq","PDF"]'
  );

  DECLARE @p3 INT = SCOPE_IDENTITY();
  INSERT INTO Groupe (nom_groupe, id_projet) VALUES (N'Groupe principal', @p3);
  DECLARE @g3 INT = SCOPE_IDENTITY();
  INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant, date_affectation)
  VALUES (@id_etudiant, @g3, @id_role_membre, @id_enseignant, GETDATE());

  INSERT INTO Dossier (nom, id_projet, dossier_parent_id, created_at) VALUES (N'notebooks', @p3, NULL, GETDATE());
  DECLARE @d3 INT = SCOPE_IDENTITY();

  INSERT INTO Fichier (nom, extension, language, contenu, size, version, last_modified, id_projet, id_dossier, created_by, created_at)
  VALUES
  (N'ask_demo', N'.py', N'python',
N'# Demo locale — simulation d''une question RAG
question = "Quels sont les objectifs du projet Unprompted ?"
print("Question:", question)
print("Réponse (à obtenir via l''assistant IA du workspace).")
', 160, 1, GETDATE(), @p3, @d3, N'seed', GETDATE()),
  (N'README', N'.md', N'markdown',
N'# Assistant IA RAG

1. Ouvrir le workspace
2. Utiliser le panneau Assistant IA
3. Poser une question sur le cahier des charges
', 140, 1, GETDATE(), @p3, NULL, N'seed', GETDATE());
END;

-- Projet 4 : À venir
IF NOT EXISTS (SELECT 1 FROM Projet WHERE titre = N'Évaluation pédagogique & analytics')
BEGIN
  INSERT INTO Projet (
    titre, description, date_debut, date_fin, statut, duree,
    id_enseignant, notes_enseignant, progression,
    Objectifs, Livrables, TechnologiesRequises
  ) VALUES (
    N'Évaluation pédagogique & analytics',
    N'Suivi des commits, prompts IA et notes d''évaluation pour chaque étudiant.',
    DATEADD(week, 2, CAST(GETDATE() AS date)),
    DATEADD(week, 14, CAST(GETDATE() AS date)),
    N'A venir',
    10,
    @id_enseignant,
    N'Démarrer après stabilisation de l''IDE.',
    5,
    N'Relier historique, évaluation et dashboard enseignant.',
    N'Évaluations, historique d''activité, stats.',
    N'["React","ASP.NET","SQL Server"]'
  );

  DECLARE @p4 INT = SCOPE_IDENTITY();
  INSERT INTO Groupe (nom_groupe, id_projet) VALUES (N'Groupe principal', @p4);
  DECLARE @g4 INT = SCOPE_IDENTITY();
  INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant, date_affectation)
  VALUES (@id_etudiant, @g4, @id_role_membre, @id_enseignant, GETDATE());

  INSERT INTO Fichier (nom, extension, language, contenu, size, version, last_modified, id_projet, id_dossier, created_by, created_at)
  VALUES (N'stats', N'.py', N'python',
N'commits = 12
prompts_ia = 7
print(f"Activité: {commits} commits, {prompts_ia} prompts IA")
', 100, 1, GETDATE(), @p4, NULL, N'seed', GETDATE());
END;

COMMIT;

SELECT 'OK' AS result;
SELECT id_projet, titre, statut, progression FROM Projet ORDER BY id_projet;
SELECT a.id_affectation, a.id_etudiant, g.id_projet, p.titre
FROM Affectation a
JOIN Groupe g ON g.id_groupe = a.id_groupe
JOIN Projet p ON p.id_projet = g.id_projet
WHERE a.id_etudiant = @id_etudiant;
SELECT id_fichier, nom + extension AS file_name, id_projet FROM Fichier ORDER BY id_projet, id_fichier;
