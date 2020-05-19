using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Models.Migrations
{
    public partial class BaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrgCode",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrgName",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Ledgers_LedgerCode_OrgId",
                table: "Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_Ledgers_LedgerName_OrgId",
                table: "Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_LedgerGroups_GroupCode_OrgId",
                table: "LedgerGroups");

            migrationBuilder.DropIndex(
                name: "IX_LedgerGroups_GroupName_OrgId",
                table: "LedgerGroups");

            migrationBuilder.RenameColumn(
                name: "OrgCode",
                table: "Organizations",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "OrgName",
                table: "Organizations",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "LedgerCode",
                table: "Ledgers",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "LedgerName",
                table: "Ledgers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "GroupCode",
                table: "LedgerGroups",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "GroupName",
                table: "LedgerGroups",
                newName: "Name");

            //migrationBuilder.AddColumn<string>(
            //    name: "Code",
            //    table: "Organizations",
            //    maxLength: 16,
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "Name",
            //    table: "Organizations",
            //    maxLength: 100,
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "Code",
            //    table: "Ledgers",
            //    maxLength: 16,
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "Name",
            //    table: "Ledgers",
            //    maxLength: 50,
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "Code",
            //    table: "LedgerGroups",
            //    maxLength: 16,
            //    nullable: false,
            //    defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "Name",
            //    table: "LedgerGroups",
            //    maxLength: 50,
            //    nullable: false,
            //    defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Code",
                table: "Organizations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Name",
                table: "Organizations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_Code_OrgId",
                table: "Ledgers",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_Name_OrgId",
                table: "Ledgers",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerGroups_Code_OrgId",
                table: "LedgerGroups",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerGroups_Name_OrgId",
                table: "LedgerGroups",
                columns: new[] { "Name", "OrgId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizations_Code",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_Name",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Ledgers_Code_OrgId",
                table: "Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_Ledgers_Name_OrgId",
                table: "Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_LedgerGroups_Code_OrgId",
                table: "LedgerGroups");

            migrationBuilder.DropIndex(
                name: "IX_LedgerGroups_Name_OrgId",
                table: "LedgerGroups");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Ledgers");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "LedgerGroups");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "LedgerGroups");

            migrationBuilder.AddColumn<string>(
                name: "OrgCode",
                table: "Organizations",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrgName",
                table: "Organizations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LedgerCode",
                table: "Ledgers",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LedgerName",
                table: "Ledgers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroupCode",
                table: "LedgerGroups",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "LedgerGroups",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrgCode",
                table: "Organizations",
                column: "OrgCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrgName",
                table: "Organizations",
                column: "OrgName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_LedgerCode_OrgId",
                table: "Ledgers",
                columns: new[] { "LedgerCode", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_LedgerName_OrgId",
                table: "Ledgers",
                columns: new[] { "LedgerName", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerGroups_GroupCode_OrgId",
                table: "LedgerGroups",
                columns: new[] { "GroupCode", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LedgerGroups_GroupName_OrgId",
                table: "LedgerGroups",
                columns: new[] { "GroupName", "OrgId" },
                unique: true);
        }
    }
}
