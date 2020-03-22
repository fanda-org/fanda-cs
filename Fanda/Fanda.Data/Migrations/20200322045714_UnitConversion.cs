using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Data.Migrations
{
    public partial class UnitConversion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnitConversions",
                columns: table => new
                {
                    FromUnitId = table.Column<Guid>(nullable: false),
                    ToUnitId = table.Column<Guid>(nullable: false),
                    CalcStep = table.Column<byte>(nullable: false),
                    Operator = table.Column<string>(nullable: false),
                    Factor = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitConversions", x => new { x.FromUnitId, x.ToUnitId });
                    table.ForeignKey(
                        name: "FK_UnitConversions_Units_FromUnitId",
                        column: x => x.FromUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitConversions_Units_ToUnitId",
                        column: x => x.ToUnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnitConversions_ToUnitId",
                table: "UnitConversions",
                column: "ToUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnitConversions");
        }
    }
}
