from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import getSampleStyleSheet, ParagraphStyle
from reportlab.lib.units import cm
from reportlab.lib.colors import HexColor
from reportlab.platypus import SimpleDocTemplate, Paragraph, Spacer, HRFlowable, Table, TableStyle
from reportlab.lib.enums import TA_CENTER, TA_JUSTIFY
import os

out = os.path.join(os.path.dirname(__file__), "Cahier_des_charges_Developpement_Web.pdf")

doc = SimpleDocTemplate(
    out,
    pagesize=A4,
    leftMargin=2 * cm,
    rightMargin=2 * cm,
    topMargin=1.8 * cm,
    bottomMargin=1.8 * cm,
)

styles = getSampleStyleSheet()
title = ParagraphStyle(
    "TitleFR",
    parent=styles["Title"],
    fontSize=18,
    textColor=HexColor("#021124"),
    spaceAfter=6,
    leading=22,
)
subtitle = ParagraphStyle(
    "SubFR",
    parent=styles["Normal"],
    fontSize=11,
    textColor=HexColor("#475569"),
    alignment=TA_CENTER,
    spaceAfter=16,
)
h1 = ParagraphStyle(
    "H1FR",
    parent=styles["Heading1"],
    fontSize=13,
    textColor=HexColor("#1e3a8a"),
    spaceBefore=14,
    spaceAfter=8,
    leading=16,
)
body = ParagraphStyle(
    "BodyFR",
    parent=styles["Normal"],
    fontSize=10,
    leading=14,
    alignment=TA_JUSTIFY,
    spaceAfter=6,
)
bullet = ParagraphStyle(
    "BulletFR",
    parent=styles["Normal"],
    fontSize=10,
    leading=13,
    leftIndent=8,
    spaceAfter=3,
)
meta = ParagraphStyle(
    "MetaFR",
    parent=styles["Normal"],
    fontSize=9,
    textColor=HexColor("#64748b"),
    leading=12,
)

story = []
story.append(Paragraph("CAHIER DES CHARGES", title))
story.append(Paragraph("Projet academique — Developpement Web", subtitle))
story.append(HRFlowable(width="100%", thickness=1, color=HexColor("#a6c1ee"), spaceAfter=12))

info = [
    ["Titre du projet", "Developpement Web — Plateforme collaborative Unprompted"],
    ["Contexte", "Cycle d'ingenierie / Projet academique"],
    ["Public cible", "Etudiants en developpement logiciel"],
    ["Encadrement", "Enseignant responsable du module"],
    ["Duree indicative", "6 semaines"],
]
t = Table(info, colWidths=[4.5 * cm, 12 * cm])
t.setStyle(
    TableStyle(
        [
            ("FONTNAME", (0, 0), (0, -1), "Helvetica-Bold"),
            ("FONTSIZE", (0, 0), (-1, -1), 9),
            ("TEXTCOLOR", (0, 0), (0, -1), HexColor("#021124")),
            ("TEXTCOLOR", (1, 0), (1, -1), HexColor("#334155")),
            ("VALIGN", (0, 0), (-1, -1), "TOP"),
            ("BOTTOMPADDING", (0, 0), (-1, -1), 4),
            ("TOPPADDING", (0, 0), (-1, -1), 4),
            ("BACKGROUND", (0, 0), (-1, -1), HexColor("#f8fafc")),
            ("BOX", (0, 0), (-1, -1), 0.5, HexColor("#e2e8f0")),
            ("INNERGRID", (0, 0), (-1, -1), 0.25, HexColor("#e2e8f0")),
        ]
    )
)
story.append(t)
story.append(Spacer(1, 10))

story.append(Paragraph("1. Contexte et problematique", h1))
story.append(
    Paragraph(
        "Dans le cadre de la plateforme Unprompted, les etudiants doivent realiser une application web "
        "permettant de collaborer sur un projet, gerer des livrables et interagir avec un assistant IA "
        "encadre (RAG). Le present cahier des charges definit le perimetre fonctionnel, les contraintes "
        "techniques et les criteres d'evaluation du module Developpement Web.",
        body,
    )
)

story.append(Paragraph("2. Objectifs du projet", h1))
for item in [
    "Concevoir et developper une application web full-stack responsive.",
    "Mettre en place une authentification securisee et une gestion des roles (etudiant / enseignant).",
    "Implementer un espace de travail collaboratif (fichiers, editeur, historique).",
    "Integrer un assistant IA gouverne s'appuyant sur le cahier des charges (mode RAG).",
    "Documenter l'architecture, les choix techniques et le deploiement local.",
]:
    story.append(Paragraph(f"• {item}", bullet))

