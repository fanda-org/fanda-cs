using Fanda.Data.Context;
//using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace Fanda.Service.Data
{
    public static class InitializeContextExtensions
    {
        public static void InitializeContext(this IServiceCollection services, string databaseType, string connectionString)
        {
            services.AddDbContext<FandaContext>(options =>
            {
                switch (databaseType)
                {
                    case "MSSQL":
                        options.UseSqlServer(connectionString, sqlopt =>
                        {
                            sqlopt.EnableRetryOnFailure();
                            //sqlopt.UseRowNumberForPaging();
                        });
                        break;

                    case "MYSQL":
                        options.UseMySql(connectionString, mysqlOptions =>
                        {
                            mysqlOptions.ServerVersion(new Version(15, 1), ServerType.MariaDb);
                        });
                        break;

                    case "PGSQL":
                        options.UseNpgsql(connectionString);
                        break;

                    default:
                        throw new Exception("Unknown database type from appsettings");
                }
                //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                //options
                //.ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));
                //.ConfigureWarnings(w => w.Throw(CoreEventId.IncludeIgnoredWarning));
            });

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