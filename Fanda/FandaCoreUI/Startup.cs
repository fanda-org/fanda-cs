﻿using AutoMapper;
using Fanda.Repository;
using Fanda.Repository.Extensions;
using Fanda.Shared;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;

namespace FandaCoreUI
{
    public class Startup
    {
        //private readonly IWebHostEnvironment _env;
        public Startup(IConfiguration configuration/*, IWebHostEnvironment env*/)
        {
            Configuration = configuration;
            //_env = env;
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
            });
            #endregion

            #region DbContext
            services.AddFandaDbContextPool(appSettings);
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

            #region Cookie, Session & TempData
            // Adds a default in-memory implementation of IDistributedCache.
            //services.AddDistributedMemoryCache();           
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            #endregion Session/TempData

            #region MVC - ControllersWithViews
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
            //.AddJsonOptions(options =>
            //{
            //    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //    ////new CamelCasePropertyNamesContractResolver();
            //    //options.SerializerSettings.Converters.Add(new StringEnumConverter());
            //    //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            //    ////options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            //    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            //    options.JsonSerializerOptions.IgnoreNullValues = true;
            //    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            //})
            .AddRazorRuntimeCompilation()
            .AddSessionStateTempDataProvider()
            .SetCompatibilityVersion(CompatibilityVersion.Latest);
            #endregion

            #region AutoMapper
            services.AddAutoMapper(typeof(Fanda.Repository.AutoMapperProfile.AutoMapperProfile));
            #endregion

            #region AppSettings
            IConfigurationSection appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            #endregion

            #region JWT Authentication
            //var appSettings = appSettingsSection.Get<AppSettings>();
            //var key = Encoding.ASCII.GetBytes(appSettings.FandaSettings.Secret);
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
            //            var userId = context.Principal.Identity.Name;
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

            services.AddAuthorization();
            #region Repositories
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IPartyCategoryRepository, PartyCategoryRepository>();
            //services.AddScoped<IPartyService, PartyService>();

            services.AddHttpContextAccessor();
            #endregion

            services.AddApplicationInsightsTelemetry();
            services.AddHealthChecks();
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("RequireSuperAdminRole", policy => policy.RequireRole("SuperAdmin"));
            //    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("SuperAdmin", "Admin"));
            //    options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("SuperAdmin", "Admin", "Manager"));
            //    options.AddPolicy("RequireSuperUserRole", policy => policy.RequireRole("SuperAdmin", "Admin", "Manager", "SuperUser"));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            /*IMapper autoMapper*/ AutoMapper.IConfigurationProvider autoMapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler(options =>
                //{
                //    options.Run(async context =>
                //    {
                //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //        context.Response.ContentType = "text/html";
                //        var ex = context.Features.Get<IExceptionHandlerFeature>();
                //        if (ex != null)
                //        {
                //            var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace }";
                //            await context.Response.WriteAsync(err).ConfigureAwait(false);
                //        }
                //    });
                //});
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            autoMapper.AssertConfigurationIsValid();

            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseResponseCompression();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowAll");

            app.UseResponseCompression();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHealthChecks("/health");
            });
            app.UseCookiePolicy();
        }
    }
}