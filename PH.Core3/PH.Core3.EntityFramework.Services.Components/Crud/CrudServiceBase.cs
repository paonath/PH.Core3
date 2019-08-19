using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Extensions;
using PH.Core3.Common.Models.ViewModels;
using PH.Core3.Common.Result;
using PH.Core3.Common.Services.Components.Crud;
using PH.Core3.Common.Services.Crud;
using PH.Core3.EntityFramework.Abstractions.Models.Entities;

namespace PH.Core3.EntityFramework.Services.Components.Crud
{
    /// <summary>
    /// CRUD Service
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TDto">The type of the dto.</typeparam>
    /// <typeparam name="TNewDto">The type of the new dto.</typeparam>
    /// <typeparam name="TEditDto">The type of the edit dto.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <seealso cref="PH.Core3.EntityFramework.Services.Components.Crud.CrudServiceBaseInfrastructure{TContext, TEntity, TDto, TNewDto, TEditDto, TKey}" />
    /// <seealso cref="PH.Core3.Common.Services.Crud.ICrudService{TDto, TNewDto, TEditDto, TKey}" />
    public abstract class CrudServiceBase<TContext, TEntity, TDto, TNewDto, TEditDto, TKey>
        : CrudServiceBaseInfrastructure<TContext, TEntity, TDto, TNewDto, TEditDto, TKey>
          , ICrudService<TDto, TNewDto, TEditDto, TKey>
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TNewDto : IDto, INewDto
        where TEditDto : TNewDto, IEditDto<TKey>, IDto<TKey>
        where TDto : IDtoResult<TKey>, TEditDto, IDto<TKey>, IIdentifiable<TKey>


