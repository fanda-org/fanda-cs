using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Fanda.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var serviceProvider = services.GetRequiredService<IServiceProvider>();
                    var configuration = services.GetRequiredService<IConfiguration>();
                    Service.Seed.SeedDefault seed = new Service.Seed.SeedDefault(serviceProvider, configuration);

                    seed.CreateRoles().Wait();
                    seed.CreateUsers().Wait();
                    seed.CreateLocations().Wait();
                    seed.CreateDevices().Wait();
                    seed.CreateUnits().Wait();
                    seed.CreatePartyCategories().Wait();
                    seed.CreateProductCategories().Wait();
                    seed.CreateInvoiceCategories().Wait();
                }
                catch (Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An error occurred while creating roles");
                }
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}