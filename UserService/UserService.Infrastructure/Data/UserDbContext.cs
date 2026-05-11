using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    // Tables liées aux utilisateurs UNIQUEMENT
    public virtual DbSet<Utilisateur> Utilisateurs { get; set; } = null!;
    public virtual DbSet<Etudiant> Etudiants { get; set; } = null!;
    public virtual DbSet<Enseignant> Enseignants { get; set; } = null!;
    public virtual DbSet<Admin> Admins { get; set; } = null!;
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<Affectation> Affectations { get; set; } = null!;
    public virtual DbSet<Classe> Classes { get; set; } = null!;
    public virtual DbSet<EnseignantClasse> EnseignantClasses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- UTILISATEUR ---
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
            entity.Property(e => e.Email).HasMaxLength(200).HasColumnName("email");
            entity.Property(e => e.Nom).HasMaxLength(100).HasColumnName("nom");
            entity.Property(e => e.Prenom).HasMaxLength(100).HasColumnName("prenom");
            entity.Property(e => e.Statut)
                .HasMaxLength(20)
                .HasDefaultValue("Actif")
                .HasColumnName("statut");
        });

        // --- ETUDIANT ---
        modelBuilder.Entity<Etudiant>(entity =>
        {
            entity.HasKey(e => e.IdEtudiant).HasName("PK__Etudiant__D1104AC72D94C259");
            entity.ToTable("Etudiant");
            entity.HasIndex(e => e.CodeApogee, "UQ__Etudiant__16C4CFE1D5FE85EE").IsUnique();
            entity.HasIndex(e => e.IdUtilisateur, "UQ__Etudiant__1A4FA5B9ABBF6F4D").IsUnique();
            entity.Property(e => e.IdEtudiant).HasColumnName("id_etudiant");
            entity.Property(e => e.CodeApogee).HasMaxLength(20).HasColumnName("code_apogee");
            entity.Property(e => e.Filiere).HasMaxLength(200).HasColumnName("filiere");
            entity.Property(e => e.IdUtilisateur).HasColumnName("id_utilisateur");
            entity.Property(e => e.Niveau).HasMaxLength(100).HasColumnName("niveau");
            entity.Property(e => e.IdClasse).HasColumnName("IdClasse");
            entity.HasOne(d => d.IdUtilisateurNavigation).WithOne(p => p.Etudiant)
                .HasForeignKey<Etudiant>(d => d.IdUtilisateur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Etudiant__id_uti__6C190EBB");

            entity.HasOne(d => d.IdClasseNavigation)
                .WithMany(p => p.Etudiants)
                .HasForeignKey(d => d.IdClasse)
                .HasConstraintName("FK_Etudiant_Classe");
        });

        // --- ENSEIGNANT ---
        modelBuilder.Entity<Enseignant>(entity =>
        {
            entity.HasKey(e => e.IdEnseignant).HasName("PK__Enseigna__CFF6D48646A0763C");
            entity.ToTable("Enseignant");
            entity.HasIndex(e => e.IdUtilisateur, "UQ__Enseigna__1A4FA5B9CC09AB17").IsUnique();
            entity.Property(e => e.IdEnseignant).HasColumnName("id_enseignant");
            entity.Property(e => e.Departement).HasMaxLength(200).HasColumnName("departement");
            entity.Property(e => e.IdUtilisateur).HasColumnName("id_utilisateur");
            entity.Property(e => e.Specialite).HasMaxLength(200).HasColumnName("specialite");
            entity.HasOne(d => d.IdUtilisateurNavigation).WithOne(p => p.Enseignant)
                .HasForeignKey<Enseignant>(d => d.IdUtilisateur)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Enseignan__id_ut__6754599E");
        });

        // --- ADMIN ---
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

        // --- ROLE ---
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Role__3D48441D528017A9");
            entity.ToTable("Role");
            entity.HasIndex(e => e.NomRole, "UQ__Role__95A62FB223DCB138").IsUnique();
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.NomRole).HasMaxLength(100).HasColumnName("nom_role");
        });

        // --- AFFECTATION ---
        modelBuilder.Entity<Affectation>(entity =>
        {
            entity.HasKey(e => e.IdAffectation).HasName("PK_Affectation");
            entity.ToTable("Affectation");
            entity.Property(e => e.IdAffectation).HasColumnName("IdAffectation");
            entity.Property(e => e.IdEtudiant).HasColumnName("IdEtudiant");
            entity.Property(e => e.IdEnseignant).HasColumnName("IdEnseignant");
            entity.Property(e => e.IdRole).HasColumnName("IdRole");
            entity.Property(e => e.IdGroupe).HasColumnName("IdGroupe");
            entity.Property(e => e.DateAffectation).HasColumnType("datetime").HasColumnName("DateAffectation");

            entity.HasOne(d => d.IdEtudiantNavigation)
                .WithMany(p => p.Affectations)
                .HasForeignKey(d => d.IdEtudiant)
                .HasConstraintName("FK_Affectation_Etudiant");

            entity.HasOne(d => d.IdEnseignantNavigation)
                .WithMany(p => p.Affectations)
                .HasForeignKey(d => d.IdEnseignant)
                .HasConstraintName("FK_Affectation_Enseignant");

            entity.HasOne(d => d.IdRoleNavigation)
                .WithMany(p => p.Affectations)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK_Affectation_Role");
        });

        // --- CLASSE ---
        modelBuilder.Entity<Classe>(entity =>
        {
            entity.HasKey(e => e.IdClasse).HasName("PK_Classe");
            entity.ToTable("Classe");
            entity.Property(e => e.IdClasse).HasColumnName("IdClasse");
            entity.Property(e => e.NomClasse).HasMaxLength(100).HasColumnName("NomClasse");
            entity.Property(e => e.AnneeAcademique).HasMaxLength(20).HasColumnName("AnneeAcademique");
            entity.Property(e => e.EffectifMax).HasColumnName("EffectifMax");
            entity.Property(e => e.DateCreation).HasColumnType("datetime").HasColumnName("DateCreation");
        });

        // --- ENSEIGNANT CLASSE ---
        modelBuilder.Entity<EnseignantClasse>(entity =>
        {
            entity.HasKey(e => e.IdEnseignantClasse);
            entity.ToTable("EnseignantClasse");
            entity.Property(e => e.IdEnseignantClasse).HasColumnName("IdEnseignantClasse");
            entity.Property(e => e.IdEnseignant).HasColumnName("IdEnseignant");
            entity.Property(e => e.IdClasse).HasColumnName("IdClasse");
            entity.Property(e => e.DateAffectation).HasColumnType("datetime").HasColumnName("DateAffectation");
            entity.HasOne(d => d.IdEnseignantNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdEnseignant)
                .HasConstraintName("FK_EnseignantClasse_Enseignant");
            entity.HasOne(d => d.IdClasseNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdClasse)
                .HasConstraintName("FK_EnseignantClasse_Classe");
        });
    }
}
