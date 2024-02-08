using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Data.Migrations
{
    /// <inheritdoc />
    public partial class addrelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "LigneDeCommandeIngredient",
                columns: table => new
                {
                    LigneDeCommandeId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LigneDeCommandeIngredient", x => new { x.LigneDeCommandeId, x.IngredientId });
                    table.ForeignKey(
                        name: "FK_LigneDeCommandeIngredient_LigneDeCommande",
                        column: x => x.LigneDeCommandeId,
                        principalTable: "LigneDeCommandes",
                        principalColumn: "LigneDeCommandeId");
                    table.ForeignKey(
                        name: "Fk_LigneDeCommandeIngredient_Ingredient",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LigneDeCommandeIngredient_IngredientId",
                table: "LigneDeCommandeIngredient",
                column: "IngredientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "LigneDeCommandeIngredient");

            //migrationBuilder.CreateTable(
            //    name: "Option",
            //    columns: table => new
            //    {
            //        OptionId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        IngredientId = table.Column<int>(type: "int", nullable: false),
            //        LigneDecommandeId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Option", x => x.OptionId);
            //        table.ForeignKey(
            //            name: "FK_Option_Ingredients_IngredientId",
            //            column: x => x.IngredientId,
            //            principalTable: "Ingredients",
            //            principalColumn: "IngredientId",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Option_LigneDeCommandes_LigneDecommandeId",
            //            column: x => x.LigneDecommandeId,
            //            principalTable: "LigneDeCommandes",
            //            principalColumn: "LigneDeCommandeId",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Option_IngredientId",
            //    table: "Option",
            //    column: "IngredientId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Option_LigneDecommandeId",
            //    table: "Option",
            //    column: "LigneDecommandeId",
            //    unique: true);
        }
    }
}
