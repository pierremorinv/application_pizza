using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class addrelationonetomanybetweencommandeandligneDeCommande : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommandeId",
                table: "LigneDeCommandes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LigneDeCommandes_CommandeId",
                table: "LigneDeCommandes",
                column: "CommandeId");

            migrationBuilder.AddForeignKey(
                name: "FK_LigneDeCommandes_Commandes_CommandeId",
                table: "LigneDeCommandes",
                column: "CommandeId",
                principalTable: "Commandes",
                principalColumn: "CommandeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LigneDeCommandes_Commandes_CommandeId",
                table: "LigneDeCommandes");

            migrationBuilder.DropIndex(
                name: "IX_LigneDeCommandes_CommandeId",
                table: "LigneDeCommandes");

            migrationBuilder.DropColumn(
                name: "CommandeId",
                table: "LigneDeCommandes");
        }
    }
}
