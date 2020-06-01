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
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fanda
{
#pragma warning restore CS1591
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

            #region DbContext
            services.AddFandaDbContextPool(appSettings);
            #endregion

            #region CORS
            services.AddCors();
            //options =>
            //{
            //    options.AddPolicy("AllowAll", builder =>
            //    {
            //        builder
            //        .AllowAnyOrigin()
            //        .AllowAnyMethod()
            //        .AllowAnyHeader();
            //        //.AllowCredentials();
            //    });

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
            //});
            #endregion

            #region Commented - Response Caching
            //services.AddResponseCaching();
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

            #region Commented - DistributedMemoryCache, DataProtection and Session
            // Adds a default in-memory implementation of IDistributedCache.
            //services.AddDistributedMemoryCache(options =>
            //{
            //    //options.ExpirationScanFrequency = TimeSpan.FromMinutes(appSettings.Cache.ExpirationMinute);
            //    // Default size limit of 200 MB
            //    //options.SizeLimit = appSettings.Cache.SizeLimitMB * 1024L * 1024L;
            //});

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

            //services.AddSession(options =>
            //{
            //    //options.Cookie.HttpOnly = false;
            //    //options.Cookie.Name = ".Fanda.Session";
            //    //options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //    //options.Cookie.SameSite = SameSiteMode.Lax;
            //    //options.IdleTimeout = TimeSpan.FromMinutes(20);

            //    //options.Cookie.IsEssential = true;
            //    //options.Cookie.Path = "/";
            //});
            #endregion

            #region AutoMapper
            services.AddAutoMapper(typeof(Fanda.Repository.AutoMapperProfile.AutoMapperProfile));
            #endregion

            #region Authorization
            services.AddAuthorization();
            #endregion

            #region JWT Authentication
            var key = Encoding.ASCII.GetBytes(appSettings.FandaSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                //x.Events = new JwtBearerEvents
                //{
                //    OnTokenValidated = (context) =>
                //    {
                //        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                //        Guid userId = new Guid(context.Principal.Identity.Name);
                //        var user = userService.GetByIdAsync(userId).Result;
                //        if (user == null)
                //        {
                //            // return unauthorized if user no longer exists
                //            context.Fail("Unauthorized");
                //        }
                //        return Task.CompletedTask;
                //    }
                //};
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            #region Repositorires
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IUserRepository, UserRepository>();
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
                //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                //.AddSessionStateTempDataProvider()
                //.AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            //.AddRazorRuntimeCompilation();

            //services.AddControllersWithViews();
            //// In production, the Angular files will be served from this directory
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/dist";
            //});
            #endregion

            #region Swagger
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Fanda API",
                    Version = "v1",
                    Description = "Communicate to Fanda backend from third party background services",
                    TermsOfService = new Uri("https://fanda.in/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Balamurugan Thanikachalam",
                        Email = "software.balu@gmail.com",
                        Url = new Uri("https://twitter.com/tbalakpm"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://fanda.in/license"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });
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
            //app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fanda API V1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();
            autoMapperConfigProvider.AssertConfigurationIsValid();

            #region Angular SPA
            //app.UseStaticFiles();
            //if (!env.IsDevelopment())
            //{
            //    app.UseSpaStaticFiles();
            //}
            #endregion

            app.UseRouting();
            app.UseResponseCaching();
            app.UseResponseCompression();
            //app.UseCors("AllowAll");
            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
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
#pragma warning restore CS1591
}
