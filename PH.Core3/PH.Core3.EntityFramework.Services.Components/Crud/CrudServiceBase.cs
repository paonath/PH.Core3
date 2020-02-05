using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Extensions;
using PH.Core3.Common.Models.ViewModels;

using PH.Core3.Common.Services.Components.Crud;
using PH.Core3.Common.Services.Crud;
using PH.Results;
using PH.Results.Internals;
using PH.UowEntityFramework.EntityFramework.Abstractions.Models;

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
        protected CrudServiceBase([NotNull] IIdentifier coreIdentifier
                                  , [NotNull]
                                  ILogger<CrudServiceBase<TContext, TEntity, TDto, TNewDto, TEditDto, TKey>> logger
                                  , [NotNull] TContext ctx
                                  , [NotNull] TransientCrudSettings settings/*, [NotNull] string tenantId*/)
            : base(coreIdentifier, logger, ctx, settings/*, tenantId*/)
        {
            _logger = logger;
        }

      

        /// <summary>
        /// Wrap a entity to a <see cref=" Result{TContent}">dto</see> to return to consuming services.
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

        internal static (StringBuilder errorDescr, Error error) PrepareValidationFailures([NotNull] ValidationResult fluentValidationResult)
        {
            StringBuilder sb   = new StringBuilder();
            StringBuilder si = new StringBuilder();
            foreach (var failure in fluentValidationResult.Errors)
            {
                string msg = $"{failure.ErrorMessage}; ";
                if (!StringExtensions.IsNullString(failure.PropertyName) && failure.PropertyName != "*")
                {
                    msg = $"PropertyName '{failure.PropertyName}' - ErrorMessage '{failure.ErrorMessage}'; ";
                }

                sb.Append(msg);
                si.Append(msg);
                
            }

            var error = new Error(si.ToString());
            return (sb, error);
        }


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
                var err = PrepareValidationFailures(t.InsertValidationResult);
               // _logger.LogError(err.errorDescr.ToString());
               _logger.LogError($"add err: {err.errorDescr.ToString()}");
                return ResultFactory.Fail<TDto>(Identifier, err.error);

                
            }

            try
            {
                var ins = await CreateEntityAsync(t.EntityToInsert);

                return ResultFactory.Ok(Identifier, ToDto(ins));
            }
            catch (Exception e)
            {
                var err = ResultFactory.Fail<TDto>(Identifier, e);
                _logger.LogCritical(err.Error);
                return err;

                //return _logger.CriticalAndReturnFail<TDto>(Identifier, e);
            }
        }


        /// <summary>
        /// Async remove a Dto
        /// </summary>
        /// <param name="entity">Content to delete</param>
        /// <returns><see cref="Result{TContent}"/> containing True or error</returns>
        [ItemNotNull]
        public virtual async Task<IResult<bool>> RemoveAsync([NotNull] TDto entity)
        {
            var t = await GetEntityForDeleteAsync(entity.Id);
            if (!t.DeleteValidationResult.IsValid)
            {
                var err = PrepareValidationFailures(t.DeleteValidationResult);
                //_logger.LogError(err.errorDescr.ToString());
                _logger.LogError($"remove err: {err.errorDescr.ToString()}");
                return ResultFactory.Fail<bool>(Identifier, err.error);

                //return _logger.ErrorAndReturnFail<bool>(Identifier, t.DeleteValidationResult);
            }

            try
            {
                return await RemoveAsync(t.EntityToDelete);
            }
            catch (Exception e)
            {
                var err = ResultFactory.Fail<bool>(Identifier, e);
                _logger.LogError(e, $"Unable to remove: {e.Message}");
                return err;
                //return _logger.CriticalAndReturnFail<bool>(Identifier, e);
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
                var err = ResultFactory.Fail<TDto>(Identifier, $"Unable to find {EntityTypeName} with id {entity.Id}");
                _logger.LogError(err.Error);
                return err;
            }

            var toUpdate = await MergeWithDtoAndValidateAsync(e, entity);
            if (!toUpdate.UpdateValidationResult.IsValid)
            {
                var err = PrepareValidationFailures(toUpdate.UpdateValidationResult);
                _logger.LogError($"update err: {err.errorDescr.ToString()}");
                return ResultFactory.Fail<TDto>(Identifier, err.error);

                //return _logger.ErrorAndReturnFail<TDto>(Identifier, toUpdate.UpdateValidationResult);

            }

            try
            {

                var updated = await UpdateEntityAsync(toUpdate.EntityToUpdate);

                return ResultFactory.Ok(Identifier, ToDto(updated));

            }
            catch (Exception exception)
            {
                var r = ResultFactory.Fail<TDto>(Identifier, exception);
                _logger.LogCritical(exception, exception.Message);
                return r;
                //return _logger.CriticalAndReturnFail<TDto>(Identifier, exception);
            }
        }
    }
}