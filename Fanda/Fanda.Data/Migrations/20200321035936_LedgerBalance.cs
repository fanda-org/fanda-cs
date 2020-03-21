using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fanda.Data.Migrations
{
    public partial class LedgerBalance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductIngredients",
                table: "ProductIngredients");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductIngredients",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductIngredients",
                table: "ProductIngredients",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "LedgerBalances",
                columns: table => new
                {
                    LedgerId = table.Column<Guid>(nullable: false),
                    YearId = table.Column<Guid>(nullable: false),
                    OpeningBalance = table.Column<decimal>(nullable: false),
                    BalanceSign = table.Column<string>(maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerBalances", x => new { x.LedgerId, x.YearId });
                    table.ForeignKey(
                        name: "FK_LedgerBalances_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LedgerBalances_AccountYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AccountYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductIngredients_ParentProductId",
                table: "ProductIngredients",
                column: "ParentProductId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerBalances_YearId",
                table: "LedgerBalances",
                column: "YearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LedgerBalances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductIngredients",
                table: "ProductIngredients");

            migrationBuilder.DropIndex(
                name: "IX_ProductIngredients_ParentProductId",
                table: "ProductIngredients");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductIngredients");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductIngredients",
                table: "ProductIngredients",
                columns: new[] { "ParentProductId", "ChildProductId" });
        }
    }
}
