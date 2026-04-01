using System;
using System.Collections.Generic;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Affectation> Affectations { get; set; }

    public virtual DbSet<ConfigurationIum> ConfigurationIa { get; set; }

    public virtual DbSet<Contribution> Contributions { get; set; }

    public virtual DbSet<Enseignant> Enseignants { get; set; }

    public virtual DbSet<Etudiant> Etudiants { get; set; }

    public virtual DbSet<Evaluation> Evaluations { get; set; }

    public virtual DbSet<Groupe> Groupes { get; set; }

    public virtual DbSet<Projet> Projets { get; set; }

    public virtual DbSet<Prompt> Prompts { get; set; }

    public virtual DbSet<ReponseIum> ReponseIa { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Utilisateur> Utilisateurs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.IdAdmin).HasName("PK__Admin__89472E95E685A1FC");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.IdUtilisateur, "UQ__Admin__1A4FA5B90A300820").IsUnique();

            entity.Property(e => e.IdAdmin).HasColumnName("id_admin");
            entity.Property(e => e.IdUtilisateur).HasColumnName("id_utilisateur");

            entity.HasOne(d => d.IdUtilisateurNavigation).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.IdUtilisateur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Admin__id_utilis__6383C8BA");
        });

        modelBuilder.Entity<Affectation>(entity =>
        {
            entity.HasKey(e => e.IdAffectation).HasName("PK__Affectat__B0B9D1EED2DE9465");

            entity.ToTable("Affectation");

            entity.Property(e => e.IdAffectation).HasColumnName("id_affectation");
            entity.Property(e => e.DateAffectation)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_affectation");
            entity.Property(e => e.IdEnseignant).HasColumnName("id_enseignant");
            entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant");
            entity.Property(e => e.IdGroupe).HasColumnName("id_groupe");
            entity.Property(e => e.IdRole).HasColumnName("id_role");

            entity.HasOne(d => d.IdEnseignantNavigation).WithMany(p => p.Affectations)
                .HasForeignKey(d => d.IdEnseignant)
                .HasConstraintName("FK__Affectati__id_en__7C4F7684");

            entity.HasOne(d => d.IdEtudiantNavigation).WithMany(p => p.Affectations)
                .HasForeignKey(d => d.IdEtudiant)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Affectati__id_et__797309D9");

            entity.HasOne(d => d.IdGroupeNavigation).WithMany(p => p.Affectations)
                .HasForeignKey(d => d.IdGroupe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Affectati__id_gr__7A672E12");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Affectations)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Affectati__id_ro__7B5B524B");
        });

        modelBuilder.Entity<ConfigurationIum>(entity =>
        {
            entity.HasKey(e => e.IdConfig).HasName("PK__Configur__8F0A1FB2901856BE");

            entity.ToTable("Configuration_IA");

            entity.HasIndex(e => e.IdProjet, "UQ__Configur__36C5661C00B13D51").IsUnique();

            entity.Property(e => e.IdConfig).HasColumnName("id_config");
            entity.Property(e => e.DateConfiguration)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_configuration");
            entity.Property(e => e.GenerationCodeAutorisee)
                .HasDefaultValue(true)
                .HasColumnName("generation_code_autorisee");
            entity.Property(e => e.IdProjet).HasColumnName("id_projet");
            entity.Property(e => e.PeriodeQuota)
                .HasMaxLength(50)
                .HasColumnName("periode_quota");
            entity.Property(e => e.QuotaRequetes).HasColumnName("quota_requetes");
            entity.Property(e => e.QuotaTokens).HasColumnName("quota_tokens");

            entity.HasOne(d => d.IdProjetNavigation).WithOne(p => p.ConfigurationIum)
                .HasForeignKey<ConfigurationIum>(d => d.IdProjet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Configura__id_pr__18EBB532");
        });

        modelBuilder.Entity<Contribution>(entity =>
        {
            entity.HasKey(e => e.IdContribution).HasName("PK__Contribu__95E7F2CE51D9ADB7");

            entity.ToTable("Contribution");

            entity.HasIndex(e => e.HashCommit, "UQ__Contribu__6557D4D019C39649").IsUnique();

            entity.Property(e => e.IdContribution).HasColumnName("id_contribution");
            entity.Property(e => e.DateCommit)
                .HasColumnType("datetime")
                .HasColumnName("date_commit");
            entity.Property(e => e.HashCommit)
                .HasMaxLength(100)
                .HasColumnName("hash_commit");
            entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant");
            entity.Property(e => e.IdProjet).HasColumnName("id_projet");
            entity.Property(e => e.LignesAjoutees)
                .HasDefaultValue(0)
                .HasColumnName("lignes_ajoutees");
            entity.Property(e => e.LignesSupprimees)
                .HasDefaultValue(0)
                .HasColumnName("lignes_supprimees");
            entity.Property(e => e.MessageCommit)
                .HasMaxLength(500)
                .HasColumnName("message_commit");

            entity.HasOne(d => d.IdEtudiantNavigation).WithMany(p => p.Contributions)
                .HasForeignKey(d => d.IdEtudiant)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contribut__id_et__02084FDA");

            entity.HasOne(d => d.IdProjetNavigation).WithMany(p => p.Contributions)
                .HasForeignKey(d => d.IdProjet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Contribut__id_pr__02FC7413");
        });

        modelBuilder.Entity<Enseignant>(entity =>
        {
            entity.HasKey(e => e.IdEnseignant).HasName("PK__Enseigna__CFF6D48646A0763C");

            entity.ToTable("Enseignant");

            entity.HasIndex(e => e.IdUtilisateur, "UQ__Enseigna__1A4FA5B9CC09AB17").IsUnique();

            entity.Property(e => e.IdEnseignant).HasColumnName("id_enseignant");
            entity.Property(e => e.Departement)
                .HasMaxLength(200)
                .HasColumnName("departement");
            entity.Property(e => e.IdUtilisateur).HasColumnName("id_utilisateur");
            entity.Property(e => e.Specialite)
                .HasMaxLength(200)
                .HasColumnName("specialite");

            entity.HasOne(d => d.IdUtilisateurNavigation).WithOne(p => p.Enseignant)
                .HasForeignKey<Enseignant>(d => d.IdUtilisateur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Enseignan__id_ut__6754599E");
        });

        modelBuilder.Entity<Etudiant>(entity =>
        {
            entity.HasKey(e => e.IdEtudiant).HasName("PK__Etudiant__D1104AC72D94C259");

            entity.ToTable("Etudiant");

            entity.HasIndex(e => e.CodeApogee, "UQ__Etudiant__16C4CFE1D5FE85EE").IsUnique();

            entity.HasIndex(e => e.IdUtilisateur, "UQ__Etudiant__1A4FA5B9ABBF6F4D").IsUnique();

            entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant");
            entity.Property(e => e.CodeApogee)
                .HasMaxLength(20)
                .HasColumnName("code_apogee");
            entity.Property(e => e.Filiere)
                .HasMaxLength(200)
                .HasColumnName("filiere");
            entity.Property(e => e.IdUtilisateur).HasColumnName("id_utilisateur");
            entity.Property(e => e.Niveau)
                .HasMaxLength(100)
                .HasColumnName("niveau");

            entity.HasOne(d => d.IdUtilisateurNavigation).WithOne(p => p.Etudiant)
                .HasForeignKey<Etudiant>(d => d.IdUtilisateur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Etudiant__id_uti__6C190EBB");
        });

        modelBuilder.Entity<Evaluation>(entity =>
        {
            entity.HasKey(e => e.IdEvaluation).HasName("PK__Evaluati__0B77AA116C9AE613");

            entity.ToTable("Evaluation");

            entity.Property(e => e.IdEvaluation).HasColumnName("id_evaluation");
            entity.Property(e => e.Commentaire).HasColumnName("commentaire");
            entity.Property(e => e.DateEvaluation)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_evaluation");
            entity.Property(e => e.IdEnseignant).HasColumnName("id_enseignant");
            entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant");
            entity.Property(e => e.IdProjet).HasColumnName("id_projet");
            entity.Property(e => e.IndicateurDependanceIa)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("indicateur_dependance_ia");
            entity.Property(e => e.Note)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("note");

            entity.HasOne(d => d.IdEnseignantNavigation).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.IdEnseignant)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Evaluatio__id_en__1332DBDC");

            entity.HasOne(d => d.IdEtudiantNavigation).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.IdEtudiant)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Evaluatio__id_et__114A936A");

            entity.HasOne(d => d.IdProjetNavigation).WithMany(p => p.Evaluations)
                .HasForeignKey(d => d.IdProjet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Evaluatio__id_pr__123EB7A3");
        });

        modelBuilder.Entity<Groupe>(entity =>
        {
            entity.HasKey(e => e.IdGroupe).HasName("PK__Groupe__6E0AF8301A830A66");

            entity.ToTable("Groupe");

            entity.Property(e => e.IdGroupe).HasColumnName("id_groupe");
            entity.Property(e => e.IdProjet).HasColumnName("id_projet");
            entity.Property(e => e.NomGroupe)
                .HasMaxLength(200)
                .HasColumnName("nom_groupe");

            entity.HasOne(d => d.IdProjetNavigation).WithMany(p => p.Groupes)
                .HasForeignKey(d => d.IdProjet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Groupe__id_proje__72C60C4A");
        });

        modelBuilder.Entity<Projet>(entity =>
        {
            entity.HasKey(e => e.IdProjet).HasName("PK__Projet__36C5661D2CDF80AB");

            entity.ToTable("Projet");

            entity.Property(e => e.IdProjet).HasColumnName("id_projet");
            entity.Property(e => e.DateDebut).HasColumnName("date_debut");
            entity.Property(e => e.DateFin).HasColumnName("date_fin");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Duree).HasColumnName("duree");
            entity.Property(e => e.IdEnseignant).HasColumnName("id_enseignant");
            entity.Property(e => e.Statut)
                .HasMaxLength(50)
                .HasDefaultValue("En cours")
                .HasColumnName("statut");
            entity.Property(e => e.Titre)
                .HasMaxLength(300)
                .HasColumnName("titre");
            entity.Property(e => e.UrlGit)
                .HasMaxLength(500)
                .HasColumnName("url_git");

            entity.HasOne(d => d.IdEnseignantNavigation).WithMany(p => p.Projets)
                .HasForeignKey(d => d.IdEnseignant)
                .HasConstraintName("FK__Projet__id_ensei__6FE99F9F");
        });

        modelBuilder.Entity<Prompt>(entity =>
        {
            entity.HasKey(e => e.IdPrompt).HasName("PK__Prompt__368514A85D7E4526");

            entity.ToTable("Prompt");

            entity.Property(e => e.IdPrompt).HasColumnName("id_prompt");
            entity.Property(e => e.Contenu).HasColumnName("contenu");
            entity.Property(e => e.DatePrompt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_prompt");
            entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant");
            entity.Property(e => e.IdProjet).HasColumnName("id_projet");
            entity.Property(e => e.NbTokensEntree)
                .HasDefaultValue(0)
                .HasColumnName("nb_tokens_entree");
            entity.Property(e => e.NbTokensSortie)
                .HasDefaultValue(0)
                .HasColumnName("nb_tokens_sortie");

            entity.HasOne(d => d.IdEtudiantNavigation).WithMany(p => p.Prompts)
                .HasForeignKey(d => d.IdEtudiant)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prompt__id_etudi__08B54D69");

            entity.HasOne(d => d.IdProjetNavigation).WithMany(p => p.Prompts)
                .HasForeignKey(d => d.IdProjet)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prompt__id_proje__09A971A2");
        });

        modelBuilder.Entity<ReponseIum>(entity =>
        {
            entity.HasKey(e => e.IdReponse).HasName("PK__Reponse___861C0C2C2684B7FC");

            entity.ToTable("Reponse_IA");

            entity.Property(e => e.IdReponse).HasColumnName("id_reponse");
            entity.Property(e => e.ContenuReponse).HasColumnName("contenu_reponse");
            entity.Property(e => e.DateReponse)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_reponse");
            entity.Property(e => e.IdPrompt).HasColumnName("id_prompt");
            entity.Property(e => e.ModeleIa)
                .HasMaxLength(100)
                .HasColumnName("modele_ia");

            entity.HasOne(d => d.IdPromptNavigation).WithMany(p => p.ReponseIa)
                .HasForeignKey(d => d.IdPrompt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reponse_I__id_pr__0D7A0286");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Role__3D48441D528017A9");

            entity.ToTable("Role");

            entity.HasIndex(e => e.NomRole, "UQ__Role__95A62FB223DCB138").IsUnique();

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.NomRole)
                .HasMaxLength(100)
                .HasColumnName("nom_role");
        });

        modelBuilder.Entity<Utilisateur>(entity =>
        {
            entity.HasKey(e => e.IdUtilisateur).HasName("PK__Utilisat__1A4FA5B8AEDF0317");

            entity.ToTable("Utilisateur");

            entity.HasIndex(e => e.Email, "UQ__Utilisat__AB6E6164AF503755").IsUnique();

            entity.Property(e => e.IdUtilisateur).HasColumnName("id_utilisateur");
            entity.Property(e => e.DateCreation)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date_creation");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .HasColumnName("email");
            entity.Property(e => e.MotDePasse)
                .HasMaxLength(500)
                .HasColumnName("mot_de_passe");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .HasColumnName("nom");
            entity.Property(e => e.Prenom)
                .HasMaxLength(100)
                .HasColumnName("prenom");
            entity.Property(e => e.Statut)
                .HasMaxLength(20)
                .HasDefaultValue("Actif")
                .HasColumnName("statut");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
