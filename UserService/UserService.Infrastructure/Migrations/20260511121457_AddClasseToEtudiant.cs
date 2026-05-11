using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClasseToEtudiant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdClasse",
                table: "Etudiant",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Etudiant_IdClasse",
                table: "Etudiant",
                column: "IdClasse");

            migrationBuilder.AddForeignKey(
                name: "FK_Etudiant_Classe",
                table: "Etudiant",
                column: "IdClasse",
                principalTable: "Classe",
                principalColumn: "IdClasse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Etudiant_Classe",
                table: "Etudiant");

            migrationBuilder.DropIndex(
                name: "IX_Etudiant_IdClasse",
                table: "Etudiant");

            migrationBuilder.DropColumn(
                name: "IdClasse",
                table: "Etudiant");
        }
    }
}
