using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFileSystemEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dossier",
                columns: table => new
                {
                    id_dossier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    id_projet = table.Column<int>(type: "int", nullable: false),
                    dossier_parent_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Dossier__3214EC27C9A8B1F0", x => x.id_dossier);
                    table.ForeignKey(
                        name: "FK_Dossier_Dossier_dossier_parent_id",
                        column: x => x.dossier_parent_id,
                        principalTable: "Dossier",
                        principalColumn: "id_dossier",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dossier_Projet_id_projet",
                        column: x => x.id_projet,
                        principalTable: "Projet",
                        principalColumn: "id_projet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fichier",
                columns: table => new
                {
                    id_fichier = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    extension = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    language = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    contenu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    version = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    last_modified = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    id_projet = table.Column<int>(type: "int", nullable: false),
                    id_dossier = table.Column<int>(type: "int", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fichier", x => x.id_fichier);
                    table.ForeignKey(
                        name: "FK_Fichier_Dossier_id_dossier",
                        column: x => x.id_dossier,
                        principalTable: "Dossier",
                        principalColumn: "id_dossier");
                    table.ForeignKey(
                        name: "FK_Fichier_Projet_id_projet",
                        column: x => x.id_projet,
                        principalTable: "Projet",
                        principalColumn: "id_projet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FichierVersion",
                columns: table => new
                {
                    id_fichier_version = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_fichier = table.Column<int>(type: "int", nullable: false),
                    contenu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    version = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichierVersion", x => x.id_fichier_version);
                    table.ForeignKey(
                        name: "FK_FichierVersion_Fichier_id_fichier",
                        column: x => x.id_fichier,
                        principalTable: "Fichier",
                        principalColumn: "id_fichier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX__Dossier__DossierParentId",
                table: "Dossier",
                column: "dossier_parent_id");

            migrationBuilder.CreateIndex(
                name: "IX__Dossier__IdProjet",
                table: "Dossier",
                column: "id_projet");

            migrationBuilder.CreateIndex(
                name: "IX_Fichier_id_dossier",
                table: "Fichier",
                column: "id_dossier");

            migrationBuilder.CreateIndex(
                name: "IX_Fichier_id_projet",
                table: "Fichier",
                column: "id_projet");

            migrationBuilder.CreateIndex(
                name: "IX_FichierVersion_id_fichier",
                table: "FichierVersion",
                column: "id_fichier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FichierVersion");

            migrationBuilder.DropTable(
                name: "Fichier");

            migrationBuilder.DropTable(
                name: "Dossier");
        }
    }
}
