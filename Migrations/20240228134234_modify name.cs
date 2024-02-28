using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class modifyname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Compte_CompteId",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Compte",
                table: "Compte");

            migrationBuilder.RenameTable(
                name: "Compte",
                newName: "Comptes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comptes",
                table: "Comptes",
                column: "CompteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Comptes_CompteId",
                table: "Clients",
                column: "CompteId",
                principalTable: "Comptes",
                principalColumn: "CompteId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Comptes_CompteId",
                table: "Clients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comptes",
                table: "Comptes");

            migrationBuilder.RenameTable(
                name: "Comptes",
                newName: "Compte");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Compte",
                table: "Compte",
                column: "CompteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Compte_CompteId",
                table: "Clients",
                column: "CompteId",
                principalTable: "Compte",
                principalColumn: "CompteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
