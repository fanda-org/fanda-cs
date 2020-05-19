using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fanda.Models.Migrations
{
    public partial class AcctYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invoices_InvoiceNumber_YearId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_AccountYears_YearCode_OrgId",
                table: "AccountYears");

            migrationBuilder.RenameColumn(
                name: "InvoiceDate",
                table: "Invoices",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "InvoiceNumber",
                table: "Invoices",
                newName: "Number");

            migrationBuilder.RenameColumn(
                name: "YearCode",
                table: "AccountYears",
                newName: "Code");

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "Date",
            //    table: "Invoices",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AddColumn<string>(
            //    name: "Number",
            //    table: "Invoices",
            //    maxLength: 16,
            //    nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AccountYears",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AccountYears",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "AccountYears",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "AccountYears",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "AccountYears",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "Code",
            //    table: "AccountYears",
            //    maxLength: 16,
            //    nullable: false,
            //    defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_Number_YearId",
                table: "Invoices",
                columns: new[] { "Number", "YearId" },
                unique: true,
                filter: "[Number] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AccountYears_Code_OrgId",
                table: "AccountYears",
                columns: new[] { "Code", "OrgId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invoices_Number_YearId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_AccountYears_Code_OrgId",
                table: "AccountYears");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "AccountYears");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "AccountYears");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AccountYears");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "AccountYears");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AccountYears");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AccountYears");

            migrationBuilder.AddColumn<DateTime>(
                name: "InvoiceDate",
                table: "Invoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "Invoices",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YearCode",
                table: "AccountYears",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber_YearId",
                table: "Invoices",
                columns: new[] { "InvoiceNumber", "YearId" },
                unique: true,
                filter: "[InvoiceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AccountYears_YearCode_OrgId",
                table: "AccountYears",
                columns: new[] { "YearCode", "OrgId" },
                unique: true);
        }
    }
}
