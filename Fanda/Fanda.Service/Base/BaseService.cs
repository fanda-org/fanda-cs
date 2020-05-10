using Fanda.Dto.Base;
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

    public interface IRootService<TModel> : IBaseService<TModel>     //, IListService<TList>
        where TModel : RootDto
    {
        Task<TModel> SaveAsync(TModel model);
        Task<bool> ExistsAsync(Duplicate data);
        Task<DtoErrors> ValidateAsync(TModel model);
    }

    public interface IService<TModel> : IBaseService<TModel>         //, IListService<TList>
        where TModel : BaseDto
    {
        Task<TModel> SaveAsync(TModel model);
        Task<bool> ExistsAsync(Duplicate data);
        Task<DtoErrors> ValidateAsync(TModel model);
    }

    public interface IChildService<TModel> : IBaseService<TModel>    //, IArgListService<TList>
        where TModel : BaseDto
    {
        Task<TModel> SaveAsync(Guid parentId, TModel model);
        Task<bool> ExistsAsync(ChildDuplicate data);
        Task<DtoErrors> ValidateAsync(Guid parentId, TModel model);
    }

    #region Get children data
    public interface IChildDataService<TModel>
    {
        Task<TModel> GetChildrenByIdAsync(Guid parentId);
    }
    #endregion

    #region List Services
    public interface IListService<TList>
    {
        IQueryable<TList> GetAll();
    }
    public interface IChildListService<TList>
    {
        IQueryable<TList> GetAll(Guid parentId);
    }
    #endregion
}