story.append(Paragraph("3. Perimetre fonctionnel", h1))
story.append(Paragraph("<b>3.1 Authentification et profils</b>", body))
for item in [
    "Connexion via un fournisseur d'identite (Keycloak / OpenID Connect).",
    "Profil utilisateur editable (nom, filiere, niveau).",
    "Redirection selon le role apres authentification.",
]:
    story.append(Paragraph(f"• {item}", bullet))

story.append(Paragraph("<b>3.2 Gestion de projet</b>", body))
for item in [
    "L'enseignant cree un projet, definit l'echeance et assigne des etudiants.",
    "L'etudiant consulte le cahier des charges et l'etat d'avancement.",
    "Suivi de progression et notes d'encadrement visibles cote enseignant.",
]:
    story.append(Paragraph(f"• {item}", bullet))

story.append(Paragraph("<b>3.3 Espace de travail (IDE leger)</b>", body))
for item in [
    "Arborescence de fichiers/dossiers liee au projet.",
    "Editeur de code avec sauvegarde automatique.",
    "Execution de scripts (Python / JavaScript) et terminal integre.",
    "Enregistrement de commits (message, statistiques de lignes).",
]:
    story.append(Paragraph(f"• {item}", bullet))

story.append(Paragraph("<b>3.4 Assistant IA (RAG)</b>", body))
for item in [
    "L'enseignant importe le cahier des charges PDF et configure le niveau d'acces IA.",
    "Niveaux : illimite, limite (quota), restreint (reponses uniquement a partir du document).",
    "Chaque question/reponse est journalisee pour l'analyse pedagogique.",
]:
    story.append(Paragraph(f"• {item}", bullet))

story.append(Paragraph("4. Exigences non fonctionnelles", h1))
for item in [
    "Performance : pages principales < 2 s en local.",
    "Securite : JWT, controle d'acces par role, pas de secrets dans le frontend.",
    "Maintenabilite : architecture en couches (API .NET, frontend React, service AI Python).",
    "Disponibilite locale : lancement sans Docker pour le parcours de demonstration.",
]:
    story.append(Paragraph(f"• {item}", bullet))

story.append(Paragraph("5. Stack technique recommandee", h1))
for item in [
    "Frontend : React + Vite + TypeScript.",
    "Backend : ASP.NET Core 8 (API REST).",
    "Authentification : Keycloak (realm unprompted, client react-frontend).",
    "Base de donnees : SQL Server.",
    "IA : service FastAPI + ChromaDB + LLM pour le RAG.",
]:
    story.append(Paragraph(f"• {item}", bullet))

story.append(Paragraph("6. Livrables attendus", h1))
for item in [
    "Code source versionne (frontend + backend + service AI).",
    "Documentation README : installation locale et scenarios de test.",
    "Demonstration : creation projet, assignation, workspace, question RAG.",
    "Rapport court (5–8 pages) : architecture, difficultes, perspectives.",
]:
    story.append(Paragraph(f"• {item}", bullet))

story.append(Paragraph("7. Criteres d'evaluation", h1))
for item in [
    "Completude fonctionnelle (40 %) — auth, projet, workspace, RAG.",
    "Qualite du code et architecture (25 %).",
    "Respect de la gouvernance IA (15 %) — niveaux d'acces et tracabilite.",
    "Documentation et soutenance (20 %).",
]:
    story.append(Paragraph(f"• {item}", bullet))

story.append(Paragraph("8. Contraintes et regles", h1))
for item in [
    "Interdiction de hardcoder des donnees metier de demonstration en production pedagogique.",
    "En mode restreint, l'assistant ne doit pas inventer hors cahier des charges.",
    "Les commits et prompts doivent etre consultables par l'enseignant.",
    "Le PDF du cahier doit etre telechargeable par les etudiants assignes.",
]:
    story.append(Paragraph(f"• {item}", bullet))

story.append(Paragraph("9. Planning indicatif (6 semaines)", h1))
for item in [
    "S1 : authentification + modeles projet/etudiants.",
    "S2 : CRUD projet + assignation + cahier des charges.",
    "S3–S4 : workspace (fichiers, editeur, execution, commits).",
    "S5 : RAG + configuration des niveaux d'acces.",
    "S6 : tests, documentation, soutenance.",
]:
    story.append(Paragraph(f"• {item}", bullet))

story.append(Spacer(1, 16))
story.append(HRFlowable(width="100%", thickness=0.5, color=HexColor("#cbd5e1"), spaceAfter=8))
story.append(
    Paragraph(
        "Document de test Unprompted — a importer via Configuration IA (enseignant) "
        "pour activer le RAG sur le projet « developpement web ».",
        meta,
    )
)

doc.build(story)
print(out)
print("SIZE", os.path.getsize(out))
