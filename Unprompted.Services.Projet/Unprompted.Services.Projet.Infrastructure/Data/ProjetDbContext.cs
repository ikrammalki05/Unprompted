using Microsoft.EntityFrameworkCore;
using Unprompted.Services.Projet.Domain.Entities;

namespace Unprompted.Services.Projet.Infrastructure.Data;

public class ProjetDbContext : DbContext
{
    public ProjetDbContext(DbContextOptions<ProjetDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Projet> Projets { get; set; }
    public DbSet<Groupe> Groupes { get; set; }
    public DbSet<Affectation> Affectations { get; set; }
    public DbSet<ConfigurationIum> ConfigurationsIum { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration de l'entité Projet
        modelBuilder.Entity<Domain.Entities.Projet>(entity =>
        {
            entity.HasKey(e => e.IdProjet);
            entity.Property(e => e.Titre).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Statut).HasMaxLength(50);
            
            // L'indexation de IdEnseignant est recommandée pour les recherches rapides, 
            // mais il n'y a pas de contrainte de clé étrangère physique (FK) vers une table Enseignant !
            entity.HasIndex(e => e.IdEnseignant);
        });

        // Configuration de l'entité Groupe
        modelBuilder.Entity<Groupe>(entity =>
        {
            entity.HasKey(e => e.IdGroupe);
            entity.Property(e => e.NomGroupe).IsRequired().HasMaxLength(100);

            entity.HasOne(d => d.IdProjetNavigation)
                .WithMany(p => p.Groupes)
                .HasForeignKey(d => d.IdProjet)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration de l'entité Affectation
        modelBuilder.Entity<Affectation>(entity =>
        {
            entity.HasKey(e => e.IdAffectation);
            
            entity.HasIndex(e => e.IdEtudiant);
            entity.HasIndex(e => e.IdRole);

            entity.HasOne(d => d.IdGroupeNavigation)
                .WithMany(g => g.Affectations)
                .HasForeignKey(d => d.IdGroupe)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration de l'entité ConfigurationIum
        modelBuilder.Entity<ConfigurationIum>(entity =>
        {
            entity.HasKey(e => e.IdConfig);
            entity.Property(e => e.PeriodeQuota).HasMaxLength(50);

            entity.HasOne(d => d.IdProjetNavigation)
                .WithOne(p => p.ConfigurationIum)
                .HasForeignKey<ConfigurationIum>(d => d.IdProjet)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}