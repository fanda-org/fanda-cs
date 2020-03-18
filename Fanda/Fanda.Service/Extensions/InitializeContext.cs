using Fanda.Data.Context;
//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace Fanda.Service.Extensions
{
    public static class InitializeContextExtensions
    {
        public static void InitializeContext(this IServiceCollection services, string databaseType, string connectionString)
        {
            switch (databaseType)
            {
                case "MSSQL":
                    services.AddEntityFrameworkSqlServer()
                        .AddDbContextPool<FandaContext>((serviceProvider, options) =>
                        {
                            options.UseSqlServer(connectionString, sqlopt =>
                            {
                                sqlopt.EnableRetryOnFailure();
                                //sqlopt.UseRowNumberForPaging();
                            })
                            .UseInternalServiceProvider(serviceProvider);

                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                            //options.ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));
                            //options.ConfigureWarnings(w => w.Throw(CoreEventId.IncludeIgnoredWarning));
                        });
                    break;
                case "MYSQL":
                    services.AddEntityFrameworkMySql()
                        .AddDbContextPool<FandaContext>((serviceProvider, options) =>
                        {
                            options.UseMySql(connectionString, mysqlopt =>
                            {
                                mysqlopt.ServerVersion(new Version(15, 1), ServerType.MariaDb);
                            })
                            .UseInternalServiceProvider(serviceProvider);

                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                            //options.ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));
                            //options.ConfigureWarnings(w => w.Throw(CoreEventId.IncludeIgnoredWarning));
                        });
                    break;
                case "PGSQL":
                    services.AddEntityFrameworkNpgsql()
                        .AddDbContextPool<FandaContext>((serviceProvider, options) =>
                        {
                            options.UseNpgsql(connectionString)
                            .UseInternalServiceProvider(serviceProvider);

                            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                            //options.ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));
                            //options.ConfigureWarnings(w => w.Throw(CoreEventId.IncludeIgnoredWarning));
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