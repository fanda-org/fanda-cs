namespace Fanda.Infrastructure.Auth
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Fanda.Core.Auth;
    using Fanda.Core.Base;
    using Fanda.Entities.Auth;
    using Fanda.Infrastructure.Base;
    using Fanda.Infrastructure.Extensions;
    using Fanda.Shared;
    using Microsoft.EntityFrameworkCore;

    public interface IResourceRepository :
        IParentRepository<ResourceDto>,
        IListRepository<ResourceListDto>
    {
        Task<bool> MapAction(ResourceActionDto model);
        Task<bool> UnmapAction(ResourceActionDto model);
    }

    public class ResourceRepository : IResourceRepository
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public ResourceRepository(AuthContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;

        }
        public async Task<bool> ChangeStatusAsync(ActiveStatus status)
        {
            if (status.Id == null || status.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }

            var resource = await context.Resources
                .FindAsync(status.Id);
            if (resource != null)
            {
                resource.Active = status.Active;
                context.Resources.Update(resource);
                await context.SaveChangesAsync();
                return true;
            }
            throw new NotFoundException("Resource not found");
        }

        public async Task<ResourceDto> CreateAsync(ResourceDto model)
        {
            var resource = mapper.Map<Resource>(model);
            resource.DateCreated = DateTime.UtcNow;
            resource.DateModified = null;
            await context.Resources.AddAsync(resource);
            await context.SaveChangesAsync();
            return mapper.Map<ResourceDto>(resource);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }
            var resource = await context.Resources
                .FindAsync(id);
            if (resource == null)
            {
                throw new NotFoundException("Resource not found");
            }

            context.Resources.Remove(resource);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(ParentDuplicate data)
            => await context.ExistsAsync<Resource>(data);

        public IQueryable<ResourceListDto> GetAll(Guid parentId)  // nullable
        {
            // if (tenantId == null || tenantId == Guid.Empty)
            // {
            //     throw new ArgumentNullException("tenantId", "Tenant id is missing");
            // }
            IQueryable<ResourceListDto> qry = context.Resources
                .AsNoTracking()
                //.Include(u => u.OrgUsers)
                //.ThenInclude(ou => ou.Organization)
                //.SelectMany(u => u.OrgUsers.Select(ou => ou.Organization))
                //.Where(u => u.OrgUsers.Any(ou => ou.OrgId == orgId))
                //.Where(u => u.TenantId == tenantId)
                .ProjectTo<ResourceListDto>(mapper.ConfigurationProvider);
            //.Where(u => u.OrgId == orgId);
            //if (orgId != null && orgId != Guid.Empty)
            //{
            //    userQry = userQry.Where(u => u.OrgId == orgId);
            //}
            return qry;
        }

        public async Task<ResourceDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is missing");
            }
            var resource = await context.Resources
                .AsNoTracking()
                .ProjectTo<ResourceDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (resource != null)
            {
                return resource;
            }
            throw new NotFoundException("Resource not found");
        }

        public async Task UpdateAsync(Guid id, ResourceDto model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("Resource id mismatch");
            }
            var resource = mapper.Map<Resource>(model);
            resource.DateModified = DateTime.UtcNow;
            context.Resources.Update(resource);
            await context.SaveChangesAsync();
        }

        public async Task<ValidationResultModel> ValidateAsync(ResourceDto model)
        {
            // Reset validation errors
            model.Errors.Clear();

            #region Formatting: Cleansing and formatting
            model.Code = model.Code.TrimExtraSpaces().ToUpper();
            model.Name = model.Name.TrimExtraSpaces();
            #endregion

            #region Validation: Duplicate
            // Check email duplicate
            var duplCode = new Duplicate { Field = DuplicateField.Code, Value = model.Code, Id = model.Id };
            if (await ExistsAsync(duplCode))
            {
                model.Errors.AddError(nameof(model.Code), $"{nameof(model.Code)} '{model.Code}' already exists");
            }
            // Check name duplicate
            var duplName = new Duplicate { Field = DuplicateField.Name, Value = model.Name, Id = model.Id };
            if (await ExistsAsync(duplName))
            {
                model.Errors.AddError(nameof(model.Name), $"{nameof(model.Name)} '{model.Name}' already exists");
            }
            #endregion

            return model.Errors;
        }

        public async Task<bool> MapAction(ResourceActionDto model)
        {
            var resourceAction = mapper.Map<ResourceAction>(model);
            await context.Set<ResourceAction>().AddAsync(resourceAction);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnmapAction(ResourceActionDto model)
        {
            var resourceAction = await context.Set<ResourceAction>()
                .FirstOrDefaultAsync(ra => ra.ResourceId == model.ResourceId &&
                    ra.ActionId == model.ActionId);
            if (resourceAction == null)
            {
                throw new NotFoundException("Resource-action not found");
            }
            context.Set<ResourceAction>().Remove(resourceAction);
            await context.SaveChangesAsync();
            return true;
        }
    }
}