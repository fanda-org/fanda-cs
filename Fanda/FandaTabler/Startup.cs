using AutoMapper;
using Fanda.Service;
using Fanda.Service.Extensions;
using Fanda.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Text.Json.Serialization;
namespace FandaTabler
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
            var appSettings = Configuration.Get<AppSettings>();
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
            });
            #endregion

            #region DbContext
            switch (appSettings.DatabaseType)
            {
                case "MSSQL":
                    services.InitializeContext(appSettings.DatabaseType, appSettings.ConnectionStrings.MsSqlConnection);
                    break;
                case "MYSQL":
                    services.InitializeContext(appSettings.DatabaseType, appSettings.ConnectionStrings.MySqlConnection);
                    break;
                case "PGSQL":
                    services.InitializeContext(appSettings.DatabaseType, appSettings.ConnectionStrings.PgSqlConnection);
                    break;
                default:
                    services.InitializeContext("MSSQL", appSettings.ConnectionStrings.DefaultConnection);
                    break;
            }
            #endregion

            #region Commented DbContext
            //services.AddEntityFrameworkMySql();
            //services.AddDbContextPool<FandaContext>(options =>
            //{
            //    switch (appSettings.DatabaseType)
            //    {
            //        case "MSSQL":
            //            options.UseSqlServer(appSettings.ConnectionStrings.MsSqlConnection, sqlopt =>
            //            {
            //                sqlopt.EnableRetryOnFailure();
            //                //sqlopt.UseRowNumberForPaging();
            //            });
            //            break;

            //        case "MYSQL":
            //            options.UseMySql(appSettings.ConnectionStrings.MySqlConnection, mysqlOptions =>
            //            {
            //                mysqlOptions.ServerVersion(new Version(15, 1), ServerType.MariaDb);
            //            });
            //            break;

            //        case "PGSQL":
            //            options.UseNpgsql(appSettings.ConnectionStrings.PgSqlConnection, pgsqlOptions =>
            //            {
            //            });
            //            break;

            //        default:
            //            throw new Exception("Unknown database type from appsettings");
            //    }
            //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            //options
            //.ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));
            //.ConfigureWarnings(w => w.Throw(CoreEventId.IncludeIgnoredWarning));
            //})
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

            #region DistributedMemoryCache and Session
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache(options =>
            {
                //options.ExpirationScanFrequency = TimeSpan.FromMinutes(appSettings.Cache.ExpirationMinute);
                // Default size limit of 200 MB
                //options.SizeLimit = appSettings.Cache.SizeLimitMB * 1024L * 1024L;
            });

            //Distributed Cache
            //services.AddSingleton<IWebCache, WebCache>();

            services.AddDataProtection()
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration()
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                });

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".Fanda.Session";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;
                //options.Cookie.SameSite = SameSiteMode.None;
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                //options.Cookie.IsEssential = true;
                //options.Cookie.Path = "/";
            });
            #endregion

            #region MVC - AddControllersWithViews
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());

                options.CacheProfiles.Add("Default",
                    new CacheProfile()
                    {
                        Duration = 240
                    });
                options.CacheProfiles.Add("NoCache",
                    new CacheProfile()
                    {
                        Duration = 0,
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
            })
            .AddJsonOptions(options =>
            {
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();    //new CamelCasePropertyNamesContractResolver();
                //options.SerializerSettings.Converters.Add(new StringEnumConverter());
                //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                //options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.IgnoreNullValues = true;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddSessionStateTempDataProvider()
            .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization()
            .AddRazorRuntimeCompilation();
            #endregion

            #region Cookie Policy Options
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });
            #endregion

            #region AutoMapper
            services.AddAutoMapper(typeof(Fanda.Service.AutoMapperProfile.AutoMapperProfile));
            #endregion

            #region Cookie Authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "auth_cookie";
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

                options.Cookie.SameSite = SameSiteMode.None;
                options.LoginPath = new PathString("/Users/Login");
                //options.ReturnUrlParameter = "/Users/Login";
                options.AccessDeniedPath = "/Errors/401";
                options.LogoutPath = "/Users/Logout";
                //options.Events.OnRedirectToLogin

                //options.LoginPath = "/Users/Login";
                //options.LogoutPath = "/Users/Logout";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });
            #endregion

            #region Authorization
            services.AddAuthorization();
            #endregion

            #region Commented - JWT Authentication
            //var appSettings = appSettingsSection.Get<AppSettings>();
            //var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.Events = new JwtBearerEvents
            //    {
            //        OnTokenValidated = (context) =>
            //        {
            //            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            //            Guid userId = new Guid(context.Principal.Identity.Name);
            //            var user = userService.GetByIdAsync(userId).Result;
            //            if (user == null)
            //            {
            //                // return unauthorized if user no longer exists
            //                context.Fail("Unauthorized");
            //            }
            //            return Task.CompletedTask;
            //        }
            //    };
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});
            #endregion

            #region Services
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IPartyCategoryService, PartyCategoryService>();
            services.AddScoped<IPartyService, PartyService>();

            services.AddHttpContextAccessor();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            AutoMapper.IConfigurationProvider autoMapper)
        {
            #region Commented - StatusCodePages
            //app.UseStatusCodePages(async context =>
            //{
            //    var request = context.HttpContext.Request;
            //    var response = context.HttpContext.Response;

            //    // you may also check requests path to do this only for specific methods       
            //    // && request.Path.Value.StartsWith("/specificPath")
            //    if ((response.StatusCode == (int)HttpStatusCode.Unauthorized) &&
            //        request.Path == "/" || request.Path.Equals("/home", StringComparison.OrdinalIgnoreCase) ||
            //            request.Path.Equals("/home/index", StringComparison.OrdinalIgnoreCase))
            //    {
            //        response.Redirect("/users/login");
            //    }
            //    else
            //    {
            //        response.Redirect($"/errors/index/{response.StatusCode}");
            //    }
            //});
            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                #region Commented - Generic Exception Handler
                //app.UseExceptionHandler(options =>
                //{
                //    options.Run(async context2 =>
                //    {
                //        context2.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //        context2.Response.ContentType = "text/html";
                //        var ex = context2.Features.Get<IExceptionHandlerFeature>();
                //        if (ex != null)
                //        {
                //            var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace}";
                //            await context2.Response.WriteAsync(err).ConfigureAwait(false);
                //        }
                //    });
                //});
                #endregion

                app.UseHsts();
            }

            #region Commented - URL Rewrite
            //app.UseRewriter(new RewriteOptions()
            //    .AddRedirectToHttps()
            //    .AddRedirect(@"^section1/(.*)", "new/$1", (int)HttpStatusCode.Redirect)
            //    .AddRedirect(@"^section2/(\\d+)/(.*)", "new/$1/$2", (int)HttpStatusCode.MovedPermanently)
            //    .AddRewrite("^feed$", "/?format=rss", skipRemainingRules: false));
            #endregion

            autoMapper.AssertConfigurationIsValid();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseSession();
            app.UseResponseCaching();
            app.UseResponseCompression();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");

            #region Commented - Previous Routes
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Users}/{action=Logout}/{id?}");
            //});
            #endregion
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Users}/{action=Logout}/{id?}");
                endpoints.MapHealthChecks("/health");
            });

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.SameAsRequest,
                MinimumSameSitePolicy = SameSiteMode.Lax,
                CheckConsentNeeded = context => false,
            });
        }
    }
}
