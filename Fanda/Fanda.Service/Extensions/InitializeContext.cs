using Fanda.Data.Context;
//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
//using MySql.Data.EntityFrameworkCore.Extensions;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;
using System;

// DbContextOptionsBuilder.EnableSensitiveDataLogging

namespace Fanda.Service.Extensions
{
    public static class InitializeContextExtensions
    {
        public static void InitializeContext(this IServiceCollection services, string databaseType, string connectionString)
        {
            switch (databaseType)
            {
                case "MSSQL":
                    //services.AddDbContext<FandaContext>(options =>
                    //{
                    //    options.UseSqlServer(connectionString);
                    //});
                    services.AddEntityFrameworkSqlServer()
                        .AddDbContextPool<FandaContext>((serviceProvider, options) =>
                        {
                            options.UseSqlServer(connectionString, sqlOptions =>
                            {
                                sqlOptions.EnableRetryOnFailure();
                                //sqlopt.UseRowNumberForPaging();
                            })
                            .UseInternalServiceProvider(serviceProvider);

                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                            options.EnableDetailedErrors();
                            options.EnableSensitiveDataLogging();
                            options.EnableServiceProviderCaching();
                        });
                    break;
                case "MYSQL":
                    services.AddEntityFrameworkMySql()
                        .AddDbContextPool<FandaContext>((serviceProvider, options) =>
                        {
                            options.UseMySql(connectionString, mysqlOptions =>
                            {
                                mysqlOptions.ServerVersion(new ServerVersion(new Version(10, 4), ServerType.MariaDb));
                            })
                            .UseInternalServiceProvider(serviceProvider);

                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                            options.EnableDetailedErrors();
                            options.EnableSensitiveDataLogging();
                            options.EnableServiceProviderCaching();
                        });
                    break;
                case "PGSQL":
                    services.AddEntityFrameworkNpgsql()
                        .AddDbContextPool<FandaContext>((serviceProvider, options) =>
                        {
                            options.UseNpgsql(connectionString, pgsqlOptions =>
                            {
                                pgsqlOptions.EnableRetryOnFailure();
                            })
                            .UseInternalServiceProvider(serviceProvider);

                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                            options.EnableDetailedErrors();
                            options.EnableSensitiveDataLogging();
                            options.EnableServiceProviderCaching();
                        });
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
    }
}