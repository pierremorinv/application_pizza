using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class modifymethoddeleteonpizzandingredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PizzaIngredient_Ingredient",
                table: "PizzaIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_PizzaIngredient_Pizza",
                table: "PizzaIngredient");

            migrationBuilder.AddForeignKey(
                name: "FK_PizzaIngredient_Ingredient",
                table: "PizzaIngredient",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PizzaIngredient_Pizza",
                table: "PizzaIngredient",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "PizzaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PizzaIngredient_Ingredient",
                table: "PizzaIngredient");

            migrationBuilder.DropForeignKey(
                name: "FK_PizzaIngredient_Pizza",
                table: "PizzaIngredient");

            migrationBuilder.AddForeignKey(
                name: "FK_PizzaIngredient_Ingredient",
                table: "PizzaIngredient",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_PizzaIngredient_Pizza",
                table: "PizzaIngredient",
                column: "PizzaId",
                principalTable: "Pizzas",
                principalColumn: "PizzaId");
        }
    }
}
