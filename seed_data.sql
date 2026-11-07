-- ============================================================================
-- Script de données de test — Unprompted Platform
-- Exécuter dans SQL Server Management Studio ou via sqlcmd
-- Base de données : Unprompted
-- ============================================================================

USE Unprompted;
GO

-- ══════════════════════════════════════════════════════════════════════════════
-- 1. AJOUT DES COLONNES MANQUANTES (si elles n'existent pas encore)
-- ══════════════════════════════════════════════════════════════════════════════

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Projet') AND name = 'Objectifs')
    ALTER TABLE Projet ADD Objectifs NVARCHAR(MAX) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Projet') AND name = 'Livrables')
    ALTER TABLE Projet ADD Livrables NVARCHAR(MAX) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Projet') AND name = 'CriteresEvaluation')
    ALTER TABLE Projet ADD CriteresEvaluation NVARCHAR(MAX) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Projet') AND name = 'TechnologiesRequises')
    ALTER TABLE Projet ADD TechnologiesRequises NVARCHAR(MAX) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Projet') AND name = 'Contraintes')
    ALTER TABLE Projet ADD Contraintes NVARCHAR(MAX) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Projet') AND name = 'RessourcesDisponibles')
    ALTER TABLE Projet ADD RessourcesDisponibles NVARCHAR(MAX) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Projet') AND name = 'CahierDesCharges')
    ALTER TABLE Projet ADD CahierDesCharges VARBINARY(MAX) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Projet') AND name = 'progression')
    ALTER TABLE Projet ADD progression INT DEFAULT 0;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Projet') AND name = 'notes_enseignant')
    ALTER TABLE Projet ADD notes_enseignant NVARCHAR(1000) NULL;
GO

-- ══════════════════════════════════════════════════════════════════════════════
-- 2. RÔLES
-- ══════════════════════════════════════════════════════════════════════════════

IF NOT EXISTS (SELECT 1 FROM Role WHERE nom_role = 'Frontend')
    INSERT INTO Role (nom_role) VALUES ('Frontend');
IF NOT EXISTS (SELECT 1 FROM Role WHERE nom_role = 'Backend')
    INSERT INTO Role (nom_role) VALUES ('Backend');
IF NOT EXISTS (SELECT 1 FROM Role WHERE nom_role = 'DevOps')
    INSERT INTO Role (nom_role) VALUES ('DevOps');
IF NOT EXISTS (SELECT 1 FROM Role WHERE nom_role = 'IA')
    INSERT INTO Role (nom_role) VALUES ('IA');
GO

-- ══════════════════════════════════════════════════════════════════════════════
-- 3. UTILISATEURS (1 enseignant + 3 étudiants)
-- ══════════════════════════════════════════════════════════════════════════════

-- Enseignant
IF NOT EXISTS (SELECT 1 FROM Utilisateur WHERE email = 'prof.amrani@univ.ma')
    INSERT INTO Utilisateur (nom, prenom, email, statut) 
    VALUES ('Amrani', 'Mohamed', 'prof.amrani@univ.ma', 'Actif');

-- Étudiants
IF NOT EXISTS (SELECT 1 FROM Utilisateur WHERE email = 'alami@etud.univ.m')
    INSERT INTO Utilisateur (nom, prenom, email, statut) 
    VALUES ('Alami', 'Youssef', 'alami@etud.univ.m', 'Actif');

IF NOT EXISTS (SELECT 1 FROM Utilisateur WHERE email = 'benali@etud.univ.ma')
    INSERT INTO Utilisateur (nom, prenom, email, statut) 
    VALUES ('Benali', 'Fatima', 'benali@etud.univ.ma', 'Actif');

IF NOT EXISTS (SELECT 1 FROM Utilisateur WHERE email = 'elkadi@etud.univ.ma')
    INSERT INTO Utilisateur (nom, prenom, email, statut) 
    VALUES ('El Kadi', 'Omar', 'elkadi@etud.univ.ma', 'Actif');
GO

-- ══════════════════════════════════════════════════════════════════════════════
-- 4. ENSEIGNANT
-- ══════════════════════════════════════════════════════════════════════════════

DECLARE @idUtilProf INT = (SELECT id_utilisateur FROM Utilisateur WHERE email = 'prof.amrani@univ.ma');
IF NOT EXISTS (SELECT 1 FROM Enseignant WHERE id_utilisateur = @idUtilProf)
    INSERT INTO Enseignant (specialite, departement, id_utilisateur) 
    VALUES ('Intelligence Artificielle', 'Informatique', @idUtilProf);
GO

-- ══════════════════════════════════════════════════════════════════════════════
-- 5. ÉTUDIANTS
-- ══════════════════════════════════════════════════════════════════════════════

DECLARE @idUtilAlami INT = (SELECT id_utilisateur FROM Utilisateur WHERE email = 'alami@etud.univ.m');
IF NOT EXISTS (SELECT 1 FROM Etudiant WHERE id_utilisateur = @idUtilAlami)
    INSERT INTO Etudiant (code_apogee, niveau, filiere, id_utilisateur) 
    VALUES ('APG2024001', 'M2', 'IA & Gouvernance', @idUtilAlami);

DECLARE @idUtilBenali INT = (SELECT id_utilisateur FROM Utilisateur WHERE email = 'benali@etud.univ.ma');
IF NOT EXISTS (SELECT 1 FROM Etudiant WHERE id_utilisateur = @idUtilBenali)
    INSERT INTO Etudiant (code_apogee, niveau, filiere, id_utilisateur) 
    VALUES ('APG2024002', 'M2', 'IA & Gouvernance', @idUtilBenali);

DECLARE @idUtilElkadi INT = (SELECT id_utilisateur FROM Utilisateur WHERE email = 'elkadi@etud.univ.ma');
IF NOT EXISTS (SELECT 1 FROM Etudiant WHERE id_utilisateur = @idUtilElkadi)
    INSERT INTO Etudiant (code_apogee, niveau, filiere, id_utilisateur) 
    VALUES ('APG2024003', 'M2', 'IA & Gouvernance', @idUtilElkadi);
GO

-- ══════════════════════════════════════════════════════════════════════════════
-- 6. PROJETS (3 sujets de recherche)
-- ══════════════════════════════════════════════════════════════════════════════

DECLARE @idEnseignant INT = (SELECT TOP 1 id_enseignant FROM Enseignant);

-- Projet 1 : Gouvernance IA
IF NOT EXISTS (SELECT 1 FROM Projet WHERE titre = 'Gouvernance IA dans le secteur public')
INSERT INTO Projet (titre, description, date_debut, date_fin, statut, duree, url_git, progression, notes_enseignant, id_enseignant,
    Objectifs, Livrables, CriteresEvaluation, TechnologiesRequises, Contraintes, RessourcesDisponibles)
VALUES (
    'Gouvernance IA dans le secteur public',
    N'Ce projet vise à analyser les cadres réglementaires et éthiques entourant le déploiement de l''intelligence artificielle dans les institutions publiques, afin de proposer des recommandations adaptées au contexte académique et gouvernemental.',
    '2025-05-05', '2025-06-25', 'En cours', 8,
    'https://github.com/unprompted/gouvernance-ia', 35,
    N'Bon démarrage, continuer sur la cartographie réglementaire.',
    @idEnseignant,
    N'["Cartographie des réglementations IA en vigueur (EU AI Act, RGPD) et de leur applicabilité aux systèmes d''information publics.","Développement d''un outil d''audit automatisé permettant d''évaluer la conformité des systèmes IA déployés.","Mise en place d''indicateurs de transparence algorithmique mesurables et reproductibles."]',
    N'["Rapport de cartographie réglementaire","Prototype d''outil d''audit","Tableau de bord de transparence algorithmique","Documentation technique complète"]',
    N'["Qualité de l''analyse réglementaire","Fonctionnalité de l''outil d''audit","Pertinence des indicateurs proposés","Qualité du code et de la documentation"]',
    N'["Python","React","FastAPI","PostgreSQL","Docker"]',
    N'["Respecter les délais de livraison","Le code doit être versionné sur Git","Les données utilisées doivent être anonymisées"]',
    N'["Accès à la documentation EU AI Act","Serveur de développement dédié","Compte GitHub Organisation"]'
);

-- Projet 2 : Optimisation LLM
IF NOT EXISTS (SELECT 1 FROM Projet WHERE titre = N'Optimisation des modèles de langage')
INSERT INTO Projet (titre, description, date_debut, date_fin, statut, duree, url_git, progression, notes_enseignant, id_enseignant,
    Objectifs, Livrables, CriteresEvaluation, TechnologiesRequises, Contraintes, RessourcesDisponibles)
VALUES (
    N'Optimisation des modèles de langage',
    N'Ce projet vise à concevoir une architecture logicielle permettant le déploiement et l''exécution de Large Language Models (LLM) sur des infrastructures locales restreintes, tout en garantissant des performances de latence et de confidentialité optimales.',
    '2025-05-12', '2025-06-30', 'En attente', 7,
    'https://github.com/unprompted/optim-llm', 0,
    NULL,
    @idEnseignant,
    N'["Mise en œuvre de techniques de quantification avancées (4-bit, 8-bit) pour réduire l''empreinte mémoire.","Développement d''une API de service asynchrone pour la gestion des requêtes simultanées.","Optimisation des couches d''attention via FlashAttention-2 pour accélérer le traitement des contextes longs."]',
    N'["API d''inférence conteneurisée","Module de quantification automatique","Suite de benchmarks comparatifs","Documentation d''architecture"]',
    N'["Performance du modèle quantifié vs baseline","Latence des requêtes API","Qualité de la documentation technique","Respect des bonnes pratiques DevOps"]',
    N'["Python","PyTorch","ONNX","Docker","Kubernetes","FastAPI"]',
    N'["GPU limité (RTX 3060 12GB)","Pas d''accès au cloud pour l''inférence","Code 100% reproductible"]',
    N'["Accès au cluster GPU du laboratoire","Modèles pré-entraînés (Llama 2, Mistral)","Documentation FlashAttention-2"]'
);

-- Projet 3 : Sécurisation APIs
IF NOT EXISTS (SELECT 1 FROM Projet WHERE titre = N'Sécurisation des APIs Locales')
INSERT INTO Projet (titre, description, date_debut, date_fin, statut, duree, url_git, progression, notes_enseignant, id_enseignant,
    Objectifs, Livrables, CriteresEvaluation, TechnologiesRequises, Contraintes, RessourcesDisponibles)
VALUES (
    N'Sécurisation des APIs Locales',
    N'Ce projet porte sur la conception et la mise en œuvre de protocoles de sécurité robustes pour les APIs d''inférence déployées en local, afin de protéger les données sensibles et d''assurer la conformité aux standards de cybersécurité académiques.',
    '2025-06-01', '2025-07-15', 'En attente', 6,
    'https://github.com/unprompted/securite-api', 0,
    NULL,
    @idEnseignant,
    N'["Implémentation d''un mécanisme d''authentification mutuelle (mTLS) entre les services d''inférence et les clients.","Développement d''un système de détection d''intrusion basé sur l''analyse comportementale des requêtes API.","Chiffrement de bout en bout des flux de données entre les composants du système distribué."]',
    N'["Module d''authentification mTLS","Système de détection d''anomalies","Dashboard de monitoring sécurité","Rapport d''audit de sécurité"]',
    N'["Robustesse du mécanisme d''authentification","Précision de la détection d''intrusion","Couverture du chiffrement","Qualité du rapport d''audit"]',
    N'["Go","Rust","Nginx","Prometheus","Grafana","Docker"]',
    N'["Aucune dépendance externe pour l''authentification","Conformité OWASP Top 10","Tests de pénétration obligatoires"]',
    N'["Certificats TLS de test","Environnement de staging isolé","Documentation OWASP"]'
);
GO

-- ══════════════════════════════════════════════════════════════════════════════
-- 7. GROUPES (1 groupe par projet)
-- ══════════════════════════════════════════════════════════════════════════════

DECLARE @idProjet1 INT = (SELECT id_projet FROM Projet WHERE titre = 'Gouvernance IA dans le secteur public');
DECLARE @idProjet2 INT = (SELECT id_projet FROM Projet WHERE titre = N'Optimisation des modèles de langage');
DECLARE @idProjet3 INT = (SELECT id_projet FROM Projet WHERE titre = N'Sécurisation des APIs Locales');

IF NOT EXISTS (SELECT 1 FROM Groupe WHERE nom_groupe = 'Groupe Alpha' AND id_projet = @idProjet1)
    INSERT INTO Groupe (nom_groupe, id_projet) VALUES ('Groupe Alpha', @idProjet1);

IF NOT EXISTS (SELECT 1 FROM Groupe WHERE nom_groupe = 'Groupe Beta' AND id_projet = @idProjet2)
    INSERT INTO Groupe (nom_groupe, id_projet) VALUES ('Groupe Beta', @idProjet2);

IF NOT EXISTS (SELECT 1 FROM Groupe WHERE nom_groupe = 'Groupe Gamma' AND id_projet = @idProjet3)
    INSERT INTO Groupe (nom_groupe, id_projet) VALUES ('Groupe Gamma', @idProjet3);
GO

-- ══════════════════════════════════════════════════════════════════════════════
-- 8. AFFECTATIONS (lier étudiants aux groupes avec des rôles)
-- ══════════════════════════════════════════════════════════════════════════════

DECLARE @idAlami    INT = (SELECT id_etudiant FROM Etudiant WHERE code_apogee = 'APG2024001');
DECLARE @idBenali   INT = (SELECT id_etudiant FROM Etudiant WHERE code_apogee = 'APG2024002');
DECLARE @idElkadi   INT = (SELECT id_etudiant FROM Etudiant WHERE code_apogee = 'APG2024003');
DECLARE @idEnseign  INT = (SELECT TOP 1 id_enseignant FROM Enseignant);

DECLARE @roleBackend  INT = (SELECT id_role FROM Role WHERE nom_role = 'Backend');
DECLARE @roleFrontend INT = (SELECT id_role FROM Role WHERE nom_role = 'Frontend');
DECLARE @roleIA       INT = (SELECT id_role FROM Role WHERE nom_role = 'IA');
DECLARE @roleDevOps   INT = (SELECT id_role FROM Role WHERE nom_role = 'DevOps');

DECLARE @grp1 INT = (SELECT id_groupe FROM Groupe WHERE nom_groupe = 'Groupe Alpha');
DECLARE @grp2 INT = (SELECT id_groupe FROM Groupe WHERE nom_groupe = 'Groupe Beta');
DECLARE @grp3 INT = (SELECT id_groupe FROM Groupe WHERE nom_groupe = 'Groupe Gamma');

-- Groupe Alpha (Projet 1) : Alami=Backend, Benali=Frontend, Elkadi=IA
IF NOT EXISTS (SELECT 1 FROM Affectation WHERE id_etudiant = @idAlami AND id_groupe = @grp1)
    INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant) VALUES (@idAlami, @grp1, @roleBackend, @idEnseign);
