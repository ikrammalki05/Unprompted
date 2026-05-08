using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCahierDesChargesToProjet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
    

            migrationBuilder.AddColumn<byte[]>(
                name: "CahierDesCharges",
                table: "Projet",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contraintes",
                table: "Projet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CriteresEvaluation",
                table: "Projet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Livrables",
                table: "Projet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Objectifs",
                table: "Projet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RessourcesDisponibles",
                table: "Projet",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnologiesRequises",
                table: "Projet",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CahierDesCharges",
                table: "Projet");

            migrationBuilder.DropColumn(
                name: "Contraintes",
                table: "Projet");

            migrationBuilder.DropColumn(
                name: "CriteresEvaluation",
                table: "Projet");

            migrationBuilder.DropColumn(
                name: "Livrables",
                table: "Projet");

            migrationBuilder.DropColumn(
                name: "Objectifs",
                table: "Projet");

            migrationBuilder.DropColumn(
                name: "RessourcesDisponibles",
                table: "Projet");

            migrationBuilder.DropColumn(
                name: "TechnologiesRequises",
                table: "Projet");

        }
    }
}
