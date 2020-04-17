using Fanda.Data;
using Fanda.Data.Context;
using Fanda.Dto;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service
{
    public interface IBaseService<TModel> where TModel : BaseDto
    {
        IQueryable<TModel> GetAll();
        Task<TModel> GetByIdAsync(Guid id, bool include = false);
        Task<TModel> SaveAsync(TModel model);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ChangeStatusAsync(ActiveStatus status);
        Task<bool> ExistsAsync(BaseDuplicate data);
        string ErrorMessage { get; }
    }

    public interface IBaseOrgService<TModel> /*: IBaseService<TModel>*/ where TModel : BaseDto
    {
        IQueryable<TModel> GetAll(Guid orgId);
        Task<TModel> GetByIdAsync(Guid id, bool include = false);
        Task<TModel> SaveAsync(Guid orgId, TModel model);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ChangeStatusAsync(ActiveStatus status);
        Task<bool> ExistsAsync(BaseOrgDuplicate data);
        string ErrorMessage { get; }
    }


    public static class DuplicateExtensions
    {
        public static async Task<bool> ExistsAsync<TModel>(this FandaContext context, BaseDuplicate data) where TModel : BaseModel
        {
            bool result = true;
            switch (data.Field)
            {
                case DuplicateField.Id:
                    if (data.Id != Guid.Empty)
                    {
                        return await context.Set<TModel>()
                            .AnyAsync(pc => pc.Id == data.Id);
                    }
                    return result;
                case DuplicateField.Code:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value && pc.Id != data.Id);
                    }
                    return result;
                case DuplicateField.Name:
                    if (data.Id == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value);
                    }
                    else if (data.Id != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value && pc.Id != data.Id);
                    }
                    return result;
                default:
                    return true;
            }
        }

        public static async Task<bool> ExistsAsync<TModel>(this FandaContext context, BaseOrgDuplicate data) where TModel : BaseOrgModel
        {
            bool result = true;
            switch (data.Field)
            {
                case DuplicateField.Id:
                    if (data.Id != Guid.Empty)
                    {
                        return await context.Set<TModel>() //PartyCategories
                            .AnyAsync(pc => pc.Id == data.Id);
                    }
                    return result;
                case DuplicateField.Code:
                    if (data.Id == Guid.Empty && data.OrgId == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value);
                    }
                    else if (data.Id == Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value && pc.OrgId == data.OrgId);
                    }
                    else if (data.Id != Guid.Empty && data.OrgId == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value && pc.Id != data.Id);
                    }
                    else if (data.Id != Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Code == data.Value && pc.Id != data.Id && pc.OrgId == data.OrgId);
                    }
                    return result;
                case DuplicateField.Name:
                    if (data.Id == Guid.Empty && data.OrgId == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value);
                    }
                    else if (data.Id == Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value && pc.OrgId == data.OrgId);
                    }
                    else if (data.Id != Guid.Empty && data.OrgId == Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value && pc.Id != data.Id);
                    }
                    else if (data.Id != Guid.Empty && data.OrgId != Guid.Empty)
                    {
                        result = await context.Set<TModel>()
                            .AnyAsync(pc => pc.Name == data.Value && pc.Id != data.Id && pc.OrgId == data.OrgId);
                    }
                    return result;
                default:
                    return true;
            }
        }
    }
}
