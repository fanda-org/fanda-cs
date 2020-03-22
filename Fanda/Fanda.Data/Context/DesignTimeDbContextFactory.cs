using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Fanda.Data.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FandaContext>
    {
        public FandaContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<FandaContext>();

            string dbType = configuration["DatabaseType"];
            string connectionString;
            switch (dbType)
            {
                case "MSSQL":
                    connectionString = configuration.GetConnectionString("MsSqlConnection");
                    builder.UseSqlServer(connectionString);
                    break;
                case "MYSQL":
                    connectionString = configuration.GetConnectionString("MySqlConnection");
                    builder.UseMySql(connectionString);
                    break;
                case "PGSQL":
                    connectionString = configuration.GetConnectionString("PgSqlConnection");
                    builder.UseNpgsql(connectionString);
                    break;
                default:
                    throw new System.Exception("Unknown database type from appsettings");
            }

            return new FandaContext(builder.Options);
        }
    }
}