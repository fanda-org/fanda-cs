using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Models.Migrations
{
    public partial class SerialNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SerialNumbers",
                columns: table => new
                {
                    YearId = table.Column<Guid>(nullable: false),
                    Module = table.Column<string>(maxLength: 16, nullable: false),
                    Prefix = table.Column<string>(maxLength: 5, nullable: true),
                    SerialFormat = table.Column<string>(maxLength: 25, nullable: true),
                    Suffix = table.Column<string>(maxLength: 5, nullable: true),
                    LastValue = table.Column<string>(maxLength: 25, nullable: true),
                    LastNumber = table.Column<int>(nullable: false),
                    Reset = table.Column<string>(maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialNumbers", x => new { x.YearId, x.Module });
                    table.ForeignKey(
                        name: "FK_SerialNumbers_AccountYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AccountYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SerialNumbers");
        }
    }
}
