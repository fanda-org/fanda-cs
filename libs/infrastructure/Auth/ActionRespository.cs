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
    using Action = Entities.Auth.Action;

    public interface IActionRepository :
        IParentRepository<ActionDto>,
        IListRepository<ActionListDto>
    {
    }

    public class ActionRepository : IActionRepository
    {
        private readonly AuthContext context;
        private readonly IMapper mapper;

        public ActionRepository(AuthContext context, IMapper mapper)
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

            var action = await context.Actions
                .FindAsync(status.Id);
            if (action != null)
            {
                action.Active = status.Active;
                context.Actions.Update(action);
                await context.SaveChangesAsync();
                return true;
            }
            throw new NotFoundException("Action not found");
        }

        public async Task<ActionDto> CreateAsync(ActionDto model)
        {
            var action = mapper.Map<Action>(model);
            action.DateCreated = DateTime.UtcNow;
            action.DateModified = null;
            await context.Actions.AddAsync(action);
            await context.SaveChangesAsync();
            return mapper.Map<ActionDto>(action);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("Id", "Id is missing");
            }
            var action = await context.Actions
                .FindAsync(id);
            if (action == null)
            {
                throw new NotFoundException("Action not found");
            }
            context.Actions.Remove(action);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(ParentDuplicate data)
            => await context.ExistsAsync<Action>(data);

        public IQueryable<ActionListDto> GetAll(Guid parentId)  // nullable
        {
            // if (tenantId == null || tenantId == Guid.Empty)
            // {
            //     throw new ArgumentNullException("tenantId", "Tenant id is missing");
            // }
            IQueryable<ActionListDto> qry = context.Actions
                .AsNoTracking()
                //.Include(u => u.OrgUsers)
                //.ThenInclude(ou => ou.Organization)
                //.SelectMany(u => u.OrgUsers.Select(ou => ou.Organization))
                //.Where(u => u.OrgUsers.Any(ou => ou.OrgId == orgId))
                //.Where(u => u.TenantId == tenantId)
                .ProjectTo<ActionListDto>(mapper.ConfigurationProvider);
            //.Where(u => u.OrgId == orgId);
            //if (orgId != null && orgId != Guid.Empty)
            //{
            //    userQry = userQry.Where(u => u.OrgId == orgId);
            //}
            return qry;
        }

        public async Task<ActionDto> GetByIdAsync(Guid id, bool includeChildren = false)
        {
            if (id == null || id == Guid.Empty)
            {
                throw new ArgumentNullException("id", "Id is missing");
            }
            var action = await context.Actions
                .AsNoTracking()
                .ProjectTo<ActionDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (action != null)
            {
                return action;
            }
            throw new NotFoundException("Action not found");
        }

        public async Task UpdateAsync(Guid id, ActionDto model)
        {
            if (id != model.Id)
            {
                throw new ArgumentException("Action id mismatch");
            }
            var action = mapper.Map<Action>(model);
            action.DateModified = DateTime.UtcNow;
            context.Actions.Update(action);
            await context.SaveChangesAsync();
        }

        public async Task<ValidationResultModel> ValidateAsync(ActionDto model)
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
    }
}