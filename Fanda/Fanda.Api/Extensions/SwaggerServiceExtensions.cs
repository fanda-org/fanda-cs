using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;

namespace Fanda.Api.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Fanda API",
                    Description = "RESTful API of Financial accounting and inventory mangement system",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "Balamurugan Thanikachalam",
                        Email = "software.balu@gmail.com",
                        Url = "https://twitter.com/tbalakpm"
                    },
                    License = new License
                    {
                        Name = "Use under Propreitary",
                        Url = string.Empty
                    }
                });

                //c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                //{
                //    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                //    Name = "Authorization",
                //    In = "header",
                //    Type = "apiKey"
                //});

                //// Swagger 2.+ support
                //var security = new Dictionary<string, IEnumerable<string>>
                //{
                //    { "Bearer", Enumerable.Empty<string>() },
                //};
                //c.AddSecurityRequirement(security);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            //app.UseSwagger(typeof(Startup).Assembly, settings =>
            //{
            //    settings.PostProcess = document =>
            //    {
            //        document.Info.Version = "v1";
            //        document.Info.Title = "Fanda API";
            //        document.Info.Description = "RESTful API of Financial accounting and inventory mangement system";
            //        document.Info.TermsOfService = "None";
            //        document.Info.Contact = new NSwag.SwaggerContact
            //        {
            //            Name = "Balamurugan Thanikachalam",
            //            Email = "software.balu@gmail.com",
            //            Url = "https://twitter.com/tbalakpm"
            //        };
            //        document.Info.License = new NSwag.SwaggerLicense
            //        {
            //            Name = "Use under Propreitary",
            //            Url = string.Empty
            //        };
            //    };
            //});

            //Enable middleware to serve swagger - ui(HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Fanda API v1.0");
                c.DocExpansion(DocExpansion.None);
            });

            // Enable the Swagger UI middleware and the Swagger generator
            //app.UseSwaggerUi(typeof(Startup).GetTypeInfo().Assembly, settings =>
            //{
            //    //settings.DocExpansion = "None";
            //    settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
            //});
            return app;
        }
    }
}