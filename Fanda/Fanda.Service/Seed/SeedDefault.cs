using Fanda.Data.Access;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Fanda.Service.Seed
{
    public class SeedDefault
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public SeedDefault(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task CreateRoles()
        {
            //adding customs roles
            var RoleManager = _serviceProvider.GetRequiredService<RoleManager<Role>>();

            string[] roles = {
                    "SuperAdmin:Super Administrators have complete and unrestricted access to the application",
                    "Admin:Administrators have complete and unrestricted access to the organization",
                    "Manager:Managers are possess limited administrative powers",
                    "SuperUser:Super users are have additional access than users",
                    "Member:Members are prevented from making accidental or intentional system-wide changes and can run most"
                };
            IdentityResult roleResult;

            foreach (var role in roles)
            {
                string roleName = role.Split(':')[0];
                string description = role.Split(':')[1];
                string roleCode = roleName.ToUpper();
                // creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new Role(roleCode, roleName, description));
                }
            }
        }

        public async Task CreateUsers()
        {
            var UserManager = _serviceProvider.GetRequiredService<UserManager<User>>();
            // creating a super user who could maintain the web app
            var superAdmin = new User
            {
                UserName = _configuration["AppSettings:UserName"],
                Email = _configuration["AppSettings:UserEmail"],
                Active = true
            };
            string userPassword = _configuration["AppSettings:UserPassword"];
            var user = await UserManager.FindByNameAsync(superAdmin.UserName);

            if (user == null)
            {
                var createSuperAdmin = await UserManager.CreateAsync(superAdmin, userPassword);
                if (createSuperAdmin.Succeeded)
                {
                    // here we assign the new user the "SuperAdmin" role
                    await UserManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                }
            }
        }

        public Task CreateLocations()
        {
            throw new NotImplementedException();
        }

        public Task CreateDevices()
        {
            throw new NotImplementedException();
        }

        public Task CreateUnits()
        {
            throw new NotImplementedException();
        }

        public Task CreateProductCategories()
        {
            throw new NotImplementedException();
        }

        public Task CreatePartyCategories()
        {
            throw new NotImplementedException();
        }

        public Task CreateInvoiceCategories()
        {
            throw new NotImplementedException();
        }
    }
}