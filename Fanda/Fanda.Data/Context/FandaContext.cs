using Fanda.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fanda.Data.Context
{
    public class FandaContext : DbContext //IdentityDbContext<User, Role, Guid>
    {
        public FandaContext(DbContextOptions<FandaContext> options)
        : base(options)
        { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }
        //public DbSet<BankContact> BankContacts { get; set; }
        //public DbSet<BankAddress> BankAddresses { get; set; }

        public DbSet<AuditTrail> AuditTrails { get; set; }

        public DbSet<Organization> Organizations { get; set; }

        //public DbSet<OrgContact> OrgContacts { get; set; }
        //public DbSet<OrgAddress> OrgAddresses { get; set; }
        //public DbSet<OrgUser> OrgUsers { get; set; }

        //public DbSet<OrgBank> OrgBanks { get; set; }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<PartyCategory> PartyCategories { get; set; }
        public DbSet<Party> Parties { get; set; }

        //public DbSet<PartyContact> PartyContacts { get; set; }
        //public DbSet<PartyAddress> PartyAddresses { get; set; }
        //public DbSet<PartyBank> PartyBanks { get; set; }

        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitConversion> UnitConversions { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductSegment> ProductSegments { get; set; }
        public DbSet<ProductVariety> ProductVarieties { get; set; }
        public DbSet<Product> Products { get; set; }

        //public DbSet<ProductIngredient> ProductIngredients { get; set; }
        //public DbSet<ProductPricing> ProductPricings { get; set; }

        public DbSet<ProductPricingRange> ProductPricingRanges { get; set; }

        public DbSet<InvoiceCategory> InvoiceCategories { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Stock> Stock { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add your customizations after calling base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<User>().ToTable("Users");
            //modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new OrgUserRoleConfig());
            //modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            //modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            //modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            //modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            //modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            modelBuilder.ApplyConfiguration(new AddressConfig());
            modelBuilder.ApplyConfiguration(new ContactConfig());

            modelBuilder.ApplyConfiguration(new AuditTrailConfig());

            modelBuilder.ApplyConfiguration(new BankAccountConfig());
            //modelBuilder.ApplyConfiguration(new BankContactConfig());
            //modelBuilder.ApplyConfiguration(new BankAddressConfig());

            modelBuilder.ApplyConfiguration(new OrganizationConfig());
            modelBuilder.ApplyConfiguration(new OrgContactConfig());
            modelBuilder.ApplyConfiguration(new OrgAddressConfig());
            modelBuilder.ApplyConfiguration(new OrgUserConfig());
            modelBuilder.ApplyConfiguration(new OrgBankConfig());

            modelBuilder.ApplyConfiguration(new LocationConfig());
            modelBuilder.ApplyConfiguration(new DeviceConfig());

            modelBuilder.ApplyConfiguration(new PartyCategoryConfig());
            modelBuilder.ApplyConfiguration(new PartyConfig());
            modelBuilder.ApplyConfiguration(new PartyContactConfig());
            modelBuilder.ApplyConfiguration(new PartyAddressConfig());
            modelBuilder.ApplyConfiguration(new PartyBankConfig());

            modelBuilder.ApplyConfiguration(new UnitConfig());
            modelBuilder.ApplyConfiguration(new UnitConversionConfig());
            modelBuilder.ApplyConfiguration(new ProductCategoryConfig());
            modelBuilder.ApplyConfiguration(new ProductBrandConfig());
            modelBuilder.ApplyConfiguration(new ProductSegmentConfig());
            modelBuilder.ApplyConfiguration(new ProductVarietyConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
            modelBuilder.ApplyConfiguration(new ProductIngredientConfig());
            modelBuilder.ApplyConfiguration(new ProductPricingConfig());
            modelBuilder.ApplyConfiguration(new ProductPrincingRangeConfig());

            modelBuilder.ApplyConfiguration(new InvoiceCategoryConfig());
            modelBuilder.ApplyConfiguration(new InvoiceConfig());
            modelBuilder.ApplyConfiguration(new InvoiceItemConfig());
            modelBuilder.ApplyConfiguration(new StockConfig());

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)))
            {
                property.SetColumnType("decimal(16, 4)");
                //.Relational().ColumnType = "decimal(16, 4)";
            }
        }
    }
}