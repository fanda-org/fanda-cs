using Microsoft.EntityFrameworkCore.Migrations;
using System;

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
                    AddressId = table.Column<Guid>(nullable: false),
                    AddressLine1 = table.Column<string>(maxLength: 100, nullable: true),
                    AddressLine2 = table.Column<string>(maxLength: 100, nullable: true),
                    City = table.Column<string>(maxLength: 25, nullable: true),
                    State = table.Column<string>(maxLength: 25, nullable: true),
                    Country = table.Column<string>(maxLength: 25, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    AddressType = table.Column<string>(maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactId = table.Column<Guid>(nullable: false),
                    ContactName = table.Column<string>(maxLength: 50, nullable: true),
                    ContactTitle = table.Column<string>(maxLength: 50, nullable: true),
                    ContactPhone = table.Column<string>(maxLength: 25, nullable: true),
                    ContactEmail = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactId);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_Organizations", x => x.OrgId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    BankAcctId = table.Column<Guid>(nullable: false),
                    AccountNumber = table.Column<string>(maxLength: 25, nullable: false),
                    BankShortName = table.Column<string>(maxLength: 15, nullable: true),
                    BankName = table.Column<string>(maxLength: 50, nullable: false),
                    AccountType = table.Column<string>(maxLength: 16, nullable: true),
                    IfscCode = table.Column<string>(maxLength: 16, nullable: true),
                    MicrCode = table.Column<string>(maxLength: 16, nullable: true),
                    BranchCode = table.Column<string>(maxLength: 16, nullable: true),
                    BranchName = table.Column<string>(maxLength: 50, nullable: true),
                    ContactId = table.Column<Guid>(nullable: true),
                    AddressId = table.Column<Guid>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.BankAcctId);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceCategories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 16, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceCategories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_InvoiceCategories_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgAddress",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgAddress", x => new { x.OrgId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_OrgAddress_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgAddress_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrgContact",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    ContactId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgContact", x => new { x.OrgId, x.ContactId });
                    table.ForeignKey(
                        name: "FK_OrgContact_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgContact_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartyCategories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyCategories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_PartyCategories_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductBrands",
                columns: table => new
                {
                    BrandId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBrands", x => x.BrandId);
                    table.ForeignKey(
                        name: "FK_ProductBrands_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.CategoryId);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCategories_ProductCategories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "ProductCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSegments",
                columns: table => new
                {
                    SegmentId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSegments", x => x.SegmentId);
                    table.ForeignKey(
                        name: "FK_ProductSegments_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductVarieties",
                columns: table => new
                {
                    VarietyId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVarieties", x => x.VarietyId);
                    table.ForeignKey(
                        name: "FK_ProductVarieties_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_Units_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgBank",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    BankAcctId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgBank", x => new { x.OrgId, x.BankAcctId });
                    table.ForeignKey(
                        name: "FK_OrgBank_BankAccounts_BankAcctId",
                        column: x => x.BankAcctId,
                        principalTable: "BankAccounts",
                        principalColumn: "BankAcctId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgBank_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    DeviceId = table.Column<Guid>(nullable: false),
                    LocationId = table.Column<Guid>(nullable: true),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_Devices_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 25, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    PasswordHash = table.Column<byte[]>(nullable: false),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    DateLastLogin = table.Column<DateTime>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    LocationId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parties",
                columns: table => new
                {
                    PartyId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 16, nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    RegdNum = table.Column<string>(maxLength: 25, nullable: true),
                    PAN = table.Column<string>(maxLength: 25, nullable: true),
                    TAN = table.Column<string>(maxLength: 25, nullable: true),
                    GSTIN = table.Column<string>(maxLength: 25, nullable: true),
                    PartyType = table.Column<string>(maxLength: 16, nullable: true),
                    PaymentTerm = table.Column<string>(maxLength: 16, nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parties", x => x.PartyId);
                    table.ForeignKey(
                        name: "FK_Parties_PartyCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "PartyCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parties_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
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
                    CentralGstPct = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    StateGstPct = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    InterGstPct = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_ProductBrands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "ProductBrands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductSegments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "ProductSegments",
                        principalColumn: "SegmentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductVarieties_VarietyId",
                        column: x => x.VarietyId,
                        principalTable: "ProductVarieties",
                        principalColumn: "VarietyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnitConversions",
                columns: table => new
                {
                    ConversionId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
                    FromUnitId = table.Column<Guid>(nullable: false),
                    ToUnitId = table.Column<Guid>(nullable: false),
                    CalcStep = table.Column<byte>(nullable: false),
                    Operator = table.Column<string>(nullable: false),
                    Factor = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitConversions", x => x.ConversionId);
                    table.ForeignKey(
                        name: "FK_UnitConversions_Units_FromUnitId",
                        column: x => x.FromUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitConversions_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitConversions_Units_ToUnitId",
                        column: x => x.ToUnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    AuditTrailId = table.Column<Guid>(nullable: false),
                    TableName = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    RowId = table.Column<Guid>(nullable: false),
                    CurrentStatus = table.Column<string>(unicode: false, maxLength: 16, nullable: false),
                    CreatedUserId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ModifiedUserId = table.Column<Guid>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: true),
                    DeletedUserId = table.Column<Guid>(nullable: true),
                    DateDeleted = table.Column<DateTime>(nullable: true),
                    PrintedUserId = table.Column<Guid>(nullable: true),
                    DatePrinted = table.Column<DateTime>(nullable: true),
                    ApprovedUserId = table.Column<Guid>(nullable: true),
                    DateApproved = table.Column<DateTime>(nullable: true),
                    RejectedUserId = table.Column<Guid>(nullable: true),
                    DateRejected = table.Column<DateTime>(nullable: true),
                    HoldUserId = table.Column<Guid>(nullable: true),
                    DateHold = table.Column<DateTime>(nullable: true),
                    ActivatedUserId = table.Column<Guid>(nullable: true),
                    DateActivated = table.Column<DateTime>(nullable: true),
                    DeactivatedUserId = table.Column<Guid>(nullable: true),
                    DateDeactivated = table.Column<DateTime>(nullable: true)
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

            migrationBuilder.CreateTable(
                name: "OrgUser",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUser", x => new { x.OrgId, x.UserId });
                    table.ForeignKey(
                        name: "FK_OrgUser_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<Guid>(nullable: false),
                    OrgId = table.Column<Guid>(nullable: false),
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
                    Subtotal = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    DiscountPct = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    DiscountAmt = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    TaxPct = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    TaxAmt = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    MiscAddDesc = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    MiscAddAmt = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_Parties_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_InvoiceCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "InvoiceCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_Organizations_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Organizations",
                        principalColumn: "OrgId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartyAddress",
                columns: table => new
                {
                    PartyId = table.Column<Guid>(nullable: false),
                    AddressId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyAddress", x => new { x.PartyId, x.AddressId });
                    table.ForeignKey(
                        name: "FK_PartyAddress_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyAddress_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartyBank",
                columns: table => new
                {
                    PartyId = table.Column<Guid>(nullable: false),
                    BankAcctId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyBank", x => new { x.PartyId, x.BankAcctId });
                    table.ForeignKey(
                        name: "FK_PartyBank_BankAccounts_BankAcctId",
                        column: x => x.BankAcctId,
                        principalTable: "BankAccounts",
                        principalColumn: "BankAcctId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyBank_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartyContact",
                columns: table => new
                {
                    PartyId = table.Column<Guid>(nullable: false),
                    ContactId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartyContact", x => new { x.PartyId, x.ContactId });
                    table.ForeignKey(
                        name: "FK_PartyContact_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "ContactId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyContact_Parties_PartyId",
                        column: x => x.PartyId,
                        principalTable: "Parties",
                        principalColumn: "PartyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductIngredients",
                columns: table => new
                {
                    IngredientId = table.Column<Guid>(nullable: false),
                    ParentProductId = table.Column<Guid>(nullable: false),
                    ChildProductId = table.Column<Guid>(nullable: false),
                    UnitId = table.Column<Guid>(nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(16, 4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductIngredients", x => x.IngredientId);
                    table.ForeignKey(
                        name: "FK_ProductIngredients_Products_ChildProductId",
                        column: x => x.ChildProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductIngredients_Products_ParentProductId",
                        column: x => x.ParentProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductIngredients_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductPricings",
                columns: table => new
                {
                    PricingId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    PartyCategoryId = table.Column<Guid>(nullable: true),
                    InvoiceCategoryId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPricings", x => x.PricingId);
                    table.ForeignKey(
                        name: "FK_ProductPricings_InvoiceCategories_InvoiceCategoryId",
                        column: x => x.InvoiceCategoryId,
                        principalTable: "InvoiceCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPricings_PartyCategories_PartyCategoryId",
                        column: x => x.PartyCategoryId,
                        principalTable: "PartyCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPricings_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    StockId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    BatchNumber = table.Column<string>(maxLength: 25, nullable: true),
                    MfgDate = table.Column<DateTime>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    UnitId = table.Column<Guid>(nullable: false),
                    QtyOnHand = table.Column<decimal>(type: "decimal(16, 4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.StockId);
                    table.ForeignKey(
                        name: "FK_Stock_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stock_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgUserRole",
                columns: table => new
                {
                    OrgId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUserRole", x => new { x.OrgId, x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_OrgUserRole_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgUserRole_OrgUser_OrgId_UserId",
                        columns: x => new { x.OrgId, x.UserId },
                        principalTable: "OrgUser",
                        principalColumns: new[] { "OrgId", "UserId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPricingRanges",
                columns: table => new
                {
                    RangeId = table.Column<Guid>(nullable: false),
                    PricingId = table.Column<Guid>(nullable: false),
                    MinQty = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    MaxQty = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    AdjustPct = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    AdjustAmt = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    RoundOffOption = table.Column<string>(maxLength: 16, nullable: true),
                    FinalPrice = table.Column<decimal>(type: "decimal(16, 4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPricingRanges", x => x.RangeId);
                    table.ForeignKey(
                        name: "FK_ProductPricingRanges_ProductPricings_PricingId",
                        column: x => x.PricingId,
                        principalTable: "ProductPricings",
                        principalColumn: "PricingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    InvItemId = table.Column<Guid>(nullable: false),
                    InvoiceId = table.Column<Guid>(nullable: false),
                    StockId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    UnitId = table.Column<Guid>(nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    DiscountPct = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    DiscountAmt = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    CentralGstPct = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    CentralGstAmt = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    StateGstPct = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    StateGstAmt = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    InterGstPct = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    InterGstAmt = table.Column<decimal>(type: "decimal(16, 4)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(16, 4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.InvItemId);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "InvoiceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Stock_StockId",
                        column: x => x.StockId,
                        principalTable: "Stock",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_AccountNumber",
                table: "BankAccounts",
                column: "AccountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_AddressId",
                table: "BankAccounts",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_ContactId",
                table: "BankAccounts",
                column: "ContactId",
                unique: true,
                filter: "[ContactId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_LocationId",
                table: "Devices",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Code_LocationId",
                table: "Devices",
                columns: new[] { "Code", "LocationId" },
                unique: true,
                filter: "[LocationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Name_LocationId",
                table: "Devices",
                columns: new[] { "Name", "LocationId" },
                unique: true,
                filter: "[LocationId] IS NOT NULL");

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
                name: "IX_Invoices_OrgId",
                table: "Invoices",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PartyId",
                table: "Invoices",
                column: "PartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_InvoiceNumber_OrgId",
                table: "Invoices",
                columns: new[] { "InvoiceNumber", "OrgId" },
                unique: true,
                filter: "[InvoiceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_OrgId",
                table: "Locations",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Code_OrgId",
                table: "Locations",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name_OrgId",
                table: "Locations",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrgAddress_AddressId",
                table: "OrgAddress",
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
                name: "IX_OrgBank_BankAcctId",
                table: "OrgBank",
                column: "BankAcctId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgContact_ContactId",
                table: "OrgContact",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUser_UserId",
                table: "OrgUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUserRole_RoleId",
                table: "OrgUserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_CategoryId",
                table: "Parties",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_OrgId",
                table: "Parties",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Code_OrgId",
                table: "Parties",
                columns: new[] { "Code", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parties_Name_OrgId",
                table: "Parties",
                columns: new[] { "Name", "OrgId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyAddress_AddressId",
                table: "PartyAddress",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyBank_BankAcctId",
                table: "PartyBank",
                column: "BankAcctId");

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
                name: "IX_PartyContact_ContactId",
                table: "PartyContact",
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
                name: "IX_ProductIngredients_ParentProductId_ChildProductId",
                table: "ProductIngredients",
                columns: new[] { "ParentProductId", "ChildProductId" },
                unique: true);

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
                name: "IX_ProductPricings_ProductId",
                table: "ProductPricings",
                column: "ProductId");

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
                name: "IX_Roles_Code",
                table: "Roles",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
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
                name: "IX_UnitConversions_FromUnitId",
                table: "UnitConversions",
                column: "FromUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitConversions_OrgId",
                table: "UnitConversions",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitConversions_ToUnitId",
                table: "UnitConversions",
                column: "ToUnitId");

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
                name: "IX_Users_LocationId",
                table: "Users",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "OrgAddress");

            migrationBuilder.DropTable(
                name: "OrgBank");

            migrationBuilder.DropTable(
                name: "OrgContact");

            migrationBuilder.DropTable(
                name: "OrgUserRole");

            migrationBuilder.DropTable(
                name: "PartyAddress");

            migrationBuilder.DropTable(
                name: "PartyBank");

            migrationBuilder.DropTable(
                name: "PartyContact");

            migrationBuilder.DropTable(
                name: "ProductIngredients");

            migrationBuilder.DropTable(
                name: "ProductPricingRanges");

            migrationBuilder.DropTable(
                name: "UnitConversions");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "OrgUser");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "ProductPricings");

            migrationBuilder.DropTable(
                name: "Parties");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "InvoiceCategories");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "PartyCategories");

            migrationBuilder.DropTable(
                name: "Locations");

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
                name: "Organizations");
        }
    }
}
