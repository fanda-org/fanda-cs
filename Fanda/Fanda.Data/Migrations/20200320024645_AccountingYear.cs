using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Data.Migrations
{
    public partial class AccountingYear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Organizations_OrgId",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_OrgId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_InvoiceNumber_OrgId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "Invoices");

            migrationBuilder.AddColumn<Guid>(
                name: "YearId",
                table: "Invoices",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AccountYear",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    YearCode = table.Column<string>(nullable: true),
                    YearBegin = table.Column<DateTime>(nullable: false),
                    YearEnd = table.Column<DateTime>(nullable: false),
                    OrganizationOrgId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountYear", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountYear_Organizations_OrganizationOrgId",
                        column: x => x.OrganizationOrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_YearId",
                table: "Invoices",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber_YearId",
                table: "Invoices",
                columns: new[] { "InvoiceNumber", "YearId" },
                unique: true,
                filter: "[InvoiceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AccountYear_OrganizationOrgId",
                table: "AccountYear",
                column: "OrganizationOrgId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AccountYear_YearId",
                table: "Invoices",
                column: "YearId",
                principalTable: "AccountYear",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AccountYear_YearId",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "AccountYear");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_YearId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_InvoiceNumber_YearId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "YearId",
                table: "Invoices");

            migrationBuilder.AddColumn<Guid>(
                name: "OrgId",
                table: "Invoices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    AuditTrailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentStatus = table.Column<string>(type: "varchar(16)", unicode: false, maxLength: 16, nullable: false),
                    DateActivated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateApproved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateDeactivated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateHold = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DatePrinted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateRejected = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeactivatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HoldUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PrintedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RejectedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TableName = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.AuditTrailId);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Users_ActivatedUserId",
                        column: x => x.ActivatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Users_ApprovedUserId",
                        column: x => x.ApprovedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Users_DeactivatedUserId",
                        column: x => x.DeactivatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Users_DeletedUserId",
                        column: x => x.DeletedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Users_HoldUserId",
                        column: x => x.HoldUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Users_ModifiedUserId",
                        column: x => x.ModifiedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Users_PrintedUserId",
                        column: x => x.PrintedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditTrails_Users_RejectedUserId",
                        column: x => x.RejectedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_OrgId",
                table: "Invoices",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber_OrgId",
                table: "Invoices",
                columns: new[] { "InvoiceNumber", "OrgId" },
                unique: true,
                filter: "[InvoiceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_ActivatedUserId",
                table: "AuditTrails",
                column: "ActivatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_ApprovedUserId",
                table: "AuditTrails",
                column: "ApprovedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_CreatedUserId",
                table: "AuditTrails",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_DeactivatedUserId",
                table: "AuditTrails",
                column: "DeactivatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_DeletedUserId",
                table: "AuditTrails",
                column: "DeletedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_HoldUserId",
                table: "AuditTrails",
                column: "HoldUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_ModifiedUserId",
                table: "AuditTrails",
                column: "ModifiedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_PrintedUserId",
                table: "AuditTrails",
                column: "PrintedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_RejectedUserId",
                table: "AuditTrails",
                column: "RejectedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_TableName_RowId",
                table: "AuditTrails",
                columns: new[] { "TableName", "RowId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Organizations_OrgId",
                table: "Invoices",
                column: "OrgId",
                principalTable: "Organizations",
                principalColumn: "OrgId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
