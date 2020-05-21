﻿using Fanda.Dto.Base;
using Fanda.Repository.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fanda.Repository.Base
{
    public interface IRepositoryBase<TModel>
    {
        // GET
        Task<TModel> GetByIdAsync(Guid id, bool includeChildren = false);
        // DELETE
        Task<bool> DeleteAsync(Guid id);
        // PATCH
        Task<bool> ChangeStatusAsync(ActiveStatus status);
    }

    public interface IRepositoryRoot<TModel> : IRepositoryBase<TModel>
        where TModel : RootDto
    {
        // POST and PUT
        Task<TModel> SaveAsync(TModel model);
        // GET
        Task<bool> ExistsAsync(Duplicate data);
        Task<DtoErrors> ValidateAsync(TModel model);
    }

    public interface IRepository<TModel> : IRepositoryBase<TModel>
        where TModel : BaseDto
    {
        // POST and PUT
        Task<TModel> SaveAsync(TModel model);
        // GET
        Task<bool> ExistsAsync(Duplicate data);
        Task<DtoErrors> ValidateAsync(TModel model);
    }

    public interface IRepositoryChild<TModel> : IRepositoryBase<TModel>
        where TModel : BaseDto
    {
        // POST and PUT
        Task<TModel> SaveAsync(Guid parentId, TModel model);
        // GET
        Task<bool> ExistsAsync(ChildDuplicate data);
        Task<DtoErrors> ValidateAsync(Guid parentId, TModel model);
    }

    #region Get data of children
    public interface IRepositoryChildData<TModel>
    {
        // GET
        Task<TModel> GetChildrenByIdAsync(Guid parentId);
    }
    #endregion

    #region List Repository
    public interface IRepositoryList<TList>
    {
        // GET
        IQueryable<TList> GetAll();
    }
    public interface IRepositoryChildList<TList>
    {
        // GET
        IQueryable<TList> GetAll(Guid parentId);
    }
    #endregion
}