    {
        private readonly ILogger<CrudServiceBase<TContext, TEntity, TDto, TNewDto, TEditDto, TKey>> _logger;

        /// <summary>
        /// Init new CRUD Service for Insert/Update/Delete 
        /// </summary>
        /// <param name="coreIdentifier">Cross Scope Identifier</param>
        /// <param name="logger">Logger</param>
        /// <param name="ctx">Entity Framework DbContext</param>
        /// <param name="settings">CRUD settings</param>
        /// <param name="tenantId">Tenant Identifier</param>
        protected CrudServiceBase([NotNull] IIdentifier coreIdentifier
                                  , [NotNull]
                                  ILogger<CrudServiceBase<TContext, TEntity, TDto, TNewDto, TEditDto, TKey>> logger
                                  , [NotNull] TContext ctx
                                  , [NotNull] TransientCrudSettings settings, [NotNull] string tenantId)
            : base(coreIdentifier, logger, ctx, settings, tenantId)
        {
            _logger = logger;
        }


        /// <summary>
        /// Wrap a entity to a <see cref=" Result{TDto}">dto</see> to return to consuming services.
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>Result </returns>
        [NotNull]
        protected internal override IResult<TDto> ToResultOkDto(TEntity entity)
        {
            var r = ToDto(entity);
            return ResultFactory.Ok(Identifier, r);
        }

        /// <summary>
        /// Find Dto by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns><see cref="Result{TDto}"/> instance</returns>
        [ItemNotNull]
        public virtual async Task<IResult<TDto>> FindByIdAsync([NotNull] TKey id)
        {
            var e = await FindEntityByIdAsync(id);
            if (null == e)
            {
                return ResultFactory.Fail<TDto>(Identifier, $"Unable to find {DtoTypeName} with id '{id}'");
            }

            return ToResultOkDto(e);
        }


        /// <summary>
        /// Load All Dto Items
        /// </summary>
        /// <returns><see cref="Result{ToDto}"/> instance</returns>
        [ItemNotNull]
        public virtual async Task<IResult<TDto[]>> LoadAllAsync()
        {
            var all = await Set.ToArrayAsync();

            var res = all.Select(ToDto).ToArray();
            return ResultFactory.Ok(Identifier, res);
        }

        ///// <summary>
        ///// Load Items as Paged Result using default pagination setting
        ///// </summary>
        ///// <param name="pageNumber">page number</param>
        ///// <returns><see cref="PagedResult{TContent}"/> instance</returns>
        //[ItemNotNull]
        //public async Task<IPagedResult<TDto>> LoadAsync(int pageNumber = 0)
        //{
        //    var b = await EntityPagedLoadAllAsync(pageNumber);
        //    if (b.OnError)
        //    {
        //        return ResultFactory.PagedFail<TDto>(Identifier, b.Errors);
        //    }
        //    else
        //    {
        //        return ResultFactory.PagedOk(Identifier, b.Content.Select(x => ToDto(x)).ToArray(), b.Count,
        //                                     b.PageNumber, b.PageSize);
        //    }
        //}

        ///// <summary>
        ///// Load Items as Paged Result
        ///// </summary>
        ///// <param name="skipItems">number of items to skip</param>
        ///// <param name="itemsToLoad">number of items to load</param>
        ///// <returns><see cref="PagedResult{TContent}"/> instance</returns>
        //[ItemNotNull]
        //public async Task<IPagedResult<TDto>> LoadAsync(int skipItems, int itemsToLoad)
        //{
        //    var b = await EntityLoadAsync(skipItems, itemsToLoad);
        //    if (b.OnError)
        //    {
        //        return ResultFactory.PagedFail<TDto>(Identifier, b.Errors);
        //    }
        //    else
        //    {
        //        return ResultFactory.PagedOk(Identifier, b.Content.Select(x => ToDto(x)).ToArray(), b.Count,
        //                                     b.PageNumber, b.PageSize);
        //    }
        //}

        /*
        protected Task<IPagedResult<TDto>> QueryPagedAsync(Expression<Func<TEntity, bool>> query,
                                                                 int pageNumber = 0)
        {
            throw new NotImplementedException();
        }

        protected Task<IPagedResult<TDto>> QueryPagedAsync(Expression<Func<TEntity, bool>> query,int skipItems, int itemsToLoad)
        {
            throw new NotImplementedException();
            //ResultFactory.ChainAsync(Identifier, a )
        }
        */


        /// <summary>
        /// Async Add new Dto item
        /// </summary>
        /// <param name="entity">Item to Add</param>
        /// <returns><see cref="Result{TDto}"/> containing added Item or error</returns>
        [ItemNotNull]
        public virtual async Task<IResult<TDto>> AddAsync(TNewDto entity)
        {
            var t = await ParseDtoAndValidateAsync(entity);
            if (!t.InsertValidationResult.IsValid)
            {
                return _logger.ErrorAndReturnFail<TDto>(Identifier, t.InsertValidationResult);
            }

            try
            {
                var ins = await CreateEntityAsync(t.EntityToInsert);

                return ResultFactory.Ok(Identifier, ToDto(ins));
            }
            catch (Exception e)
            {
                return _logger.CriticalAndReturnFail<TDto>(Identifier, e);
            }
        }


        /// <summary>
        /// Async remove a Dto
        /// </summary>
        /// <param name="entity">Content to delete</param>
        /// <returns><see cref="PH.Core3.Common.Result"/> containing True or error</returns>
        [ItemNotNull]
        public virtual async Task<IResult<bool>> RemoveAsync([NotNull] TDto entity)
        {
            var t = await GetEntityForDeleteAsync(entity.Id);
            if (!t.DeleteValidationResult.IsValid)
            {
                return _logger.ErrorAndReturnFail<bool>(Identifier, t.DeleteValidationResult);
            }

            try
            {
                return await RemoveAsync(t.EntityToDelete);
            }
            catch (Exception e)
            {
                return _logger.CriticalAndReturnFail<bool>(Identifier, e);
            }
        }


        /// <summary>
        /// Async update a dto
        /// </summary>
        /// <param name="entity">Content to Update</param>
        /// <returns><see cref="Result{TDto}"/> result</returns>
        [ItemNotNull]
        public virtual async Task<IResult<TDto>> UpdateAsync(TEditDto entity)
        {
            var e = await FindEntityByIdAsync(entity.Id);
            if (null == e)
            {
                return _logger.CriticalAndReturnFail<TDto>(Identifier,
                                                           $"Unable to find {EntityTypeName} with id {entity.Id}");
            }

            var toUpdate = await MergeWithDtoAndValidateAsync(e, entity);
            if (!toUpdate.UpdateValidationResult.IsValid)
            {
                return _logger.ErrorAndReturnFail<TDto>(Identifier, toUpdate.UpdateValidationResult);

            }

            try
            {

                var updated = await UpdateEntityAsync(toUpdate.EntityToUpdate);

                return ResultFactory.Ok(Identifier, ToDto(updated));

            }
            catch (Exception exception)
            {
                return _logger.CriticalAndReturnFail<TDto>(Identifier, exception);
            }
        }
    }
}