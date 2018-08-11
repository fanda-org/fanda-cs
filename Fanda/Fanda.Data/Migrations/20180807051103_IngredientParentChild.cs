using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Data.Migrations
{
    public partial class IngredientParentChild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Products_IngredientProductId",
                table: "ProductIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Products_ProductId",
                table: "ProductIngredients");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductIngredients",
                newName: "ParentProductId");

            migrationBuilder.RenameColumn(
                name: "IngredientProductId",
                table: "ProductIngredients",
                newName: "ChildProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredients_ProductId_IngredientProductId",
                table: "ProductIngredients",
                newName: "IX_ProductIngredients_ParentProductId_ChildProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredients_IngredientProductId",
                table: "ProductIngredients",
                newName: "IX_ProductIngredients_ChildProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Products_ChildProductId",
                table: "ProductIngredients",
                column: "ChildProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Products_ParentProductId",
                table: "ProductIngredients",
                column: "ParentProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Products_ChildProductId",
                table: "ProductIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductIngredients_Products_ParentProductId",
                table: "ProductIngredients");

            migrationBuilder.RenameColumn(
                name: "ParentProductId",
                table: "ProductIngredients",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "ChildProductId",
                table: "ProductIngredients",
                newName: "IngredientProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredients_ParentProductId_ChildProductId",
                table: "ProductIngredients",
                newName: "IX_ProductIngredients_ProductId_IngredientProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductIngredients_ChildProductId",
                table: "ProductIngredients",
                newName: "IX_ProductIngredients_IngredientProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Products_IngredientProductId",
                table: "ProductIngredients",
                column: "IngredientProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductIngredients_Products_ProductId",
                table: "ProductIngredients",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
