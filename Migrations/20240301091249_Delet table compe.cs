using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class Delettablecompe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Comptes_CompteId",
                table: "Clients");

            migrationBuilder.DropTable(
                name: "Comptes");

            migrationBuilder.DropIndex(
                name: "IX_Clients_CompteId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CompteId",
                table: "Clients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompteId",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Comptes",
                columns: table => new
                {
                    CompteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comptes", x => x.CompteId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CompteId",
                table: "Clients",
                column: "CompteId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Comptes_CompteId",
                table: "Clients",
                column: "CompteId",
                principalTable: "Comptes",
                principalColumn: "CompteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
