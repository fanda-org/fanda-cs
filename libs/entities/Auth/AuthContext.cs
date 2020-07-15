using Fanda.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace Fanda.Entities.Auth
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options)
            : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Action> Actions { get; set; }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ApplicationConfig());
            modelBuilder.ApplyConfiguration(new ResourceConfig());
            modelBuilder.ApplyConfiguration(new ActionConfig());

            modelBuilder.ApplyConfiguration(new AppResourceConfig());
            modelBuilder.ApplyConfiguration(new ResourceActionConfig());
            modelBuilder.ApplyConfiguration(new PrivilegeConfig());

            modelBuilder.ApplyConfiguration(new TenantConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
        }
    }
}