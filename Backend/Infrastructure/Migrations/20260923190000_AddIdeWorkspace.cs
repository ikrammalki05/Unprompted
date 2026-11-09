using Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260923190000_AddIdeWorkspace")]
    public class AddIdeWorkspace : Migration
    {
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
                        name: "FK_Dossier_Projet",
                        column: x => x.id_projet,
                        principalTable: "Projet",
                        principalColumn: "id_projet");
                    table.ForeignKey(
                        name: "FK_Dossier_Parent",
                        column: x => x.dossier_parent_id,
                        principalTable: "Dossier",
                        principalColumn: "id_dossier",
                        onDelete: ReferentialAction.Restrict);
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
                    ContentHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
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
                        name: "FK_Fichier_Projet",
                        column: x => x.id_projet,
                        principalTable: "Projet",
                        principalColumn: "id_projet",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fichier_Dossier",
                        column: x => x.id_dossier,
                        principalTable: "Dossier",
                        principalColumn: "id_dossier");
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
                        name: "FK_FichierVersion_Fichier",
                        column: x => x.id_fichier,
                        principalTable: "Fichier",
                        principalColumn: "id_fichier",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionCode",
                columns: table => new
                {
                    id_execution = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    langage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    statut = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    sortie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_conteneur_docker = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    date_debut = table.Column<DateTime>(type: "datetime", nullable: false),
                    date_fin = table.Column<DateTime>(type: "datetime", nullable: true),
                    id_utilisateur = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    id_projet = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionCode", x => x.id_execution);
                    table.ForeignKey(
                        name: "FK_ExecutionCode_Projet",
                        column: x => x.id_projet,
                        principalTable: "Projet",
                        principalColumn: "id_projet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionTerminal",
                columns: table => new
                {
                    id_session = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    id_conteneur_docker = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    id_utilisateur = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    date_creation = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionTerminal", x => x.id_session);
                });

            migrationBuilder.CreateIndex("IX__Dossier__IdProjet", "Dossier", "id_projet");
            migrationBuilder.CreateIndex("IX__Dossier__DossierParentId", "Dossier", "dossier_parent_id");
            migrationBuilder.CreateIndex("IX_Fichier_IdProjet", "Fichier", "id_projet");
            migrationBuilder.CreateIndex("IX_Fichier_IdDossier", "Fichier", "id_dossier");
            migrationBuilder.CreateIndex("IX_FichierVersion_IdFichier", "FichierVersion", "id_fichier");
            migrationBuilder.CreateIndex("IX_ExecutionCode_IdProjet", "ExecutionCode", "id_projet");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "SessionTerminal");
            migrationBuilder.DropTable(name: "ExecutionCode");
            migrationBuilder.DropTable(name: "FichierVersion");
            migrationBuilder.DropTable(name: "Fichier");
            migrationBuilder.DropTable(name: "Dossier");
        }
    }
}
