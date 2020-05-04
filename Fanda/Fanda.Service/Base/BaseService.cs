using Fanda.Dto;
using Fanda.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Service.Base
{
    public interface IBaseService<TModel>
    {
        Task<TModel> GetByIdAsync(Guid id, bool includeChildren = false);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ChangeStatusAsync(ActiveStatus status);
    }

    public interface IRootService<TModel, TList> : IBaseService<TModel>, IListService<TList>
        where TModel : RootDto
        where TList : RootListDto
    {
        Task<TModel> SaveAsync(TModel model);
        Task<bool> ExistsAsync(BaseDuplicate data);
        Task<bool> ValidateAsync(TModel model);
    }

    public interface IService<TModel, TList> : IBaseService<TModel>, IListService<TList>
    where TModel : BaseDto
    where TList : BaseListDto
    {
        Task<TModel> SaveAsync(TModel model);
        Task<bool> ExistsAsync(BaseDuplicate data);
        Task<bool> ValidateAsync(TModel model);
    }

    public interface IOrgService<TModel, TList> : IBaseService<TModel>, IOrgListService<TList>
        where TModel : BaseDto
        where TList : BaseListDto
    {
        Task<TModel> SaveAsync(Guid orgId, TModel model);
        Task<bool> ExistsAsync(BaseOrgDuplicate data);
        Task<bool> ValidateAsync(Guid orgId, TModel model);
    }

    #region List Services
    public interface IListService<TList>
    {
        IQueryable<TList> GetAll();
    }

    public interface IOrgListService<TList>
    {
        IQueryable<TList> GetAll(Guid orgId);
    }
    #endregion
}
