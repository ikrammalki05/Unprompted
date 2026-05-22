using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Unprompted.Services.Projet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateMicroserviceProjet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projets",
                columns: table => new
                {
                    IdProjet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Statut = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CahierDesChargesPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdEnseignant = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projets", x => x.IdProjet);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationsIum",
                columns: table => new
                {
                    IdConfig = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotaRequetes = table.Column<int>(type: "int", nullable: true),
                    QuotaTokens = table.Column<int>(type: "int", nullable: true),
                    PeriodeQuota = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GenerationCodeAutorisee = table.Column<bool>(type: "bit", nullable: true),
                    IdProjet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationsIum", x => x.IdConfig);
                    table.ForeignKey(
                        name: "FK_ConfigurationsIum_Projets_IdProjet",
                        column: x => x.IdProjet,
                        principalTable: "Projets",
                        principalColumn: "IdProjet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groupes",
                columns: table => new
                {
                    IdGroupe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomGroupe = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdProjet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groupes", x => x.IdGroupe);
                    table.ForeignKey(
                        name: "FK_Groupes_Projets_IdProjet",
                        column: x => x.IdProjet,
                        principalTable: "Projets",
                        principalColumn: "IdProjet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Affectations",
                columns: table => new
                {
                    IdAffectation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdGroupe = table.Column<int>(type: "int", nullable: false),
                    IdEtudiant = table.Column<int>(type: "int", nullable: false),
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    DateAffectation = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Affectations", x => x.IdAffectation);
                    table.ForeignKey(
                        name: "FK_Affectations_Groupes_IdGroupe",
                        column: x => x.IdGroupe,
                        principalTable: "Groupes",
                        principalColumn: "IdGroupe",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Affectations_IdEtudiant",
                table: "Affectations",
                column: "IdEtudiant");

            migrationBuilder.CreateIndex(
                name: "IX_Affectations_IdGroupe",
                table: "Affectations",
                column: "IdGroupe");

            migrationBuilder.CreateIndex(
                name: "IX_Affectations_IdRole",
                table: "Affectations",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationsIum_IdProjet",
                table: "ConfigurationsIum",
                column: "IdProjet",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groupes_IdProjet",
                table: "Groupes",
                column: "IdProjet");

            migrationBuilder.CreateIndex(
                name: "IX_Projets_IdEnseignant",
                table: "Projets",
                column: "IdEnseignant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Affectations");

            migrationBuilder.DropTable(
                name: "ConfigurationsIum");

            migrationBuilder.DropTable(
                name: "Groupes");

            migrationBuilder.DropTable(
                name: "Projets");
        }
    }
}
