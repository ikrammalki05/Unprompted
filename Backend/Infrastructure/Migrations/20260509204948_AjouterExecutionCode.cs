using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjouterExecutionCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        name: "FK_ExecutionCode_Projet_id_projet",
                        column: x => x.id_projet,
                        principalTable: "Projet",
                        principalColumn: "id_projet",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionCode_id_projet",
                table: "ExecutionCode",
                column: "id_projet");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionCode");
        }
    }
}
