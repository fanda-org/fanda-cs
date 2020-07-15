using Fanda.Entities.Context;
using Fanda.Infrastructure.SqlClients;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;
using System.Reflection;

namespace Fanda.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddFandaDbContextPool<TDbContext>(this IServiceCollection services,
            AppSettings settings, string migrationsAssemblyName)
            where TDbContext : DbContext
        {
            switch (settings.DatabaseType)
            {
                case "MSSQL":
                    services.AddEntityFrameworkSqlServer()
                        .AddDbContextPool<TDbContext>((sp, options) =>
                        {
                            MsSqlOptions(sp, options, settings.ConnectionStrings.MsSqlConnection,
                                migrationsAssemblyName);
                        });
                    services.AddTransient<IDbClient>(_ => new SqlServerClient(settings.ConnectionStrings.MsSqlConnection));
                    break;
                case "MYSQL":
                    services.AddEntityFrameworkMySql()
                        .AddDbContextPool<TDbContext>((sp, options) =>
                        {
                            MySqlOptions(sp, options, settings.ConnectionStrings.MySqlConnection,
                                migrationsAssemblyName);
                        });
                    services.AddTransient<IDbClient>(_ => new MySqlClient(settings.ConnectionStrings.MySqlConnection));
                    break;
                case "PGSQL":
                    services.AddEntityFrameworkNpgsql()
                        .AddDbContextPool<TDbContext>((sp, options) =>
                        {
                            PgSqlOptions(sp, options, settings.ConnectionStrings.PgSqlConnection,
                                migrationsAssemblyName);
                        });
                    services.AddTransient<IDbClient>(_ => new PgSqlClient(settings.ConnectionStrings.PgSqlConnection));
                    break;
                default:
                    services.AddEntityFrameworkMySql()
                        .AddDbContextPool<TDbContext>((sp, options) =>
                        {
                            MySqlOptions(sp, options, settings.ConnectionStrings.MySqlConnection,
                                migrationsAssemblyName);
                        });
                    services.AddTransient<IDbClient>(_ => new MySqlClient(settings.ConnectionStrings.MySqlConnection));
                    break;
            }

            //services.AddIdentity<User, Role>(options =>
            //{
            //    //options.SignIn.RequireConfirmedEmail = true;
            //    // Password settings
            //    options.Password.RequireDigit = true;
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequiredUniqueChars = 3;

            //    // Lockout settings
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;

            //    // User settings
            //    options.User.RequireUniqueEmail = true;
            //})
            //.AddEntityFrameworkStores<FandaContext>()
            //.AddDefaultTokenProviders();

            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()
            //    .AddInMemoryPersistedGrants()
            //    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            //    .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
            //    .AddInMemoryClients(IdentityServerConfig.GetClients())
            //    .AddAspNetIdentity<User>();
        }

        public static DbContextOptionsBuilder CreateDbContextOptionsBuilder<TDbContext>(AppSettings settings,
            string migrationsAssemblyName)
            where TDbContext : DbContext
        {
            var options = new DbContextOptionsBuilder<TDbContext>();
            switch (settings.DatabaseType)
            {
                case "MSSQL":
                    MsSqlOptions(null, options, settings.ConnectionStrings.MsSqlConnection,
                        migrationsAssemblyName);
                    break;
                case "MYSQL":
                    MySqlOptions(null, options, settings.ConnectionStrings.MySqlConnection,
                        migrationsAssemblyName);
                    break;
                case "PGSQL":
                    PgSqlOptions(null, options, settings.ConnectionStrings.PgSqlConnection,
                        migrationsAssemblyName);
                    break;
                default:
                    MsSqlOptions(null, options, settings.ConnectionStrings.DefaultConnection,
                        migrationsAssemblyName);
                    break;
            }
            return options;
        }

        private static void MsSqlOptions(IServiceProvider sp, DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName)
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                //sqlOptions.EnableRetryOnFailure();
                //sqlopt.UseRowNumberForPaging();
                sqlOptions.MigrationsAssembly(migrationsAssemblyName);
            })
            .UseInternalServiceProvider(sp);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableDetailedErrors(true);
            options.EnableSensitiveDataLogging(true);
            options.EnableServiceProviderCaching();
        }

        private static void MySqlOptions(IServiceProvider sp, DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName)
        {
            options.UseMySql(connectionString, mysqlOptions =>
            {
                mysqlOptions.MigrationsAssembly(migrationsAssemblyName);
                mysqlOptions.ServerVersion(new ServerVersion(new Version(10, 4), ServerType.MariaDb));
            })
            .UseInternalServiceProvider(sp);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.EnableServiceProviderCaching();
        }

        private static void PgSqlOptions(IServiceProvider sp, DbContextOptionsBuilder options,
            string connectionString, string migrationsAssemblyName)
        {
            options.UseNpgsql(connectionString, pgsqlOptions =>
            {
                pgsqlOptions.MigrationsAssembly(migrationsAssemblyName);
                pgsqlOptions.EnableRetryOnFailure();
            })
            .UseInternalServiceProvider(sp);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
            options.EnableServiceProviderCaching();
        }
    }
}