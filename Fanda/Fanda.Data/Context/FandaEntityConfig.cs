using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fanda.Data.Context
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // table
            builder.ToTable("Roles");

            // key
            //builder.HasKey(r => r.Id);

            // columns
            builder.Property(r => r.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(25);
            builder.Property(r => r.Description)
                .HasMaxLength(255);
            //builder.Property(o => o.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(r => r.Code)
                .IsUnique();
            builder.HasIndex(r => r.Name)
                .IsUnique();
        }
    }

    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // table
            builder.ToTable("Users");

            // key
            //builder.HasKey(u => u.Id);

            // columns
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(25);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(u => u.FirstName)
                .IsRequired(false)
                .HasMaxLength(50);
            builder.Property(u => u.LastName)
                .IsRequired(false)
                .HasMaxLength(50);
            builder.Property(u => u.PasswordHash)
                .IsRequired(true);
            //builder.Property(o => o.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(u => u.UserName)
                .IsUnique();
            builder.HasIndex(u => u.Email)
                .IsUnique();
            // foreign keys
            builder.HasOne(ou => ou.Location)
                .WithMany(l => l.Users)
                .HasForeignKey(ou => ou.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
            //builder.HasOne(u => u.Role)
            //    .WithMany(r => r.Users)
            //    .HasForeignKey(u => u.RoleId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }

    //public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    //{
    //    public void Configure(EntityTypeBuilder<UserRole> builder)
    //    {
    //        // table
    //        builder.ToTable("UserRole");
    //        // key
    //        builder.HasKey(ur => new { ur.UserId, ur.RoleId });
    //        // foreign keys
    //        builder.HasOne<User>(ur => ur.User)
    //            .WithMany(u => u.UserRoles)
    //            .HasForeignKey(ur => ur.UserId);
    //        builder.HasOne<Role>(ur => ur.Role)
    //            .WithMany(r => r.UserRoles)
    //            .HasForeignKey(ur => ur.RoleId);
    //    }
    //}

    public class ContactConfig : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            // table
            builder.ToTable("Contacts");
            // key
            builder.HasKey(c => c.ContactId);
            // columns
            builder.Property(c => c.ContactName)
                .HasMaxLength(50);
            builder.Property(c => c.ContactTitle)
                .HasMaxLength(50);
            builder.Property(c => c.ContactPhone)
                .HasMaxLength(25);
            builder.Property(c => c.ContactEmail)
                .HasMaxLength(255);
        }
    }

    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            // table
            builder.ToTable("Addresses");
            // key
            builder.HasKey(a => a.AddressId);
            // columns
            builder.Property(a => a.AddressLine1)
                .HasMaxLength(100);
            builder.Property(a => a.AddressLine2)
                .HasMaxLength(100);
            builder.Property(a => a.City)
                .HasMaxLength(25);
            builder.Property(a => a.State)
                .HasMaxLength(25);
            builder.Property(a => a.Country)
                .HasMaxLength(25);
            builder.Property(a => a.PostalCode)
                .HasMaxLength(10);
            builder.Ignore(a => a.AddressType);
            builder.Property(a => a.AddressTypeString)
                .HasColumnName("AddressType")
                .HasMaxLength(25)
                .IsRequired();
        }
    }

    public class BankAccountConfig : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            // table
            builder.ToTable("BankAccounts");
            // key
            builder.HasKey(b => b.BankAcctId);
            // columns
            builder.Property(b => b.AccountNumber)
                .IsRequired()
                .HasMaxLength(25);
            builder.Property(b => b.BankShortName)
                .IsRequired(false)
                .HasMaxLength(15);
            builder.Property(b => b.BankName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Ignore(b => b.AccountType);
            builder.Property(b => b.AccountTypeString)
                .HasColumnName("AccountType")
                .HasMaxLength(16);
            builder.Property(b => b.IfscCode)
                .HasMaxLength(16);
            builder.Property(b => b.MicrCode)
                .HasMaxLength(16);
            builder.Property(b => b.BranchCode)
                .HasMaxLength(16);
            builder.Property(b => b.BranchName)
                .HasMaxLength(50);
            //builder.Property(b => b.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(b => b.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(p => p.AccountNumber)
                .IsUnique();
            // foreign key
            builder.HasOne(b => b.Contact)
                .WithOne(c => c.BankAccount)
                .HasForeignKey<BankAccount>(a => a.ContactId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(b => b.Address)
                .WithOne(a => a.BankAccount)
                .HasForeignKey<BankAccount>(a => a.AddressId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    //public class BankContactConfig : IEntityTypeConfiguration<BankContact>
    //{
    //    public void Configure(EntityTypeBuilder<BankContact> builder)
    //    {
    //        // table
    //        builder.ToTable("BankContact");
    //        // key
    //        builder.HasKey(oc => new { oc.BankId, oc.ContactId });
    //        // foreign key
    //        builder.HasOne(oc => oc.Bank)
    //            .WithMany(b => b.Contacts)
    //            .HasForeignKey(oc => oc.BankId)
    //            .OnDelete(DeleteBehavior.Cascade);
    //        builder.HasOne(oc => oc.Contact)
    //            .WithMany(c => c.BankContacts)
    //            .HasForeignKey(oc => oc.ContactId)
    //            .OnDelete(DeleteBehavior.Cascade);
    //    }
    //}

    //public class BankAddressConfig : IEntityTypeConfiguration<BankAddress>
    //{
    //    public void Configure(EntityTypeBuilder<BankAddress> builder)
    //    {
    //        // table
    //        builder.ToTable("BankAddress");
    //        // key
    //        builder.HasKey(oa => new { oa.BankId, oa.AddressId });
    //        // foreign key
    //        builder.HasOne(oa => oa.Bank)
    //            .WithMany(b => b.Addresses)
    //            .HasForeignKey(oa => oa.BankId)
    //            .OnDelete(DeleteBehavior.Cascade);
    //        builder.HasOne(oa => oa.Address)
    //            .WithMany(c => c.BankAddresses)
    //            .HasForeignKey(oa => oa.AddressId)
    //            .OnDelete(DeleteBehavior.Cascade);
    //    }
    //}

    public class AuditTrailConfig : IEntityTypeConfiguration<AuditTrail>
    {
        public void Configure(EntityTypeBuilder<AuditTrail> builder)
        {
            // table
            builder.ToTable("AuditTrails");
            // key
            builder.HasKey(at => at.AuditTrailId);
            // columns
            builder.Property(at => at.TableName)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(32);
            builder.Ignore(at => at.CurrentStatus);
            builder.Property(at => at.CurrentStatusString)
                .HasColumnName("CurrentStatus")
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(16);
            // indexes
            builder.HasIndex(at => new { at.TableName, at.RowId })
                .IsUnique();
            // foreign key
            builder.HasOne(at => at.CreatedUser)
                .WithMany(u => u.CreatedTrails)
                .HasForeignKey(at => at.CreatedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(at => at.ModifiedUser)
                .WithMany(u => u.ModifiedTrails)
                .HasForeignKey(at => at.ModifiedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(at => at.DeletedUser)
                .WithMany(u => u.DeletedTrails)
                .HasForeignKey(at => at.DeletedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(at => at.PrintedUser)
                .WithMany(u => u.PrintedTrails)
                .HasForeignKey(at => at.PrintedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(at => at.ApprovedUser)
                .WithMany(u => u.ApprovedTrails)
                .HasForeignKey(at => at.ApprovedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(at => at.RejectedUser)
                .WithMany(u => u.RejectedTrails)
                .HasForeignKey(at => at.RejectedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(at => at.HoldUser)
                .WithMany(u => u.HoldTrails)
                .HasForeignKey(at => at.HoldUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(at => at.ActivatedUser)
                .WithMany(u => u.ActivatedTrails)
                .HasForeignKey(at => at.ActivatedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(at => at.DeactivatedUser)
                .WithMany(u => u.DeactivatedTrails)
                .HasForeignKey(at => at.DeactivatedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class OrganizationConfig : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            // table
            builder.ToTable("Organizations");
            // key
            builder.HasKey(o => o.OrgId);
            // columns
            builder.Property(o => o.OrgCode)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(o => o.OrgName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(o => o.Description)
                .HasMaxLength(255);
            builder.Property(o => o.RegdNum)
                .HasMaxLength(25);
            builder.Property(o => o.PAN)
                .HasMaxLength(25);
            builder.Property(o => o.TAN)
                .HasMaxLength(25);
            builder.Property(o => o.GSTIN)
                .HasMaxLength(25);
            //builder.Property(o => o.DateCreated)
            //    .HasDefaultValueSql("getdate()")
            //    .ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified)
            //    .HasDefaultValueSql("getdate()")
            //    .ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(o => o.OrgCode)
                .IsUnique();
            builder.HasIndex(o => o.OrgName)
                .IsUnique();
        }
    }

    public class OrgContactConfig : IEntityTypeConfiguration<OrgContact>
    {
        public void Configure(EntityTypeBuilder<OrgContact> builder)
        {
            // table
            builder.ToTable("OrgContact");
            // key
            builder.HasKey(oc => new { oc.OrgId, oc.ContactId });
            // foreign key
            builder.HasOne(oc => oc.Organization)
                .WithMany(b => b.Contacts)
                .HasForeignKey(oc => oc.OrgId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(oc => oc.Contact)
                .WithMany(c => c.OrgContacts)
                .HasForeignKey(oc => oc.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class OrgAddressConfig : IEntityTypeConfiguration<OrgAddress>
    {
        public void Configure(EntityTypeBuilder<OrgAddress> builder)
        {
            // table
            builder.ToTable("OrgAddress");
            // key
            builder.HasKey(oa => new { oa.OrgId, oa.AddressId });
            // foreign key
            builder.HasOne(oa => oa.Organization)
                .WithMany(b => b.Addresses)
                .HasForeignKey(oa => oa.OrgId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(oa => oa.Address)
                .WithMany(c => c.OrgAddresses)
                .HasForeignKey(oa => oa.AddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class OrgUserConfig : IEntityTypeConfiguration<OrgUser>
    {
        public void Configure(EntityTypeBuilder<OrgUser> builder)
        {
            // table
            builder.ToTable("OrgUser");
            // key
            builder.HasKey(ou => new { ou.OrgId, ou.UserId });
            // foreign key
            builder.HasOne(ou => ou.Organization)
                .WithMany(b => b.Users)
                .HasForeignKey(ou => ou.OrgId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ou => ou.User)
                .WithMany(c => c.Organizations)
                .HasForeignKey(ou => ou.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class OrgUserRoleConfig : IEntityTypeConfiguration<OrgUserRole>
    {
        public void Configure(EntityTypeBuilder<OrgUserRole> builder)
        {
            // table
            builder.ToTable("OrgUserRole");
            // key
            builder.HasKey(our => new { our.OrgId, our.UserId, our.RoleId });
            // foreign key
            builder.HasOne<OrgUser>(our => our.OrgUser)
                .WithMany(ou => ou.OrgUserRoles)
                .HasForeignKey(our => new { our.OrgId, our.UserId });
            builder.HasOne<Role>(our => our.Role)
                .WithMany(r => r.OrgUserRoles)
                .HasForeignKey(our => our.RoleId);
        }
    }

    public class OrgBankConfig : IEntityTypeConfiguration<OrgBank>
    {
        public void Configure(EntityTypeBuilder<OrgBank> builder)
        {
            // table
            builder.ToTable("OrgBank");
            // key
            builder.HasKey(ou => new { ou.OrgId, ou.BankAcctId });
            // foreign key
            builder.HasOne(ou => ou.Organization)
                .WithMany(b => b.Banks)
                .HasForeignKey(ou => ou.OrgId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ou => ou.BankAccount)
                .WithMany(c => c.OrgBanks)
                .HasForeignKey(ou => ou.BankAcctId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class LocationConfig : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            // table
            builder.ToTable("Locations");
            // key
            builder.HasKey(l => l.LocationId);
            // columns
            builder.Property(l => l.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(l => l.Description)
                .HasMaxLength(255);
            //builder.Property(o => o.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(l => new { l.Code, l.OrgId })
                .IsUnique();
            builder.HasIndex(l => new { l.Name, l.OrgId })
                .IsUnique();
            // foreign key
            builder.HasOne(l => l.Organization)
                .WithMany(o => o.Locations)
                .HasForeignKey(l => l.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class DeviceConfig : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            // table
            builder.ToTable("Devices");
            // key
            builder.HasKey(d => d.DeviceId);
            // columns
            builder.Property(d => d.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(l => l.Description)
                .HasMaxLength(255);
            //builder.Property(o => o.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(d => new { d.Code, d.LocationId })
                .IsUnique();
            builder.HasIndex(d => new { d.Name, d.LocationId })
                .IsUnique();
            // foreign key
            builder.HasOne(d => d.Location)
                .WithMany(l => l.Devices)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class PartyCategoryConfig : IEntityTypeConfiguration<PartyCategory>
    {
        public void Configure(EntityTypeBuilder<PartyCategory> builder)
        {
            // table
            builder.ToTable("PartyCategories");
            // key
            builder.HasKey(pc => pc.CategoryId);
            // columns
            builder.Property(pc => pc.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(pc => pc.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(pc => pc.Description)
                .HasMaxLength(255);
            //builder.Property(pc => pc.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(pc => pc.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(pc => new { pc.Code, pc.OrgId })
                .IsUnique();
            builder.HasIndex(pc => new { pc.Name, pc.OrgId })
                .IsUnique();
            // foreign key
            builder.HasOne(pc => pc.Organization)
                .WithMany(o => o.PartyCategories)
                .HasForeignKey(pc => pc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class PartyConfig : IEntityTypeConfiguration<Party>
    {
        public void Configure(EntityTypeBuilder<Party> builder)
        {
            // table
            builder.ToTable("Parties");
            // key
            builder.HasKey(c => c.PartyId);
            // columns
            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Ignore(pc => pc.PartyType);
            builder.Property(pc => pc.PartyTypeString)
                .HasColumnName("PartyType")
                .HasMaxLength(16);
            builder.Property(o => o.RegdNum)
                .HasMaxLength(25);
            builder.Property(o => o.PAN)
                .HasMaxLength(25);
            builder.Property(o => o.TAN)
                .HasMaxLength(25);
            builder.Property(o => o.GSTIN)
                .HasMaxLength(25);
            builder.Ignore(s => s.PaymentTerm);
            builder.Property(s => s.PaymentTermString)
                .HasColumnName("PaymentTerm")
                .HasMaxLength(16);
            //builder.Property(p => p.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(p => p.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(c => new { c.Code, c.OrgId })
                .IsUnique();
            builder.HasIndex(c => new { c.Name, c.OrgId })
                .IsUnique();
            // foreign key
            builder.HasOne(p => p.Organization)
                .WithMany(o => o.Parties)
                .HasForeignKey(p => p.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Category)
                .WithMany(pc => pc.Parties)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class PartyContactConfig : IEntityTypeConfiguration<PartyContact>
    {
        public void Configure(EntityTypeBuilder<PartyContact> builder)
        {
            // table
            builder.ToTable("PartyContact");
            // key
            builder.HasKey(oc => new { oc.PartyId, oc.ContactId });
            // foreign key
            builder.HasOne(oc => oc.Party)
                .WithMany(b => b.Contacts)
                .HasForeignKey(oc => oc.PartyId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(oc => oc.Contact)
                .WithMany(c => c.PartyContacts)
                .HasForeignKey(oc => oc.ContactId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class PartyAddressConfig : IEntityTypeConfiguration<PartyAddress>
    {
        public void Configure(EntityTypeBuilder<PartyAddress> builder)
        {
            // table
            builder.ToTable("PartyAddress");
            // key
            builder.HasKey(oa => new { oa.PartyId, oa.AddressId });
            // foreign key
            builder.HasOne(oa => oa.Party)
                .WithMany(b => b.Addresses)
                .HasForeignKey(oa => oa.PartyId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(oa => oa.Address)
                .WithMany(c => c.PartyAddresses)
                .HasForeignKey(oa => oa.AddressId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class PartyBankConfig : IEntityTypeConfiguration<PartyBank>
    {
        public void Configure(EntityTypeBuilder<PartyBank> builder)
        {
            // table
            builder.ToTable("PartyBank");
            // key
            builder.HasKey(ou => new { ou.PartyId, ou.BankAcctId });
            // foreign key
            builder.HasOne(ou => ou.Party)
                .WithMany(b => b.Banks)
                .HasForeignKey(ou => ou.PartyId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ou => ou.BankAccount)
                .WithMany(c => c.PartyBanks)
                .HasForeignKey(ou => ou.BankAcctId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class UnitConfig : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> builder)
        {
            // table
            builder.ToTable("Units");
            // key
            builder.HasKey(u => u.UnitId);
            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(25);
            //builder.Property(u => u.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(u => u.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();
            // foreign key
            builder.HasOne(u => u.Organization)
                .WithMany(o => o.Units)
                .HasForeignKey(u => u.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class UnitConversionConfig : IEntityTypeConfiguration<UnitConversion>
    {
        public void Configure(EntityTypeBuilder<UnitConversion> builder)
        {
            // table
            builder.ToTable("UnitConversions");
            // key
            builder.HasKey(u => u.ConversionId);
            // columns
            // index
            // foreign key
            builder.HasOne(uc => uc.Organization)
                .WithMany(o => o.UnitConversions)
                .HasForeignKey(uc => uc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(uc => uc.FromUnit)
                .WithMany(u => u.FromUnitConversions)
                .HasForeignKey(uc => uc.FromUnitId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(uc => uc.ToUnit)
                .WithMany(u => u.ToUnitConversions)
                .HasForeignKey(uc => uc.ToUnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            // table
            builder.ToTable("ProductCategories");
            // key
            builder.HasKey(u => u.CategoryId);
            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            //builder.Property(pc => pc.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(pc => pc.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();
            // foreign key
            builder.HasOne(pc => pc.Organization)
                .WithMany(o => o.ProductCategories)
                .HasForeignKey(pc => pc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pc => pc.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(pc => pc.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductBrandConfig : IEntityTypeConfiguration<ProductBrand>
    {
        public void Configure(EntityTypeBuilder<ProductBrand> builder)
        {
            // table
            builder.ToTable("ProductBrands");
            // key
            builder.HasKey(u => u.BrandId);
            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            //builder.Property(b => b.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(b => b.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();
            // foreign key
            builder.HasOne(pc => pc.Organization)
                .WithMany(o => o.ProductBrands)
                .HasForeignKey(pc => pc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductSegmentConfig : IEntityTypeConfiguration<ProductSegment>
    {
        public void Configure(EntityTypeBuilder<ProductSegment> builder)
        {
            // table
            builder.ToTable("ProductSegments");
            // key
            builder.HasKey(u => u.SegmentId);
            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            //builder.Property(s => s.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(s => s.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();
            // foreign key
            builder.HasOne(pc => pc.Organization)
                .WithMany(o => o.ProductSegments)
                .HasForeignKey(pc => pc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductVarietyConfig : IEntityTypeConfiguration<ProductVariety>
    {
        public void Configure(EntityTypeBuilder<ProductVariety> builder)
        {
            // table
            builder.ToTable("ProductVarieties");
            // key
            builder.HasKey(u => u.VarietyId);
            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            //builder.Property(v => v.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(v => v.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();
            // foreign key
            builder.HasOne(pc => pc.Organization)
                .WithMany(o => o.ProductVarieties)
                .HasForeignKey(pc => pc.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // table
            builder.ToTable("Products");
            // key
            builder.HasKey(u => u.ProductId);
            // columns
            builder.Property(u => u.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(u => u.Description)
                .HasMaxLength(255);
            builder.Ignore(p => p.ProductType);
            builder.Property(p => p.ProductTypeString)
                .HasColumnName("ProductType")
                .HasMaxLength(16);
            builder.Ignore(p => p.TaxPreference);
            builder.Property(p => p.TaxPreferenceString)
                .HasColumnName("TaxPreference")
                .HasMaxLength(16);
            //builder.Property(p => p.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(p => p.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(p => new { p.Code, p.OrgId })
                .IsUnique();
            builder.HasIndex(p => new { p.Name, p.OrgId })
                .IsUnique();
            // foreign key
            builder.HasOne(p => p.Organization)
                .WithMany(o => o.Products)
                .HasForeignKey(p => p.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Category)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Unit)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Segment)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.SegmentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Variety)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.VarietyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductIngredientConfig : IEntityTypeConfiguration<ProductIngredient>
    {
        public void Configure(EntityTypeBuilder<ProductIngredient> builder)
        {
            // table
            builder.ToTable("ProductIngredients");
            // key
            builder.HasKey(i => i.IngredientId);
            // index
            builder.HasIndex(i => new { i.ParentProductId, i.ChildProductId })
                .IsUnique();
            // foreign key
            builder.HasOne(i => i.ParentProduct)
                .WithMany(p => p.ParentIngredients)
                .HasForeignKey(i => i.ParentProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(i => i.ChildProduct)
                .WithMany(p => p.ChildIngredients)
                .HasForeignKey(i => i.ChildProductId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(i => i.Unit)
                .WithMany(u => u.ProductIngredients)
                .HasForeignKey(i => i.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductPricingConfig : IEntityTypeConfiguration<ProductPricing>
    {
        public void Configure(EntityTypeBuilder<ProductPricing> builder)
        {
            // table
            builder.ToTable("ProductPricings");
            // key
            builder.HasKey(pp => pp.PricingId);
            // columns
            //builder.Ignore(pp => pp.InvoiceCategory);
            //builder.Property(pp => pp.InvoiceCategoryString)
            //    .HasColumnName("InvoiceCategory")
            //    .HasMaxLength(16);
            // foreign key
            builder.HasOne(pp => pp.Product)
                .WithMany(p => p.ProductPricings)
                .HasForeignKey(pp => pp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(pp => pp.PartyCategory)
                .WithMany(pc => pc.ProductPricings)
                .HasForeignKey(pp => pp.PartyCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pp => pp.InvoiceCategory)
                .WithMany(ic => ic.ProductPricings)
                .HasForeignKey(pp => pp.InvoiceCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class ProductPrincingRangeConfig : IEntityTypeConfiguration<ProductPricingRange>
    {
        public void Configure(EntityTypeBuilder<ProductPricingRange> builder)
        {
            // table
            builder.ToTable("ProductPricingRanges");
            // key
            builder.HasKey(r => r.RangeId);
            // columns
            builder.Ignore(r => r.RoundOffOption);
            builder.Property(r => r.RoundOffOptionString)
                .HasColumnName("RoundOffOption")
                .HasMaxLength(16);
            // foreign key
            builder.HasOne(r => r.ProductPricing)
                .WithMany(pp => pp.PricingRanges)
                .HasForeignKey(r => r.PricingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class InvoiceCategoryConfig : IEntityTypeConfiguration<InvoiceCategory>
    {
        public void Configure(EntityTypeBuilder<InvoiceCategory> builder)
        {
            // table
            builder.ToTable("InvoiceCategories");
            // key
            builder.HasKey(ic => ic.CategoryId);
            // columns
            builder.Property(ic => ic.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(ic => ic.Name)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(ic => ic.Description)
                .HasMaxLength(255);
            //builder.Property(c => c.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(c => c.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(ic => new { ic.Code, ic.OrgId })
                .IsUnique();
            builder.HasIndex(ic => new { ic.Name, ic.OrgId })
                .IsUnique();
            // foreign key
            builder.HasOne(ic => ic.Organization)
                .WithMany(o => o.InvoiceCategories)
                .HasForeignKey(ic => ic.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class InvoiceConfig : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            // table
            builder.ToTable("Invoices");
            // key
            builder.HasKey(i => i.InvoiceId);
            // columns
            builder.Property(i => i.InvoiceNumber)
                .HasMaxLength(16);
            builder.Ignore(i => i.InvoiceType);
            builder.Property(i => i.InvoiceTypeString)
                .HasColumnName("InvoiceType")
                .HasMaxLength(16);
            builder.Property(i => i.Notes)
                .HasMaxLength(255);
            builder.Property(i => i.PartyRefNum)
                .HasMaxLength(16);
            builder.Ignore(i => i.StockInvoiceType);
            builder.Property(i => i.StockInvoiceTypeString)
                .HasColumnName("StockInvoiceType")
                .HasMaxLength(16);
            builder.Ignore(i => i.GstTreatment);
            builder.Property(i => i.GstTreatmentString)
                .HasColumnName("GstTreatment")
                .HasMaxLength(16);
            builder.Ignore(i => i.TaxPreference);
            builder.Property(i => i.TaxPreferenceString)
                .HasColumnName("TaxPreference")
                .HasMaxLength(16);

            //builder.Property(o => o.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified).ValueGeneratedOnUpdate();
            // index
            builder.HasIndex(i => new { i.InvoiceNumber, i.OrgId })
                .IsUnique();
            // foreign key
            builder.HasOne(i => i.Organization)
                .WithMany(o => o.Invoices)
                .HasForeignKey(i => i.OrgId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(i => i.Party)
                .WithMany(p => p.Invoices)
                .HasForeignKey(i => i.PartyId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(i => i.Category)
                .WithMany(ic => ic.Invoices)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(i => i.Buyer)
                .WithMany(b => b.BuyerInvoices)
                .HasForeignKey(i => i.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class InvoiceItemConfig : IEntityTypeConfiguration<InvoiceItem>
    {
        public void Configure(EntityTypeBuilder<InvoiceItem> builder)
        {
            // table
            builder.ToTable("InvoiceItems");
            // key
            builder.HasKey(i => i.InvItemId);
            // columns
            builder.Property(i => i.Description)
                .HasMaxLength(255);
            // foreign key
            builder.HasOne(ii => ii.Invoice)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ii => ii.Stock)
                .WithMany(i => i.InvoiceItems)
                .HasForeignKey(ii => ii.StockId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(ii => ii.Unit)
                .WithMany(u => u.InvoiceItems)
                .HasForeignKey(ii => ii.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class StockConfig : IEntityTypeConfiguration<Stock>
    {
        public void Configure(EntityTypeBuilder<Stock> builder)
        {
            // table
            builder.ToTable("Stock");
            // key
            builder.HasKey(s => s.StockId);
            // columns
            builder.Property(s => s.BatchNumber)
                .HasMaxLength(25);
            // index
            builder.HasIndex(s => new { s.BatchNumber, s.ProductId })
                .IsUnique();
            // foreign key
            builder.HasOne(s => s.Product)
                .WithMany(p => p.Stocks)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(s => s.Unit)
                .WithMany(u => u.Stocks)
                .HasForeignKey(s => s.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}