using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class testdeleterelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LigneDeCommandeIngredient_LigneDeCommande",
                table: "LigneDeCommandeIngredient");

            migrationBuilder.DropForeignKey(
                name: "Fk_LigneDeCommandeIngredient_Ingredient",
                table: "LigneDeCommandeIngredient");

            migrationBuilder.AddForeignKey(
                name: "FK_LigneDeCommandeIngredient_LigneDeCommande",
                table: "LigneDeCommandeIngredient",
                column: "LigneDeCommandeId",
                principalTable: "LigneDeCommandes",
                principalColumn: "LigneDeCommandeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "Fk_LigneDeCommandeIngredient_Ingredient",
                table: "LigneDeCommandeIngredient",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LigneDeCommandeIngredient_LigneDeCommande",
                table: "LigneDeCommandeIngredient");

            migrationBuilder.DropForeignKey(
                name: "Fk_LigneDeCommandeIngredient_Ingredient",
                table: "LigneDeCommandeIngredient");

            migrationBuilder.AddForeignKey(
                name: "FK_LigneDeCommandeIngredient_LigneDeCommande",
                table: "LigneDeCommandeIngredient",
                column: "LigneDeCommandeId",
                principalTable: "LigneDeCommandes",
                principalColumn: "LigneDeCommandeId");

            migrationBuilder.AddForeignKey(
                name: "Fk_LigneDeCommandeIngredient_Ingredient",
                table: "LigneDeCommandeIngredient",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId");
        }
    }
}
