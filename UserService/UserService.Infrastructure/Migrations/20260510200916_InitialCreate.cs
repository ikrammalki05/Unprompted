using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "EnseignantClasse",
                columns: table => new
                {
                    IdEnseignantClasse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEnseignant = table.Column<int>(type: "int", nullable: false),
                    IdClasse = table.Column<int>(type: "int", nullable: false),
                    DateAffectation = table.Column<DateTime>(type: "datetime", nullable: true),
                    ClasseIdClasse = table.Column<int>(type: "int", nullable: true),
                    EnseignantIdEnseignant = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_EnseignantClasse_Classe_ClasseIdClasse",
                        column: x => x.ClasseIdClasse,
                        principalTable: "Classe",
                        principalColumn: "IdClasse");
                    table.ForeignKey(
                        name: "FK_EnseignantClasse_Enseignant",
                        column: x => x.IdEnseignant,
                        principalTable: "Enseignant",
                        principalColumn: "id_enseignant",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnseignantClasse_Enseignant_EnseignantIdEnseignant",
                        column: x => x.EnseignantIdEnseignant,
                        principalTable: "Enseignant",
                        principalColumn: "id_enseignant");
                });

            migrationBuilder.CreateTable(
                name: "Affectation",
                columns: table => new
                {
                    IdAffectation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdEtudiant = table.Column<int>(type: "int", nullable: true),
                    IdEnseignant = table.Column<int>(type: "int", nullable: true),
                    IdRole = table.Column<int>(type: "int", nullable: false),
                    IdGroupe = table.Column<int>(type: "int", nullable: false),
                    DateAffectation = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Affectation", x => x.IdAffectation);
                    table.ForeignKey(
                        name: "FK_Affectation_Enseignant",
                        column: x => x.IdEnseignant,
                        principalTable: "Enseignant",
                        principalColumn: "id_enseignant");
                    table.ForeignKey(
                        name: "FK_Affectation_Etudiant",
                        column: x => x.IdEtudiant,
                        principalTable: "Etudiant",
                        principalColumn: "id_etudiant");
                    table.ForeignKey(
                        name: "FK_Affectation_Role",
                        column: x => x.IdRole,
                        principalTable: "Role",
                        principalColumn: "id_role",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Admin__1A4FA5B90A300820",
                table: "Admin",
                column: "id_utilisateur",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Affectation_IdEnseignant",
                table: "Affectation",
                column: "IdEnseignant");

            migrationBuilder.CreateIndex(
                name: "IX_Affectation_IdEtudiant",
                table: "Affectation",
                column: "IdEtudiant");

            migrationBuilder.CreateIndex(
                name: "IX_Affectation_IdRole",
                table: "Affectation",
                column: "IdRole");

            migrationBuilder.CreateIndex(
                name: "UQ__Enseigna__1A4FA5B9CC09AB17",
                table: "Enseignant",
                column: "id_utilisateur",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnseignantClasse_ClasseIdClasse",
                table: "EnseignantClasse",
                column: "ClasseIdClasse");

            migrationBuilder.CreateIndex(
                name: "IX_EnseignantClasse_EnseignantIdEnseignant",
                table: "EnseignantClasse",
                column: "EnseignantIdEnseignant");

            migrationBuilder.CreateIndex(
                name: "IX_EnseignantClasse_IdClasse",
                table: "EnseignantClasse",
                column: "IdClasse");

            migrationBuilder.CreateIndex(
                name: "IX_EnseignantClasse_IdEnseignant",
                table: "EnseignantClasse",
                column: "IdEnseignant");

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
                name: "EnseignantClasse");

            migrationBuilder.DropTable(
                name: "Etudiant");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Classe");

            migrationBuilder.DropTable(
                name: "Enseignant");

            migrationBuilder.DropTable(
                name: "Utilisateur");
        }
    }
}
