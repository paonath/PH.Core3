﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Extensions;
using PH.Core3.Common.Models.Entities;
using PH.Core3.Common.Models.ViewModels;
using PH.Core3.Common.Result;
using PH.Core3.Common.Services.Crud;

namespace PH.Core3.Common.Services.Components.EF.Crud
{
    /// <summary>
    /// CRUD Service
    /// </summary>
    /// <typeparam name="TContext">Type of DbContext</typeparam>
    /// <typeparam name="TEntity">Type Of Entity</typeparam>
    /// <typeparam name="TDto">Type of Result Dto</typeparam>
    /// <typeparam name="TNewDto">Type of Insert Dto</typeparam>
    /// <typeparam name="TEditDto">Type of Edit Dto</typeparam>
    /// <typeparam name="TKey">Type of Id Property of bot Entity and Dto</typeparam>
    public abstract class CrudServiceBase<TContext, TEntity, TDto, TNewDto, TEditDto, TKey>
        : CrudServiceBaseInfrastructure<TContext, TEntity, TDto, TNewDto, TEditDto, TKey>
          , ICrudService<TDto, TNewDto, TEditDto, TKey>
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TNewDto : IDto, INewDto 
        where TEditDto :  TNewDto, IEditDto<TKey>, IDto<TKey>
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
        /// Wrap a <see cref="TEntity">entity</see> to a <see cref=" Result{TDto}">dto</see> to return to consuming services.
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
        /// Find <see cref="TDto"/> by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns><see cref="Result{TDto}"/> instance</returns>
        [ItemNotNull]
        public virtual async Task<IResult<TDto>> FindByIdAsync([NotNull] TKey id)
        {
            var e = await FindEntityByIdAsync(id);
            if (null == e)
                return ResultFactory.Fail<TDto>(Identifier, $"Unable to find {DtoTypeName} with id '{id}'");

            return ToResultOkDto(e);
        }


        /// <summary>
        /// Load All <see cref="TDto"/> Items
        /// </summary>
        /// <returns><see cref="Result{ToDto}"/> instance</returns>
        [ItemNotNull]
        public  virtual async Task<IResult<TDto[]>> LoadAllAsync()
        {
            var all = await Set.ToArrayAsync();
            
            var res = all.Select(ToDto).ToArray();
            return ResultFactory.Ok(Identifier, res);
        }

        /// <summary>
        /// Load Items as Paged Result using default pagination setting
        /// </summary>
        /// <param name="pageNumber">page number</param>
        /// <returns><see cref="PagedResult{TContent}"/> instance</returns>
        public async Task<IPagedResult<TDto>> LoadAsync(int pageNumber = 0)
        {
            if (ItemsPaginationSize.Value == -1)
                return await LoadAsync(-1, -1);

            var itemsToSkip = pageNumber * ItemsPaginationSize.Value;
            return await LoadAsync(itemsToSkip, ItemsPaginationSize.Value);
        }

        /// <summary>
        /// Load Items as Paged Result
        /// </summary>
        /// <param name="skipItems">number of items to skip</param>
        /// <param name="itemsToLoad">number of items to load</param>
        /// <returns><see cref="PagedResult{TContent}"/> instance</returns>
        public async Task<IPagedResult<TDto>> LoadAsync(int skipItems, int itemsToLoad)
        {
            var c = await Set.LongCountAsync();
            if (c == 0)
                return ResultFactory.PagedEmpty<TDto>(Identifier);



            TEntity[] all = null;
            int pageNumber = 0;

            if (skipItems == -1 && itemsToLoad == -1)
            {
                all = await Set.ToArrayAsync();
                pageNumber = -1;
            }
            else
            {
                if (skipItems == 0)
                    pageNumber = 0;
                else
                    pageNumber = (int) (c / skipItems);

            }

            var res = all?.Select(ToDto).ToArray();
            return ResultFactory.PagedOk(Identifier, res, c, pageNumber, itemsToLoad);
        }

        protected async Task<IPagedResult<TDto>> QueryPagedAsync(Expression<Func<TEntity, bool>> query,
                                                                 int pageNumber = 0)
        {
            throw new NotImplementedException();
        }

        protected async Task<IPagedResult<TDto>> QueryPagedAsync(Expression<Func<TEntity, bool>> query,int skipItems, int itemsToLoad)
        {
            throw new NotImplementedException();
            //ResultFactory.ChainAsync(Identifier, a )
        }




        /// <summary>
        /// Async Add new <see cref="TNewDto">item</see>
        /// </summary>
        /// <param name="entity">Item to Add</param>
        /// <returns><see cref="Result{TDto}"/> containing added Item or error</returns>
        [ItemNotNull]
        public  virtual async Task<IResult<TDto>> AddAsync(TNewDto entity)
        {
            var t = await ParseDtoAndValidateAsync(entity);
            if (t.InsertValidationResult.IsValid)
            {
                var ins = await CreateEntityAsync(t.EntityToInsert);
                
                return ResultFactory.Ok(Identifier,ToDto(ins));
            }
            else
            {
                return _logger.ErrorAndReturnFail<TDto>(Identifier, t.InsertValidationResult);
            }
        }


        /// <summary>
        /// Async remove a <see cref="TDto">dto</see>
        /// </summary>
        /// <param name="entity">Content to delete</param>
        /// <returns><see cref="Result"/> containing True or error</returns>
        [ItemNotNull]
        public  virtual async Task<IResult> RemoveAsync([NotNull] TDto entity)
        {
            var t = await GetEntityForDeleteAsync(entity.Id);
            if (t.DeleteValidationResult.IsValid)
            {
                return await RemoveAsync(t.EntityToDelete);
            }
            else
            {
                return _logger.ErrorAndReturnFail(Identifier,t.DeleteValidationResult);
            }
        }


        /// <summary>
        /// Async update a <see cref="TEditDto">dto</see>
        /// </summary>
        /// <param name="entity">Content to Update</param>
        /// <returns><see cref="Result{TDto}"/> result</returns>
        [ItemNotNull]
        public  virtual async Task<IResult<TDto>> UpdateAsync(TEditDto entity)
        {
            var e = await FindEntityByIdAsync(entity.Id);
            if (null == e)
                return _logger.CriticalAndReturnFail<TDto>(Identifier,$"Unable to find {EntityTypeName} with id {entity.Id}");
            var toUpdate = await MergeWithDtoAndValidateAsync(e, entity);
            if (toUpdate.UpdateValidationResult.IsValid)
            {
                var updated = await UpdateEntityAsync(toUpdate.EntityToUpdate);

                return ResultFactory.Ok(Identifier,ToDto(updated));
            }
            else
            {
                return _logger.ErrorAndReturnFail<TDto>(Identifier,toUpdate.UpdateValidationResult);
            }
        }
    }
}