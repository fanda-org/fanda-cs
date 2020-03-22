using Fanda.Dto;
using Fanda.Shared;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Fanda.Service.Seed
{
    public class SeedDefault
    {
        private readonly IServiceProvider _provider;
        private readonly AppSettings _settings;
        private readonly ILogger<SeedDefault> _logger;

        public SeedDefault(IServiceProvider provider, IOptions<AppSettings> options)
        {
            _provider = provider;
            _settings = options.Value;
            _logger = _provider.GetRequiredService<ILogger<SeedDefault>>();
        }

        #region Fanda Org
        public async Task CreateOrgAsync(string orgName)
        {
            try
            {
                var service = _provider.GetRequiredService<IOrganizationService>();

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

                    await CreateRolesAsync(org);
                    //var loc = await CreateLocations(org);
                    await CreateUsersAsync(org);
                    await CreateUnitsAsync(org);
                    await CreatePartyCategoriesAsync(org);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

        }
        #endregion

        private async Task CreateRolesAsync(OrganizationDto org)
        {
            try
            {
                var service = _provider.GetRequiredService<IRoleService>();

                //adding customs roles
                string[] rolesArray = {
                        "SuperAdmin:Super Administrators have complete and unrestricted access to the application",
                        "Admin:Administrators have complete and unrestricted access to the organization",
                        //"Manager:Managers are possess limited administrative powers",
                        "SuperUser:Super users are have additional access than users",
                        "User:Users are prevented from making accidental or intentional system-wide changes and can run most"
                    };


                foreach (var roleElement in rolesArray)
                {
                    string roleName = roleElement.Split(':')[0];
                    string description = roleElement.Split(':')[1];
                    string roleCode = roleName.ToUpper();
                    // creating the roles and seeding them to the database
                    if (!service.Exists(org.Id, roleCode))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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

        private async Task CreateUsersAsync(OrganizationDto org)
        {
            try
            {
                var service = _provider.GetRequiredService<IUserService>();

                // creating a super user who could maintain the web app
                var superAdmin = new UserDto
                {
                    UserName = _settings.FandaSettings.UserName,
                    Email = _settings.FandaSettings.UserEmail,
                    //LocationId = loc.LocationId,
                    Active = true
                };
                var user = await service.GetByNameAsync(superAdmin.UserName);
                if (user == null)
                {
                    user = await service.SaveAsync(superAdmin, _settings.FandaSettings.UserPassword);
                }
                await service.AddToOrgAsync(org.Id, user.Id);
                await service.AddToRoleAsync(org.Id, user.Id, "SuperAdmin");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        //public async Task CreateDevices()
        //{
        //    //throw new NotImplementedException();
        //}

        private async Task CreateUnitsAsync(OrganizationDto org)
        {
            try
            {
                var service = _provider.GetRequiredService<IUnitService>();
                if (!service.Exists(org.Id, "DEFAULT"))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task CreatePartyCategoriesAsync(OrganizationDto org)
        {
            try
            {
                var service = _provider.GetRequiredService<IPartyCategoryService>();
                if (!service.Exists(org.Id, "DEFAULT"))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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