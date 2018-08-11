using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Data.Migrations
{
    public partial class ProductIngredients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductIngredients_IngredientProductId",
                table: "ProductIngredients",
                column: "IngredientProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Products_IngredientProductId",
                table: "ProductIngredients",
                column: "IngredientProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Products_IngredientProductId",
                table: "ProductIngredients");

            migrationBuilder.DropIndex(
                name: "IX_ProductIngredients_IngredientProductId",
                table: "ProductIngredients");
        }
    }
}
