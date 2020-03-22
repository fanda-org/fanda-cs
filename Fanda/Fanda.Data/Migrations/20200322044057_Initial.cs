﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Fanda.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Attention = table.Column<string>(maxLength: 50, nullable: true),
                    AddressLine1 = table.Column<string>(maxLength: 100, nullable: true),
                    AddressLine2 = table.Column<string>(maxLength: 100, nullable: true),
                    City = table.Column<string>(maxLength: 25, nullable: true),
                    State = table.Column<string>(maxLength: 25, nullable: true),
                    Country = table.Column<string>(maxLength: 25, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    Phone = table.Column<string>(maxLength: 25, nullable: true),
                    Fax = table.Column<string>(maxLength: 25, nullable: true),
                    AddressType = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Salutation = table.Column<string>(maxLength: 5, nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    Designation = table.Column<string>(maxLength: 25, nullable: true),
                    Department = table.Column<string>(maxLength: 25, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    WorkPhone = table.Column<string>(maxLength: 25, nullable: true),
                    Mobile = table.Column<string>(maxLength: 25, nullable: true),
                    IsPrimary = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrgCode = table.Column<string>(maxLength: 16, nullable: false),
                    OrgName = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    RegdNum = table.Column<string>(maxLength: 25, nullable: true),
                    PAN = table.Column<string>(maxLength: 25, nullable: true),
                    TAN = table.Column<string>(maxLength: 25, nullable: true),
                    GSTIN = table.Column<string>(maxLength: 25, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 25, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: false),
                    PasswordSalt = table.Column<byte[]>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    DateLastLogin = table.Column<DateTime>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    YearCode = table.Column<string>(maxLength: 16, nullable: false),
                    YearBegin = table.Column<DateTime>(nullable: false),
                    YearEnd = table.Column<DateTime>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountYears_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 16, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    OrgId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceCategories_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LedgerGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GroupCode = table.Column<string>(maxLength: 16, nullable: false),
                    GroupName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    GroupType = table.Column<string>(maxLength: 20, nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    IsSystem = table.Column<bool>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LedgerGroups_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LedgerGroups_LedgerGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "LedgerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgAddresses",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgAddresses", x => new { x.OrgId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_OrgAddresses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgAddresses_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrgContacts",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    ContactId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgContacts", x => new { x.OrgId, x.ContactId });
                    table.ForeignKey(
                        name: "FK_OrgContacts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgContacts_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartyCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    OrgId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyCategories_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductBrands",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    OrgId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBrands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductBrands_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    OrgId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCategories_ProductCategories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSegments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    OrgId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSegments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSegments_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductVarieties",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    OrgId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVarieties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductVarieties_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    OrgId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    OrgId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Units_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgUsers",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUsers", x => new { x.OrgId, x.UserId });
                    table.ForeignKey(
                        name: "FK_OrgUsers_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ledgers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LedgerCode = table.Column<string>(maxLength: 16, nullable: false),
                    LedgerName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    LedgerGroupId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: true),
                    IsSystem = table.Column<bool>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ledgers_LedgerGroups_LedgerGroupId",
                        column: x => x.LedgerGroupId,
                        principalTable: "LedgerGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ledgers_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ledgers_Ledgers_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    ProductType = table.Column<string>(maxLength: 16, nullable: true),
                    CategoryId = table.Column<Guid>(nullable: false),
                    BrandId = table.Column<Guid>(nullable: true),
                    SegmentId = table.Column<Guid>(nullable: true),
                    VarietyId = table.Column<Guid>(nullable: true),
                    UnitId = table.Column<Guid>(nullable: false),
                    TaxCode = table.Column<string>(nullable: true),
                    TaxPreference = table.Column<string>(maxLength: 16, nullable: true),
                    CentralGstPct = table.Column<decimal>(nullable: false),
                    StateGstPct = table.Column<decimal>(nullable: false),
                    InterGstPct = table.Column<decimal>(nullable: false),
                    CostPrice = table.Column<decimal>(nullable: false),
                    SellingPrice = table.Column<decimal>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductBrands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "ProductBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductSegments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "ProductSegments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductVarieties_VarietyId",
                        column: x => x.VarietyId,
                        principalTable: "ProductVarieties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgUserRoles",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUserRoles", x => new { x.OrgId, x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_OrgUserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgUserRoles_OrgUsers_OrgId_UserId",
                        columns: x => new { x.OrgId, x.UserId },
                        principalTable: "OrgUsers",
                        principalColumns: new[] { "OrgId", "UserId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                columns: table => new
                {
                    LedgerId = table.Column<Guid>(nullable: false),
                    AccountNumber = table.Column<string>(maxLength: 25, nullable: false),
                    AccountType = table.Column<string>(maxLength: 16, nullable: true),
                    IfscCode = table.Column<string>(maxLength: 16, nullable: true),
                    MicrCode = table.Column<string>(maxLength: 16, nullable: true),
                    BranchCode = table.Column<string>(maxLength: 16, nullable: true),
                    BranchName = table.Column<string>(maxLength: 50, nullable: true),
                    ContactId = table.Column<Guid>(nullable: true),
                    AddressId = table.Column<Guid>(nullable: true),
                    IsDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.LedgerId);
                    table.ForeignKey(
                        name: "FK_Banks_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Banks_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Banks_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LedgerBalances_AccountYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AccountYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    LedgerId = table.Column<Guid>(nullable: false),
                    RegdNum = table.Column<string>(maxLength: 25, nullable: true),
                    PAN = table.Column<string>(maxLength: 25, nullable: true),
                    TAN = table.Column<string>(maxLength: 25, nullable: true),
                    GSTIN = table.Column<string>(maxLength: 25, nullable: true),
                    PartyType = table.Column<string>(maxLength: 16, nullable: true),
                    PaymentTerm = table.Column<string>(maxLength: 16, nullable: true),
                    CreditLimit = table.Column<decimal>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.LedgerId);
                    table.ForeignKey(
                        name: "FK_Parties_PartyCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "PartyCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parties_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductIngredients",
                columns: table => new
                {
                    ParentProductId = table.Column<Guid>(nullable: false),
                    ChildProductId = table.Column<Guid>(nullable: false),
                    UnitId = table.Column<Guid>(nullable: false),
                    Qty = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIngredients", x => new { x.ParentProductId, x.ChildProductId, x.UnitId });
                    table.ForeignKey(
                        name: "FK_ProductIngredients_Products_ChildProductId",
                        column: x => x.ChildProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductIngredients_Products_ParentProductId",
                        column: x => x.ParentProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductIngredients_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductPricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    PartyCategoryId = table.Column<Guid>(nullable: true),
                    InvoiceCategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPricings_InvoiceCategories_InvoiceCategoryId",
                        column: x => x.InvoiceCategoryId,
                        principalTable: "InvoiceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPricings_PartyCategories_PartyCategoryId",
                        column: x => x.PartyCategoryId,
                        principalTable: "PartyCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPricings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    BatchNumber = table.Column<string>(maxLength: 25, nullable: true),
                    MfgDate = table.Column<DateTime>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    UnitId = table.Column<Guid>(nullable: false),
                    QtyOnHand = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stock_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InvoiceNumber = table.Column<string>(maxLength: 16, nullable: true),
                    InvoiceDate = table.Column<DateTime>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    InvoiceType = table.Column<string>(maxLength: 16, nullable: true),
                    StockInvoiceType = table.Column<string>(maxLength: 16, nullable: true),
                    GstTreatment = table.Column<string>(maxLength: 16, nullable: true),
                    TaxPreference = table.Column<string>(maxLength: 16, nullable: true),
                    Notes = table.Column<string>(maxLength: 255, nullable: true),
                    PartyId = table.Column<Guid>(nullable: false),
                    PartyRefNum = table.Column<string>(maxLength: 16, nullable: true),
                    PartyRefDate = table.Column<DateTime>(nullable: true),
                    BuyerId = table.Column<Guid>(nullable: true),
                    Subtotal = table.Column<decimal>(nullable: false),
                    DiscountPct = table.Column<decimal>(nullable: false),
                    DiscountAmt = table.Column<decimal>(nullable: false),
                    TaxPct = table.Column<decimal>(nullable: false),
                    TaxAmt = table.Column<decimal>(nullable: false),
                    MiscAddDesc = table.Column<decimal>(nullable: false),
                    MiscAddAmt = table.Column<decimal>(nullable: false),
                    GrandTotal = table.Column<decimal>(nullable: false),
                    YearId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Parties_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Parties",
                        principalColumn: "LedgerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_InvoiceCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "InvoiceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "LedgerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_AccountYears_YearId",
                        column: x => x.YearId,
                        principalTable: "AccountYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyAddresses",
                columns: table => new
                {
                    PartyId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyAddresses", x => new { x.PartyId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_PartyAddresses_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyAddresses_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "LedgerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartyContacts",
                columns: table => new
                {
                    PartyId = table.Column<Guid>(nullable: false),
                    ContactId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyContacts", x => new { x.PartyId, x.ContactId });
                    table.ForeignKey(
                        name: "FK_PartyContacts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyContacts_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "LedgerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPricingRanges",
                columns: table => new
                {
                    RangeId = table.Column<Guid>(nullable: false),
                    PricingId = table.Column<Guid>(nullable: false),
                    MinQty = table.Column<decimal>(nullable: false),
                    MaxQty = table.Column<decimal>(nullable: false),
                    AdjustPct = table.Column<decimal>(nullable: false),
                    AdjustAmt = table.Column<decimal>(nullable: false),
                    RoundOffOption = table.Column<string>(maxLength: 16, nullable: true),
                    FinalPrice = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPricingRanges", x => new { x.RangeId, x.PricingId });
                    table.ForeignKey(
                        name: "FK_ProductPricingRanges_ProductPricings_PricingId",
                        column: x => x.PricingId,
                        principalTable: "ProductPricings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    InvoiceItemId = table.Column<Guid>(nullable: false),
                    InvoiceId = table.Column<Guid>(nullable: false),
                    StockId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    UnitId = table.Column<Guid>(nullable: false),
                    Qty = table.Column<decimal>(nullable: false),
                    UnitPrice = table.Column<decimal>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    DiscountPct = table.Column<decimal>(nullable: false),
                    DiscountAmt = table.Column<decimal>(nullable: false),
                    CentralGstPct = table.Column<decimal>(nullable: false),
                    CentralGstAmt = table.Column<decimal>(nullable: false),
                    StateGstPct = table.Column<decimal>(nullable: false),
                    StateGstAmt = table.Column<decimal>(nullable: false),
                    InterGstPct = table.Column<decimal>(nullable: false),
                    InterGstAmt = table.Column<decimal>(nullable: false),
                    LineTotal = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => new { x.InvoiceItemId, x.InvoiceId });
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Stock_StockId",
                        column: x => x.StockId,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountYears_OrgId",
                table: "AccountYears",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountYears_YearCode_OrgId",
                table: "AccountYears",
                columns: new[] { "YearCode", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_AccountNumber",
                table: "Banks",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_AddressId",
                table: "Banks",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_ContactId",
                table: "Banks",
                column: "ContactId",
                unique: true,
                filter: "[ContactId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceCategories_OrgId",
                table: "InvoiceCategories",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceCategories_Code_OrgId",
                table: "InvoiceCategories",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceCategories_Name_OrgId",
                table: "InvoiceCategories",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_StockId",
                table: "InvoiceItems",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_UnitId",
                table: "InvoiceItems",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_BuyerId",
                table: "Invoices",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_CategoryId",
                table: "Invoices",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PartyId",
                table: "Invoices",
                column: "PartyId");

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
                name: "IX_LedgerBalances_YearId",
                table: "LedgerBalances",
                column: "YearId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerGroups_OrgId",
                table: "LedgerGroups",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerGroups_ParentId",
                table: "LedgerGroups",
                column: "ParentId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_LedgerGroupId",
                table: "Ledgers",
                column: "LedgerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_OrgId",
                table: "Ledgers",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_ParentId",
                table: "Ledgers",
                column: "ParentId");

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
                name: "IX_OrgAddresses_AddressId",
                table: "OrgAddresses",
                column: "AddressId");

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
                name: "IX_OrgContacts_ContactId",
                table: "OrgContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUserRoles_RoleId",
                table: "OrgUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUsers_UserId",
                table: "OrgUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_CategoryId",
                table: "Parties",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyAddresses_AddressId",
                table: "PartyAddresses",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyCategories_OrgId",
                table: "PartyCategories",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyCategories_Code_OrgId",
                table: "PartyCategories",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyCategories_Name_OrgId",
                table: "PartyCategories",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyContacts_ContactId",
                table: "PartyContacts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrands_OrgId",
                table: "ProductBrands",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrands_Code_OrgId",
                table: "ProductBrands",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrands_Name_OrgId",
                table: "ProductBrands",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_OrgId",
                table: "ProductCategories",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ParentId",
                table: "ProductCategories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_Code_OrgId",
                table: "ProductCategories",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_Name_OrgId",
                table: "ProductCategories",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductIngredients_ChildProductId",
                table: "ProductIngredients",
                column: "ChildProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductIngredients_UnitId",
                table: "ProductIngredients",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricingRanges_PricingId",
                table: "ProductPricingRanges",
                column: "PricingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricings_InvoiceCategoryId",
                table: "ProductPricings",
                column: "InvoiceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricings_PartyCategoryId",
                table: "ProductPricings",
                column: "PartyCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPricings_ProductId_PartyCategoryId_InvoiceCategoryId",
                table: "ProductPricings",
                columns: new[] { "ProductId", "PartyCategoryId", "InvoiceCategoryId" },
                unique: true,
                filter: "[PartyCategoryId] IS NOT NULL AND [InvoiceCategoryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrgId",
                table: "Products",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SegmentId",
                table: "Products",
                column: "SegmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitId",
                table: "Products",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VarietyId",
                table: "Products",
                column: "VarietyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code_OrgId",
                table: "Products",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name_OrgId",
                table: "Products",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSegments_OrgId",
                table: "ProductSegments",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSegments_Code_OrgId",
                table: "ProductSegments",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSegments_Name_OrgId",
                table: "ProductSegments",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVarieties_OrgId",
                table: "ProductVarieties",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVarieties_Code_OrgId",
                table: "ProductVarieties",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVarieties_Name_OrgId",
                table: "ProductVarieties",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_OrgId",
                table: "Roles",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Code_OrgId",
                table: "Roles",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name_OrgId",
                table: "Roles",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ProductId",
                table: "Stock",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_UnitId",
                table: "Stock",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_BatchNumber_ProductId",
                table: "Stock",
                columns: new[] { "BatchNumber", "ProductId" },
                unique: true,
                filter: "[BatchNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Units_OrgId",
                table: "Units",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Code_OrgId",
                table: "Units",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_Name_OrgId",
                table: "Units",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banks");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "LedgerBalances");

            migrationBuilder.DropTable(
                name: "OrgAddresses");

            migrationBuilder.DropTable(
                name: "OrgContacts");

            migrationBuilder.DropTable(
                name: "OrgUserRoles");

            migrationBuilder.DropTable(
                name: "PartyAddresses");

            migrationBuilder.DropTable(
                name: "PartyContacts");

            migrationBuilder.DropTable(
                name: "ProductIngredients");

            migrationBuilder.DropTable(
                name: "ProductPricingRanges");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "OrgUsers");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "ProductPricings");

            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "AccountYears");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "InvoiceCategories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "PartyCategories");

            migrationBuilder.DropTable(
                name: "Ledgers");

            migrationBuilder.DropTable(
                name: "ProductBrands");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductSegments");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "ProductVarieties");

            migrationBuilder.DropTable(
                name: "LedgerGroups");

            migrationBuilder.DropTable(
                name: "Organizations");
        }
    }
}