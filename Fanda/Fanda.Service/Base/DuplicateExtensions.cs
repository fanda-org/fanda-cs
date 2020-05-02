using Fanda.Data;
using Fanda.Data.Context;
using Fanda.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Fanda.Service.Base
{
    public static class DuplicateExtensions
    {
        public static async Task<bool> ExistsAsync<TModel>(this FandaContext context, RootDuplicate data)
            where TModel : RootModel
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

        public static async Task<bool> ExistsAsync<TModel>(this FandaContext context, BaseDuplicate data)
            where TModel : BaseModel
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

        public static async Task<bool> ExistsAsync<TModel>(this FandaContext context, BaseOrgDuplicate data)
            where TModel : BaseOrgModel
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
