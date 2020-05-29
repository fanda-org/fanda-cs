using AutoMapper;
using Fanda.Repository;
using Fanda.Repository.Extensions;
using Fanda.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fanda
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Health checks
            services.AddHealthChecks();
            #endregion

            #region AppSettings
            services.Configure<AppSettings>(Configuration);
            AppSettings appSettings = Configuration.Get<AppSettings>();
            #endregion

            #region CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                    //.AllowCredentials();
                });

                //var urls = new[]
                //{
                //    Configuration["Order.Web.Url"],
                //    Configuration["Order.Ng.Url"]
                //};
                //options.AddPolicy("_MyAllowedOrigins", builder =>
                //{
                //    builder.WithOrigins(urls)
                //    .AllowAnyHeader()
                //    .AllowAnyMethod();
                //});
            });
            #endregion

            #region DbContext
            services.AddFandaDbContextPool(appSettings);
            #endregion

            #region Response Caching
            services.AddResponseCaching();
            #endregion

            #region Response compression
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/svg+xml" });
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = System.IO.Compression.CompressionLevel.Fastest;
            });
            #endregion Response compression

            #region DistributedMemoryCache, DataProtection and Session
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache(options =>
            {
                //options.ExpirationScanFrequency = TimeSpan.FromMinutes(appSettings.Cache.ExpirationMinute);
                // Default size limit of 200 MB
                //options.SizeLimit = appSettings.Cache.SizeLimitMB * 1024L * 1024L;
            });

            //Distributed Cache
            //services.AddSingleton<IWebCache, WebCache>();

            #region DataProtection
            services.AddDataProtection()
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });
            #endregion

            services.AddSession(options =>
            {
                //options.Cookie.HttpOnly = false;
                //options.Cookie.Name = ".Fanda.Session";
                //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                //options.Cookie.SameSite = SameSiteMode.Lax;
                //options.IdleTimeout = TimeSpan.FromMinutes(20);

                //options.Cookie.IsEssential = true;
                //options.Cookie.Path = "/";
            });
            #endregion

            #region AutoMapper
            services.AddAutoMapper(typeof(Fanda.Repository.AutoMapperProfile.AutoMapperProfile));
            #endregion

            #region Authorization
            services.AddAuthorization();
            #endregion

            #region Commented - JWT Authentication
            var key = Encoding.ASCII.GetBytes(appSettings.FandaSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = (context) =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                        Guid userId = new Guid(context.Principal.Identity.Name);
                        var user = userService.GetByIdAsync(userId).Result;
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            #endregion

            #region Repositorires
            services.AddScoped<IUnitRepository, UnitRepository>();
            #endregion

            #region Angular SPA
            services.AddControllers()
                .AddXmlSerializerFormatters()
                .AddJsonOptions(options =>
                {
                    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();    //new CamelCasePropertyNamesContractResolver();
                    //options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    //options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;    // null = default property (Pascal) casing
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    //options.JsonSerializerOptions.Converters.Add(new JsonStringTrimConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddSessionStateTempDataProvider()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
                //.AddRazorRuntimeCompilation();

            //services.AddControllersWithViews();
            //// In production, the Angular files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";
            //});
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            AutoMapper.IConfigurationProvider autoMapperConfigProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            autoMapperConfigProvider.AssertConfigurationIsValid();

            #region Angular SPA
            //app.UseStaticFiles();
            //if (!env.IsDevelopment())
            //{
            //    app.UseSpaStaticFiles();
            //}
            #endregion

            app.UseStaticFiles();
            app.UseRouting();
            app.UseResponseCaching();
            app.UseResponseCompression();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            #region Angular SPA
            //app.UseSpa(spa =>
            //{
            //    // To learn more about options for serving an Angular SPA from ASP.NET Core,
            //    // see https://go.microsoft.com/fwlink/?linkid=864501

            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseAngularCliServer(npmScript: "start");
            //    }
            //});
            #endregion
        }
    }
}
