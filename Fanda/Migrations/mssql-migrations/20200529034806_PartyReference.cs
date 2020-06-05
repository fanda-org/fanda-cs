using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Models.Migrations
{
    public partial class PartyReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PartyBatchNumber",
                table: "Stock",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartyRefNum",
                table: "Invoices",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartyBatchNumber",
                table: "Stock");

            migrationBuilder.AlterColumn<string>(
                name: "PartyRefNum",
                table: "Invoices",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 25,
                oldNullable: true);
        }
    }
}
