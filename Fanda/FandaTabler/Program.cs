using Fanda.Service.Seed;
using Fanda.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace FandaTabler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            await CreateAndRunTasks(host);
            host.Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                //.AddEnvironmentVariables()
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(configuration);
                    webBuilder.UseStartup<Startup>();
                });
        }

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

                await seed.CreateFanda();
            }
            catch (Exception exception)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(exception, "An error occurred while seeding a database at startup");
            }
        }

        //private static async Task RunTasks(List<Task> tasks)
        //{
        //    while (tasks.Count > 0)
        //    {
        //        var finished = await Task.WhenAny(tasks);
        //        //Debug.WriteLine("RunTasks: {0} - {1}", DateTime.Now.ToString("HH:mm:ss.ffffff"), finished.ToString());
        //        tasks.Remove(finished);
        //    }
        //}
    }
}
