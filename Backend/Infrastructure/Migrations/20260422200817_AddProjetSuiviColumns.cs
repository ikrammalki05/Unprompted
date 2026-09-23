using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjetSuiviColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "notes_enseignant",
                table: "Projet",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "progression",
                table: "Projet",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Classe",
                columns: table => new
                {
                    IdClasse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomClasse = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AnneeAcademique = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EffectifMax = table.Column<int>(type: "int", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classe", x => x.IdClasse);
                });

            migrationBuilder.CreateTable(
                name: "EnseignantClasse",
                columns: table => new
                {
                    IdEnseignantClasse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEnseignant = table.Column<int>(type: "int", nullable: false),
                    IdClasse = table.Column<int>(type: "int", nullable: false),
                    DateAffectation = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnseignantClasse", x => x.IdEnseignantClasse);
                    table.ForeignKey(
                        name: "FK_EnseignantClasse_Classe",
                        column: x => x.IdClasse,
                        principalTable: "Classe",
                        principalColumn: "IdClasse",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnseignantClasse_Enseignant",
                        column: x => x.IdEnseignant,
                        principalTable: "Enseignant",
                        principalColumn: "id_enseignant",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnseignantClasse_IdClasse",
                table: "EnseignantClasse",
                column: "IdClasse");

            migrationBuilder.CreateIndex(
                name: "IX_EnseignantClasse_IdEnseignant",
                table: "EnseignantClasse",
                column: "IdEnseignant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnseignantClasse");

            migrationBuilder.DropTable(
                name: "Classe");

            migrationBuilder.DropColumn(
                name: "notes_enseignant",
                table: "Projet");

            migrationBuilder.DropColumn(
                name: "progression",
                table: "Projet");
        }
    }
}
