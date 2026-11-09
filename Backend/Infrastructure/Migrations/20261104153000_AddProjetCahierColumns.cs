using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

public partial class AddProjetCahierColumns : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF COL_LENGTH('Projet', 'Objectifs') IS NULL
    ALTER TABLE [Projet] ADD [Objectifs] nvarchar(max) NULL;
IF COL_LENGTH('Projet', 'Livrables') IS NULL
    ALTER TABLE [Projet] ADD [Livrables] nvarchar(max) NULL;
IF COL_LENGTH('Projet', 'CriteresEvaluation') IS NULL
    ALTER TABLE [Projet] ADD [CriteresEvaluation] nvarchar(max) NULL;
IF COL_LENGTH('Projet', 'TechnologiesRequises') IS NULL
    ALTER TABLE [Projet] ADD [TechnologiesRequises] nvarchar(max) NULL;
IF COL_LENGTH('Projet', 'Contraintes') IS NULL
    ALTER TABLE [Projet] ADD [Contraintes] nvarchar(max) NULL;
IF COL_LENGTH('Projet', 'RessourcesDisponibles') IS NULL
    ALTER TABLE [Projet] ADD [RessourcesDisponibles] nvarchar(max) NULL;
IF COL_LENGTH('Projet', 'CahierDesCharges') IS NULL
    ALTER TABLE [Projet] ADD [CahierDesCharges] varbinary(max) NULL;
");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
IF COL_LENGTH('Projet', 'Objectifs') IS NOT NULL
    ALTER TABLE [Projet] DROP COLUMN [Objectifs];
IF COL_LENGTH('Projet', 'Livrables') IS NOT NULL
    ALTER TABLE [Projet] DROP COLUMN [Livrables];
IF COL_LENGTH('Projet', 'CriteresEvaluation') IS NOT NULL
    ALTER TABLE [Projet] DROP COLUMN [CriteresEvaluation];
IF COL_LENGTH('Projet', 'TechnologiesRequises') IS NOT NULL
    ALTER TABLE [Projet] DROP COLUMN [TechnologiesRequises];
IF COL_LENGTH('Projet', 'Contraintes') IS NOT NULL
    ALTER TABLE [Projet] DROP COLUMN [Contraintes];
IF COL_LENGTH('Projet', 'RessourcesDisponibles') IS NOT NULL
    ALTER TABLE [Projet] DROP COLUMN [RessourcesDisponibles];
IF COL_LENGTH('Projet', 'CahierDesCharges') IS NOT NULL
    ALTER TABLE [Projet] DROP COLUMN [CahierDesCharges];
");
    }
}
