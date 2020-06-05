using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Fanda.Models.Migrations
{
    public partial class Buyer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Parties_BuyerId",
                table: "Invoices");

            migrationBuilder.CreateTable(
                name: "Buyer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContactId = table.Column<Guid>(nullable: true),
                    AddressId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Buyer_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Buyer_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buyer_AddressId",
                table: "Buyer",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Buyer_ContactId",
                table: "Buyer",
                column: "ContactId",
                unique: true,
                filter: "[ContactId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Buyer_BuyerId",
                table: "Invoices",
                column: "BuyerId",
                principalTable: "Buyer",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Buyer_BuyerId",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "Buyer");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Parties_BuyerId",
                table: "Invoices",
                column: "BuyerId",
                principalTable: "Parties",
                principalColumn: "LedgerId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
