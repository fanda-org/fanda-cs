namespace Fanda.Entities.Auth
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    #region  Application configuration
    public class ApplicationConfig : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            // table
            builder.ToTable("Applications");

            // key
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(t => t.Code)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(16);
            builder.Property(t => t.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            builder.Property(t => t.Description)
                .IsUnicode(false)
                .HasMaxLength(255);
            builder.Property(a => a.Edition)
                .IsUnicode(false)
                .HasMaxLength(25);
            builder.Property(a => a.Version)
                .IsUnicode(false)
                .HasMaxLength(16);

            // index
            builder.HasIndex(a => a.Code)
                .IsUnique();
            builder.HasIndex(a => a.Name)
                .IsUnique();
        }
    }

    public class ResourceConfig : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            // table
            builder.ToTable("Resources");

            // key
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(t => t.Code)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(16);
            builder.Property(t => t.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            builder.Property(t => t.Description)
                .IsUnicode(false)
                .HasMaxLength(255);
            builder.Ignore(r => r.ResourceType);
            builder.Property(r => r.ResourceTypeString)
                .HasColumnName("ResourceType")
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(16);

            // index
            builder.HasIndex(a => a.Code)
                .IsUnique();
            builder.HasIndex(a => a.Name)
                .IsUnique();
        }
    }

    public class ActionConfig : IEntityTypeConfiguration<Action>
    {
        public void Configure(EntityTypeBuilder<Action> builder)
        {
            // table
            builder.ToTable("Actions");

            // key
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(t => t.Code)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(16);
            builder.Property(t => t.Name)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            builder.Property(t => t.Description)
                .IsUnicode(false)
                .HasMaxLength(255);

            // index
            builder.HasIndex(a => a.Code)
                .IsUnique();
            builder.HasIndex(a => a.Name)
                .IsUnique();
        }
    }

    public class AppResourceConfig : IEntityTypeConfiguration<AppResource>
    {
        public void Configure(EntityTypeBuilder<AppResource> builder)
        {
            // table
            builder.ToTable("AppResources");

            // key
            builder.HasKey(ar => ar.Id);
            builder.Property(ar => ar.Id).ValueGeneratedOnAdd();

            // index
            builder.HasIndex(ar => new { ar.ApplicationId, ar.ResourceId })
                .IsUnique();

            // foreign key
            builder.HasOne(ar => ar.Application)
                .WithMany(a => a.AppResources)
                .HasForeignKey(ar => ar.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(r => r.Resource)
                .WithMany(a => a.AppResources)
                .HasForeignKey(r => r.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class ResourceActionConfig : IEntityTypeConfiguration<ResourceAction>
    {
        public void Configure(EntityTypeBuilder<ResourceAction> builder)
        {
            // table
            builder.ToTable("ResourceActions");

            // key
            builder.HasKey(ar => new { ar.Id });
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            // index
            builder.HasIndex(ar => new { ar.ResourceId, ar.ActionId })
                .IsUnique();

            // foreign keys
            builder.HasOne(ar => ar.Resource)
                .WithMany(r => r.ResourceActions)
                .HasForeignKey(ar => ar.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(ar => ar.Action)
                .WithMany(r => r.ResourceActions)
                .HasForeignKey(ar => ar.ActionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    #endregion

    #region Privileges
    public class PrivilegeConfig : IEntityTypeConfiguration<Privilege>
    {
        public void Configure(EntityTypeBuilder<Privilege> builder)
        {
            // table
            builder.ToTable("Privileges");

            // key
            builder.HasKey(p => new { p.RoleId, p.AppResourceId, p.ResourceActionId });

            // index

            // foreign keys
            builder.HasOne(p => p.Role)
                .WithMany(r => r.Privileges)
                .HasForeignKey(p => new { p.RoleId })
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.AppResource)
                .WithMany(ra => ra.Privileges)
                .HasForeignKey(p => new { p.AppResourceId })
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(p => p.ResourceAction)
                .WithMany(ra => ra.Privileges)
                .HasForeignKey(p => new { p.ResourceActionId })
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
    #endregion

    #region  Tenants configuration
    public class TenantConfig : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            // table
            builder.ToTable("Tenants");

            // key
            builder.HasKey(t => t.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(16);
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(t => t.Description)
                .HasMaxLength(255);

            // index
            builder.HasIndex(a => a.Code)
                .IsUnique();
            builder.HasIndex(a => a.Name)
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
            builder.HasKey(u => new { u.Id });
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(25);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .IsUnicode(false)
                .IsFixedLength()
                .HasMaxLength(255);
            builder.Property(u => u.PasswordSalt)
                .IsRequired()
                .IsUnicode(false)
                .IsFixedLength()
                .HasMaxLength(255);
            builder.Property(u => u.FirstName)
                .HasMaxLength(50);
            builder.Property(u => u.LastName)
                .HasMaxLength(50);
            //builder.Property(o => o.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified).ValueGeneratedOnUpdate();

            // index
            builder.HasIndex(u => u.Name)
                .IsUnique();
            builder.HasIndex(u => u.Email)
                .IsUnique();

            // Foreign keys - Owns
            //builder.OwnsMany(u => u.RefreshTokens);
            builder.HasOne(u => u.Tenant)
                .WithMany(t => t.Users)
                .HasForeignKey(u => u.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // table
            builder.ToTable("RefreshTokens");

            // key
            builder.HasKey(u => new { u.Id });
            builder.Property(t => t.Id).ValueGeneratedOnAdd();

            // columns
            builder.Property(u => u.Token)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(100);
            builder.Property(u => u.CreatedByIp)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(50);
            builder.Property(u => u.RevokedByIp)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(50);
            builder.Property(u => u.ReplacedByToken)
                .IsRequired(false)
                .IsUnicode(false)
                .HasMaxLength(100);
            //builder.Property(o => o.DateCreated).ValueGeneratedOnAdd();
            //builder.Property(o => o.DateModified).ValueGeneratedOnUpdate();

            // Foreign keys - Owns
            builder.HasOne(u => u.User)
                .WithMany(t => t.RefreshTokens)
                .HasForeignKey(u => new { u.UserId })
                .OnDelete(DeleteBehavior.Cascade);

            // index
        }
    }

    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // table
            builder.ToTable("Roles");

            // key
            builder.HasKey(r => new { r.Id });
            builder.Property(a => a.Id).ValueGeneratedOnAdd();

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
            builder.HasIndex(r => new { r.Code, r.TenantId })
                .IsUnique();
            builder.HasIndex(r => new { r.Name, r.TenantId })
                .IsUnique();

            // foreign key
            builder.HasOne(r => r.Tenant)
                .WithMany(o => o.Roles)
                .HasForeignKey(r => r.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
    #endregion
}