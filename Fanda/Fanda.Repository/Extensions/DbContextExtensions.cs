using Fanda.Models.Context;
using Fanda.Repository.SqlClients;
using Fanda.Shared;
//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
//using MySql.Data.EntityFrameworkCore.Extensions;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;

// DbContextOptionsBuilder.EnableSensitiveDataLogging

namespace Fanda.Repository.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddFandaDbContextPool(this IServiceCollection services, AppSettings settings)
        {
            switch (settings.DatabaseType)
            {
                case "MSSQL":
                    //services.AddEntityFrameworkSqlServer()
                    services.AddDbContextPool<FandaContext>(options =>
                    {
                        MsSqlOptions(options, settings.ConnectionStrings.MsSqlConnection);
                    });
                    services.AddTransient<IDbClient>(_ => new SqlServerClient(settings.ConnectionStrings.MsSqlConnection));
                    break;
                case "MYSQL":
                    //services.AddEntityFrameworkMySql()
                    services.AddDbContextPool<FandaContext>((_, options) =>
                    {
                        MySqlOptions(options, settings.ConnectionStrings.MySqlConnection);
                    });
                    services.AddTransient<IDbClient>(_ => new MySqlClient(settings.ConnectionStrings.MySqlConnection));
                    break;
                case "PGSQL":
                    //services.AddEntityFrameworkNpgsql()
                    services.AddDbContextPool<FandaContext>((_, options) =>
                    {
                        PgSqlOptions(options, settings.ConnectionStrings.PgSqlConnection);
                    });
                    services.AddTransient<IDbClient>(_ => new PgSqlClient(settings.ConnectionStrings.PgSqlConnection));
                    break;
                default:
                    //services.AddEntityFrameworkSqlServer()
                    services.AddDbContextPool<FandaContext>(options =>
                    {
                        MsSqlOptions(options, settings.ConnectionStrings.DefaultConnection);
                    });
                    services.AddTransient<IDbClient>(_ => new SqlServerClient(settings.ConnectionStrings.DefaultConnection));
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

        public static DbContextOptionsBuilder<FandaContext> CreateDbContextOptionsBuilder(AppSettings settings)
        {
            var options = new DbContextOptionsBuilder<FandaContext>();
            switch (settings.DatabaseType)
            {
                case "MSSQL":
                    MsSqlOptions(options, settings.ConnectionStrings.MsSqlConnection);
                    break;
                case "MYSQL":
                    MySqlOptions(options, settings.ConnectionStrings.MySqlConnection);
                    break;
                case "PGSQL":
                    PgSqlOptions(options, settings.ConnectionStrings.PgSqlConnection);
                    break;
                default:
                    MsSqlOptions(options, settings.ConnectionStrings.DefaultConnection);
                    break;
            }
            return options;
        }

        private static void MsSqlOptions(DbContextOptionsBuilder options, string connectionString)
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                //sqlOptions.EnableRetryOnFailure();
                //sqlopt.UseRowNumberForPaging();
            });
            //.UseInternalServiceProvider(serviceProvider);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //options.EnableDetailedErrors();
            //options.EnableSensitiveDataLogging();
            //options.EnableServiceProviderCaching();
        }

        private static void MySqlOptions(DbContextOptionsBuilder options, string connectionString)
        {
            options.UseMySql(connectionString, mysqlOptions =>
            {
                mysqlOptions.ServerVersion(new ServerVersion(new Version(10, 4), ServerType.MariaDb));
            });
            //.UseInternalServiceProvider(serviceProvider);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //options.EnableDetailedErrors();
            //options.EnableSensitiveDataLogging();
            //options.EnableServiceProviderCaching();
        }

        private static void PgSqlOptions(DbContextOptionsBuilder options, string connectionString)
        {
            options.UseNpgsql(connectionString, pgsqlOptions =>
            {
                pgsqlOptions.EnableRetryOnFailure();
            });
            //.UseInternalServiceProvider(serviceProvider);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //options.EnableDetailedErrors();
            //options.EnableSensitiveDataLogging();
            //options.EnableServiceProviderCaching();
        }
    }
}