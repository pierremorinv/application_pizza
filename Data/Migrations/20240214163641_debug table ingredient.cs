using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class debugtableingredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantiteIngredient",
                table: "LigneDeCommandes");

            migrationBuilder.AddColumn<int>(
                name: "quantite",
                table: "Ingredients",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "quantite",
                table: "Ingredients");

            migrationBuilder.AddColumn<int>(
                name: "QuantiteIngredient",
                table: "LigneDeCommandes",
                type: "int",
                nullable: true);
        }
    }
}
