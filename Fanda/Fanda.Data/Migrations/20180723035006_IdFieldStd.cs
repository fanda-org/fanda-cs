using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Data.Migrations
{
    public partial class IdFieldStd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProdVarId",
                table: "ProductVarieties",
                newName: "VarietyId");

            migrationBuilder.RenameColumn(
                name: "ProdSegId",
                table: "ProductSegments",
                newName: "SegmentId");

            migrationBuilder.RenameColumn(
                name: "ProdPriceId",
                table: "ProductPricings",
                newName: "PricingId");

            migrationBuilder.RenameColumn(
                name: "PriceRangeId",
                table: "ProductPricingRanges",
                newName: "RangeId");

            migrationBuilder.RenameColumn(
                name: "ProdIngId",
                table: "ProductIngredients",
                newName: "IngredientId");

            migrationBuilder.RenameColumn(
                name: "ProdCatId",
                table: "ProductCategories",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "ProdBrandId",
                table: "ProductBrands",
                newName: "BrandId");

            migrationBuilder.RenameColumn(
                name: "PartyCatId",
                table: "PartyCategories",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "InvCatId",
                table: "InvoiceCategories",
                newName: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VarietyId",
                table: "ProductVarieties",
                newName: "ProdVarId");

            migrationBuilder.RenameColumn(
                name: "SegmentId",
                table: "ProductSegments",
                newName: "ProdSegId");

            migrationBuilder.RenameColumn(
                name: "PricingId",
                table: "ProductPricings",
                newName: "ProdPriceId");

            migrationBuilder.RenameColumn(
                name: "RangeId",
                table: "ProductPricingRanges",
                newName: "PriceRangeId");

            migrationBuilder.RenameColumn(
                name: "IngredientId",
                table: "ProductIngredients",
                newName: "ProdIngId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ProductCategories",
                newName: "ProdCatId");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "ProductBrands",
                newName: "ProdBrandId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "PartyCategories",
                newName: "PartyCatId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "InvoiceCategories",
                newName: "InvCatId");
        }
    }
}
