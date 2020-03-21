using Fanda.Service.Seed;
using Fanda.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace FandaCoreUI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            await CreateAndRunTasks(webHost);
            webHost.Run();

            //using (var scope = webHost.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        var serviceProvider = services.GetRequiredService<IServiceProvider>();
            //        //var configuration = services.GetRequiredService<IConfiguration>();
            //        var options = services.GetRequiredService<IOptions<AppSettings>>();
            //        Service.Seed.SeedDefault seed = new Service.Seed.SeedDefault(serviceProvider, options);

            //        seed.CreateRoles().Wait();
            //        seed.CreateUsers().Wait();
            //        seed.CreateLocations();
            //        seed.CreateDevices();
            //        seed.CreateUnits();
            //        seed.CreatePartyCategories();
            //        seed.CreateProductCategories();
            //        seed.CreateInvoiceCategories();
            //        seed.CreateDemoOrg().Wait();
            //    }
            //    catch (Exception exception)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(exception, "An error occurred while seeding a database at startup");
            //    }
            //}
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                //webBuilder.UseApplicationInsights();
            });

        private static async Task CreateAndRunTasks(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var serviceProvider = services.GetRequiredService<IServiceProvider>();
                //var configuration = services.GetRequiredService<IConfiguration>();
                var options = services.GetRequiredService<IOptions<AppSettings>>();
                SeedDefault seed = new SeedDefault(serviceProvider, options);

                await seed.CreateOrg("Fanda");
                await seed.CreateOrg("Demo");
            }
            catch (Exception exception)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(exception, "An error occurred while seeding a database at startup");
            }
        }
    }
}