IF NOT EXISTS (SELECT 1 FROM Affectation WHERE id_etudiant = @idBenali AND id_groupe = @grp1)
    INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant) VALUES (@idBenali, @grp1, @roleFrontend, @idEnseign);
IF NOT EXISTS (SELECT 1 FROM Affectation WHERE id_etudiant = @idElkadi AND id_groupe = @grp1)
    INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant) VALUES (@idElkadi, @grp1, @roleIA, @idEnseign);

-- Groupe Beta (Projet 2) : Alami=Backend, Benali=Frontend, Elkadi=DevOps
IF NOT EXISTS (SELECT 1 FROM Affectation WHERE id_etudiant = @idAlami AND id_groupe = @grp2)
    INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant) VALUES (@idAlami, @grp2, @roleBackend, @idEnseign);
IF NOT EXISTS (SELECT 1 FROM Affectation WHERE id_etudiant = @idBenali AND id_groupe = @grp2)
    INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant) VALUES (@idBenali, @grp2, @roleFrontend, @idEnseign);
IF NOT EXISTS (SELECT 1 FROM Affectation WHERE id_etudiant = @idElkadi AND id_groupe = @grp2)
    INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant) VALUES (@idElkadi, @grp2, @roleDevOps, @idEnseign);

