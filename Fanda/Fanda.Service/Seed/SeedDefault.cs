using Fanda.Dto;
using Fanda.Shared;
//using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Fanda.Service.Seed
{
    public class SeedDefault
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AppSettings _settings;

        public SeedDefault(IServiceProvider serviceProvider, IOptions<AppSettings> options)
        {
            _serviceProvider = serviceProvider;
            _settings = options.Value;
        }

        #region Fanda Org
        public async Task CreateOrg(string orgName)
        {
            var service = _serviceProvider.GetRequiredService<IOrganizationService>();
            string orgCode = orgName.ToUpper();
            if (!service.ExistsByCode(orgCode))
            {
                var org = await service.SaveAsync(new OrganizationDto
                {
                    OrgCode = orgCode,
                    OrgName = orgName,
                    Description = $"{orgName} organization",
                    Active = true
                });

                await CreateRoles(org);
                //var loc = await CreateLocations(org);
                await CreateUsers(org);
                await CreateUnits(org);
                await CreatePartyCategories(org);
            }
        }
        #endregion

        private async Task CreateRoles(OrganizationDto org)
        {
            //adding customs roles
            var service = _serviceProvider.GetRequiredService<IRoleService>();

            string[] rolesArray = {
                    "SuperAdmin:Super Administrators have complete and unrestricted access to the application",
                    "Admin:Administrators have complete and unrestricted access to the organization",
                    //"Manager:Managers are possess limited administrative powers",
                    "SuperUser:Super users are have additional access than users",
                    "User:Users are prevented from making accidental or intentional system-wide changes and can run most"
                };
            //IdentityResult roleResult;

            foreach (var roleElement in rolesArray)
            {
                string roleName = roleElement.Split(':')[0];
                string description = roleElement.Split(':')[1];
                string roleCode = roleName.ToUpper();
                // creating the roles and seeding them to the database
                var roleExist = service.ExistsAsync(roleCode);
                if (!roleExist)
                {
                    var model = new RoleDto
                    {
                        Code = roleCode,
                        Name = roleName,
                        Description = description,
                        Active = true
                    };
                    await service.SaveAsync(org.Id, model);
                }
            }
        }

        //private async Task<LocationDto> CreateLocations(OrganizationDto org)
        //{
        //    var service = _serviceProvider.GetRequiredService<ILocationService>();
        //    var loc = await service.ExistsAsync("DEFAULT");
        //    if (loc == null)
        //    {
        //        loc = new LocationDto
        //        {
        //            Code = "DEFAULT",
        //            Name = "Default",
        //            Description = "Default Location",
        //            Active = true
        //        };
        //        loc = await service.SaveAsync(org.OrgId, loc);
        //    }
        //    return loc;
        //}

        private async Task CreateUsers(OrganizationDto org)
        {
            var service = _serviceProvider.GetRequiredService<IUserService>();
            // creating a super user who could maintain the web app
            var superAdmin = new UserDto
            {
                UserName = _settings.FandaSettings.UserName,
                Email = _settings.FandaSettings.UserEmail,
                //LocationId = loc.LocationId,
                Active = true
            };
            if (!await service.ExistsAsync(superAdmin.UserName))
            {
                var user = await service.SaveAsync(org.Id, superAdmin, _settings.FandaSettings.UserPassword);
                await service.AddToRoleAsync(org.Id, user, "SuperAdmin");
            }
        }

        //public async Task CreateDevices()
        //{
        //    //throw new NotImplementedException();
        //}

        private async Task CreateUnits(OrganizationDto org)
        {
            var service = _serviceProvider.GetRequiredService<IUnitService>();
            if (!await service.ExistsAsync("DEFAULT"))
            {
                var unit = new UnitDto
                {
                    Code = "DEFAULT",
                    Name = "Default",
                    Active = true,
                };
                await service.SaveAsync(org.Id, unit);
            }
        }

        private async Task CreatePartyCategories(OrganizationDto org)
        {
            var service = _serviceProvider.GetRequiredService<IPartyCategoryService>();
            if (!service.ExistsAsync("DEFAULT").Result)
            {
                await service.SaveAsync(org.Id, new PartyCategoryDto
                {
                    Code = "DEFAULT",
                    Name = "Default",
                    Description = "Default Category",
                    Active = true
                });
            }
        }

        //public async Task CreateProductCategories()
        //{
        //    //throw new NotImplementedException();
        //}

        //public async Task CreateInvoiceCategories()
        //{
        //    //throw new NotImplementedException();
        //}
    }
}