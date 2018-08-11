using Fanda.Api.Extensions;
using Fanda.Service.Access;
using Fanda.Service.AutoMapperProfile;
using Fanda.Service.Business;
using Fanda.Service.Commodity;
using Fanda.Service.Data;
using Fanda.Service.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Sieve.Services;

namespace Fanda.Api
{
    // Ref: https://github.com/openiddict/openiddict-core/issues/577
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
            //services.AddDbContext<FandaContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            //});

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

            // Sieve - Paging, Sorting, Filtering
            services.AddScoped<SieveProcessor>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<IPartyService, PartyService>();
            services.AddScoped<IPartyCategoryService, PartyCategoryService>();
            services.AddScoped<IProductCategoryService, ProductCategoryService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IProductBrandService, ProductBrandService>();
            services.AddScoped<IProductSegmentService, ProductSegmentService>();
            services.AddScoped<IProductVarietyService, ProductVarietyService>();
            services.AddScoped<IProductService, ProductService>();

            // AutoMapper
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            // Configuration
            services.AddSingleton(Configuration);

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters()
                .AddApiExplorer()
                .AddJsonOptions(options =>
                {
                    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    //new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    //options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //// Add custom properties as claims and return the claims as class properties.
            //services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User>>();
            //// Add custom SignInManager to check for expired passwords and limit IP addresses.
            //services.AddScoped<SignInManager<User>, SignInManager<User>>();
            //// Add custom UserManager to override PasswordChange() & PasswordReset().
            //services.AddScoped<UserManager<User>, UserManager<User>>();
            //// Add custom UserStore to store user previous passwords.
            ////services.AddScoped<IUserStore<User>, UserStore<User>>();
            //services.AddScoped<RoleManager<Role>, RoleManager<Role>>();

            services.AddCors();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireSuperAdminRole", policy => policy.RequireRole("SuperAdmin"));
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("SuperAdmin", "Admin"));
                options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("SuperAdmin", "Admin", "Manager"));
                options.AddPolicy("RequireSuperUserRole", policy => policy.RequireRole("SuperAdmin", "Admin", "Manager", "SuperUser"));
            });
            /*
            [Authorize(Policy = "RequireAdminRole")]
            public class AdminController : Controller
            {
                public IActionResult Index()
                {
                    return View();
                }
            }
            */

            //services.AddIdentityServer()
            //    .AddDeveloperSigningCredential()
            //    .AddInMemoryPersistedGrants()
            //    .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
            //    .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
            //    .AddInMemoryClients(IdentityServerConfig.GetClients())
            //    .AddAspNetIdentity<User>();
            services.InitializeContext(Configuration.GetConnectionString("DefaultConnection"));

            //services.AddAuthentication("Bearer")
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = Configuration["AppSettings:ApiUrl"]; //"http://localhost:57071/";
            //        options.RequireHttpsMetadata = false;

            //        options.ApiName = IdentityServerConfig.FandaApiName;
            //    });

            services.AddSwaggerDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            //else
            //app.UseHsts();

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseIdentityServer();

            app.UseCors(c =>
                c.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials()
            );
            app.UseSwaggerDocumentation();
            //app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}