using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id_role = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_role = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__3D48441D528017A9", x => x.id_role);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    prenom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    mot_de_passe = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    statut = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Actif"),
                    date_creation = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Utilisat__1A4FA5B8AEDF0317", x => x.id_utilisateur);
                });

            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    id_admin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_utilisateur = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Admin__89472E95E685A1FC", x => x.id_admin);
                    table.ForeignKey(
                        name: "FK__Admin__id_utilis__6383C8BA",
                        column: x => x.id_utilisateur,
                        principalTable: "Utilisateur",
                        principalColumn: "id_utilisateur");
                });

            migrationBuilder.CreateTable(
                name: "Enseignant",
                columns: table => new
                {
                    id_enseignant = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    specialite = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    departement = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    id_utilisateur = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Enseigna__CFF6D48646A0763C", x => x.id_enseignant);
                    table.ForeignKey(
                        name: "FK__Enseignan__id_ut__6754599E",
                        column: x => x.id_utilisateur,
                        principalTable: "Utilisateur",
                        principalColumn: "id_utilisateur");
                });

            migrationBuilder.CreateTable(
                name: "Etudiant",
                columns: table => new
                {
                    id_etudiant = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code_apogee = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    niveau = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    filiere = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    id_utilisateur = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Etudiant__D1104AC72D94C259", x => x.id_etudiant);
                    table.ForeignKey(
                        name: "FK__Etudiant__id_uti__6C190EBB",
                        column: x => x.id_utilisateur,
                        principalTable: "Utilisateur",
                        principalColumn: "id_utilisateur");
                });

            migrationBuilder.CreateTable(
                name: "Projet",
                columns: table => new
                {
                    id_projet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titre = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_debut = table.Column<DateOnly>(type: "date", nullable: true),
                    date_fin = table.Column<DateOnly>(type: "date", nullable: true),
                    statut = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "En cours"),
                    duree = table.Column<int>(type: "int", nullable: true),
                    url_git = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    id_enseignant = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Projet__36C5661D2CDF80AB", x => x.id_projet);
                    table.ForeignKey(
                        name: "FK__Projet__id_ensei__6FE99F9F",
                        column: x => x.id_enseignant,
                        principalTable: "Enseignant",
                        principalColumn: "id_enseignant");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_IA",
                columns: table => new
                {
                    id_config = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    quota_requetes = table.Column<int>(type: "int", nullable: true),
                    quota_tokens = table.Column<int>(type: "int", nullable: true),
                    periode_quota = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    generation_code_autorisee = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    date_configuration = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    id_projet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Configur__8F0A1FB2901856BE", x => x.id_config);
                    table.ForeignKey(
                        name: "FK__Configura__id_pr__18EBB532",
                        column: x => x.id_projet,
                        principalTable: "Projet",
                        principalColumn: "id_projet");
                });

            migrationBuilder.CreateTable(
                name: "Contribution",
                columns: table => new
                {
                    id_contribution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message_commit = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    date_commit = table.Column<DateTime>(type: "datetime", nullable: true),
                    hash_commit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    lignes_ajoutees = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    lignes_supprimees = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    id_etudiant = table.Column<int>(type: "int", nullable: false),
                    id_projet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Contribu__95E7F2CE51D9ADB7", x => x.id_contribution);
                    table.ForeignKey(
                        name: "FK__Contribut__id_et__02084FDA",
                        column: x => x.id_etudiant,
                        principalTable: "Etudiant",
                        principalColumn: "id_etudiant");
                    table.ForeignKey(
                        name: "FK__Contribut__id_pr__02FC7413",
                        column: x => x.id_projet,
                        principalTable: "Projet",
                        principalColumn: "id_projet");
                });

            migrationBuilder.CreateTable(
                name: "Evaluation",
                columns: table => new
                {
                    id_evaluation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    note = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    commentaire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_evaluation = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    indicateur_dependance_ia = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    id_etudiant = table.Column<int>(type: "int", nullable: false),
                    id_projet = table.Column<int>(type: "int", nullable: false),
                    id_enseignant = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Evaluati__0B77AA116C9AE613", x => x.id_evaluation);
                    table.ForeignKey(
                        name: "FK__Evaluatio__id_en__1332DBDC",
                        column: x => x.id_enseignant,
                        principalTable: "Enseignant",
                        principalColumn: "id_enseignant");
                    table.ForeignKey(
                        name: "FK__Evaluatio__id_et__114A936A",
                        column: x => x.id_etudiant,
                        principalTable: "Etudiant",
                        principalColumn: "id_etudiant");
                    table.ForeignKey(
                        name: "FK__Evaluatio__id_pr__123EB7A3",
                        column: x => x.id_projet,
                        principalTable: "Projet",
                        principalColumn: "id_projet");
                });

            migrationBuilder.CreateTable(
                name: "Groupe",
                columns: table => new
                {
                    id_groupe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom_groupe = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    id_projet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Groupe__6E0AF8301A830A66", x => x.id_groupe);
                    table.ForeignKey(
                        name: "FK__Groupe__id_proje__72C60C4A",
                        column: x => x.id_projet,
                        principalTable: "Projet",
                        principalColumn: "id_projet");
                });

            migrationBuilder.CreateTable(
                name: "Prompt",
                columns: table => new
                {
                    id_prompt = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contenu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_prompt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    nb_tokens_entree = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    nb_tokens_sortie = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    id_etudiant = table.Column<int>(type: "int", nullable: false),
                    id_projet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Prompt__368514A85D7E4526", x => x.id_prompt);
                    table.ForeignKey(
                        name: "FK__Prompt__id_etudi__08B54D69",
                        column: x => x.id_etudiant,
                        principalTable: "Etudiant",
                        principalColumn: "id_etudiant");
                    table.ForeignKey(
                        name: "FK__Prompt__id_proje__09A971A2",
                        column: x => x.id_projet,
                        principalTable: "Projet",
                        principalColumn: "id_projet");
                });

            migrationBuilder.CreateTable(
                name: "Affectation",
                columns: table => new
                {
                    id_affectation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_etudiant = table.Column<int>(type: "int", nullable: false),
                    id_groupe = table.Column<int>(type: "int", nullable: false),
                    id_role = table.Column<int>(type: "int", nullable: false),
                    id_enseignant = table.Column<int>(type: "int", nullable: true),
                    date_affectation = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Affectat__B0B9D1EED2DE9465", x => x.id_affectation);
                    table.ForeignKey(
                        name: "FK__Affectati__id_en__7C4F7684",
                        column: x => x.id_enseignant,
                        principalTable: "Enseignant",
                        principalColumn: "id_enseignant");
                    table.ForeignKey(
                        name: "FK__Affectati__id_et__797309D9",
                        column: x => x.id_etudiant,
                        principalTable: "Etudiant",
                        principalColumn: "id_etudiant");
                    table.ForeignKey(
                        name: "FK__Affectati__id_gr__7A672E12",
                        column: x => x.id_groupe,
                        principalTable: "Groupe",
                        principalColumn: "id_groupe");
                    table.ForeignKey(
                        name: "FK__Affectati__id_ro__7B5B524B",
                        column: x => x.id_role,
                        principalTable: "Role",
                        principalColumn: "id_role");
                });

            migrationBuilder.CreateTable(
                name: "Reponse_IA",
                columns: table => new
                {
                    id_reponse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    contenu_reponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_reponse = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    modele_ia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    id_prompt = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reponse___861C0C2C2684B7FC", x => x.id_reponse);
                    table.ForeignKey(
                        name: "FK__Reponse_I__id_pr__0D7A0286",
                        column: x => x.id_prompt,
                        principalTable: "Prompt",
                        principalColumn: "id_prompt");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Admin__1A4FA5B90A300820",
                table: "Admin",
                column: "id_utilisateur",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Affectation_id_enseignant",
                table: "Affectation",
                column: "id_enseignant");

            migrationBuilder.CreateIndex(
                name: "IX_Affectation_id_etudiant",
                table: "Affectation",
                column: "id_etudiant");

            migrationBuilder.CreateIndex(
                name: "IX_Affectation_id_groupe",
                table: "Affectation",
                column: "id_groupe");

            migrationBuilder.CreateIndex(
                name: "IX_Affectation_id_role",
                table: "Affectation",
                column: "id_role");

            migrationBuilder.CreateIndex(
                name: "UQ__Configur__36C5661C00B13D51",
                table: "Configuration_IA",
                column: "id_projet",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contribution_id_etudiant",
                table: "Contribution",
                column: "id_etudiant");

            migrationBuilder.CreateIndex(
                name: "IX_Contribution_id_projet",
                table: "Contribution",
                column: "id_projet");

            migrationBuilder.CreateIndex(
                name: "UQ__Contribu__6557D4D019C39649",
                table: "Contribution",
                column: "hash_commit",
                unique: true,
                filter: "[hash_commit] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Enseigna__1A4FA5B9CC09AB17",
                table: "Enseignant",
                column: "id_utilisateur",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Etudiant__16C4CFE1D5FE85EE",
                table: "Etudiant",
                column: "code_apogee",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Etudiant__1A4FA5B9ABBF6F4D",
                table: "Etudiant",
                column: "id_utilisateur",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluation_id_enseignant",
                table: "Evaluation",
                column: "id_enseignant");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluation_id_etudiant",
                table: "Evaluation",
                column: "id_etudiant");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluation_id_projet",
                table: "Evaluation",
                column: "id_projet");

            migrationBuilder.CreateIndex(
                name: "IX_Groupe_id_projet",
                table: "Groupe",
                column: "id_projet");

            migrationBuilder.CreateIndex(
                name: "IX_Projet_id_enseignant",
                table: "Projet",
                column: "id_enseignant");

            migrationBuilder.CreateIndex(
                name: "IX_Prompt_id_etudiant",
                table: "Prompt",
                column: "id_etudiant");

            migrationBuilder.CreateIndex(
                name: "IX_Prompt_id_projet",
                table: "Prompt",
                column: "id_projet");

            migrationBuilder.CreateIndex(
                name: "IX_Reponse_IA_id_prompt",
                table: "Reponse_IA",
                column: "id_prompt");

            migrationBuilder.CreateIndex(
                name: "UQ__Role__95A62FB223DCB138",
                table: "Role",
                column: "nom_role",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Utilisat__AB6E6164AF503755",
                table: "Utilisateur",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Affectation");

            migrationBuilder.DropTable(
                name: "Configuration_IA");

            migrationBuilder.DropTable(
                name: "Contribution");

            migrationBuilder.DropTable(
                name: "Evaluation");

            migrationBuilder.DropTable(
                name: "Reponse_IA");

            migrationBuilder.DropTable(
                name: "Groupe");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Prompt");

            migrationBuilder.DropTable(
                name: "Etudiant");

            migrationBuilder.DropTable(
                name: "Projet");

            migrationBuilder.DropTable(
                name: "Enseignant");

            migrationBuilder.DropTable(
                name: "Utilisateur");
        }
    }
}