-- Groupe Gamma (Projet 3) : Alami=Backend, Benali=Frontend, Elkadi=DevOps
IF NOT EXISTS (SELECT 1 FROM Affectation WHERE id_etudiant = @idAlami AND id_groupe = @grp3)
    INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant) VALUES (@idAlami, @grp3, @roleBackend, @idEnseign);
IF NOT EXISTS (SELECT 1 FROM Affectation WHERE id_etudiant = @idBenali AND id_groupe = @grp3)
    INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant) VALUES (@idBenali, @grp3, @roleFrontend, @idEnseign);
IF NOT EXISTS (SELECT 1 FROM Affectation WHERE id_etudiant = @idElkadi AND id_groupe = @grp3)
    INSERT INTO Affectation (id_etudiant, id_groupe, id_role, id_enseignant) VALUES (@idElkadi, @grp3, @roleDevOps, @idEnseign);
GO

-- ══════════════════════════════════════════════════════════════════════════════
-- VÉRIFICATION
-- ══════════════════════════════════════════════════════════════════════════════

PRINT '=== Vérification des données insérées ===';
SELECT 'Utilisateurs' AS [Table], COUNT(*) AS [Count] FROM Utilisateur
UNION ALL SELECT 'Enseignants', COUNT(*) FROM Enseignant
UNION ALL SELECT 'Etudiants', COUNT(*) FROM Etudiant
UNION ALL SELECT 'Projets', COUNT(*) FROM Projet
UNION ALL SELECT 'Groupes', COUNT(*) FROM Groupe
UNION ALL SELECT 'Affectations', COUNT(*) FROM Affectation
UNION ALL SELECT 'Roles', COUNT(*) FROM Role;

PRINT '=== Projets avec détails ===';
SELECT p.id_projet, p.titre, p.statut, p.progression, 
       e.specialite AS [Enseignant Spécialité]
FROM Projet p
LEFT JOIN Enseignant e ON p.id_enseignant = e.id_enseignant;

PRINT '=== Affectations par projet ===';
SELECT p.titre AS [Projet], g.nom_groupe AS [Groupe],
       u.prenom + ' ' + u.nom AS [Étudiant], r.nom_role AS [Rôle]
FROM Affectation a
JOIN Etudiant et ON a.id_etudiant = et.id_etudiant
JOIN Utilisateur u ON et.id_utilisateur = u.id_utilisateur
JOIN Groupe g ON a.id_groupe = g.id_groupe
JOIN Projet p ON g.id_projet = p.id_projet
JOIN Role r ON a.id_role = r.id_role
ORDER BY p.titre, r.nom_role;
GO
