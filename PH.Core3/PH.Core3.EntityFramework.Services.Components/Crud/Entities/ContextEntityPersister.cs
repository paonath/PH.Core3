using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.Result;
using PH.Core3.Common.Services.Components.Crud;
using PH.Core3.Common.Services.Components.Crud.Entities;
using PH.Core3.Common.Services.Crud;
using PH.Core3.Common.Settings;
using PH.Core3.EntityFramework.Abstractions.Models.Entities;

namespace PH.Core3.EntityFramework.Services.Components.Crud.Entities
{

    /// <summary>
    /// Service for Persist (CRUD op) Entities
    /// </summary>
    /// <typeparam name="TContext">Type of DbContext</typeparam>
    /// <typeparam name="TEntity">Type of Entity</typeparam>
    /// <typeparam name="TKey">Type of Entity Id Property</typeparam>
    public abstract class ContextEntityPersister<TContext, TEntity, TKey> :
        ContextEntityReader<TContext, TEntity, TKey>,
        ICrudAbstractionBase
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly ILogger<ContextEntityPersister<TContext, TEntity, TKey>> _logger;
        
        private readonly EntityValidator<TEntity, TKey> _entityValidator;
        private readonly TransientCrudSettings _crudSettings;

        /// <summary>
        /// If True the Service perform SaveChanges after every Add/Update/Delete.
        ///
        /// </summary>
        public bool AutoSaveChanges
        {
            get => _crudSettings.AutoSaveChanges;
            set => _crudSettings.AutoSaveChanges.SetValue(value);
        }

        /// <summary>
        /// If False do not perform a validation and return a valid result
        /// </summary>
        public SettingVariable<bool> InsertValidationEnabled => _crudSettings.InsertValidationEnabled;

        /// <summary>
        /// If False do not perform a validation and return a valid result
        /// </summary>
        public SettingVariable<bool> UpdateValidationEnabled => _crudSettings.UpdateValidationEnabled;


        /// <summary>
        /// If False do not perform a validation and return a valid result
        /// </summary>
        public SettingVariable<bool> DeleteValidationEnabled => _crudSettings.DeleteValidationEnabled;

        

        /// <summary>
        /// Init new instance of Service for CRUD Entity
        /// </summary>
        /// <param name="coreIdentifier">Cross-Scope Identifier</param>
        /// <param name="logger">Logger</param>
        /// <param name="ctx">Entity Framework Db Context</param>
        /// <param name="settings">Crud Settings</param>
        // ReSharper disable once IdentifierTypo
        protected ContextEntityPersister([NotNull] IIdentifier coreIdentifier,
                                         [NotNull] ILogger<ContextEntityPersister<TContext, TEntity, TKey>> logger
                                         , [NotNull] TContext ctx
                                         ,[NotNull] TransientCrudSettings settings
                                         /*, [NotNull] string tenantId*/)
            : base(coreIdentifier,  ctx, settings, /*tenantId,*/ logger)
        {
            _crudSettings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger       = logger;


            _entityValidator = new EntityValidator<TEntity, TKey>(coreIdentifier, FuncOnCreateValidation,
                                                                  FuncOnUpdateValidation, FuncOnDeleteValidation,
                                                                  logger, _crudSettings);
        }

        #region methods

        #region Validation

        private async Task<ValidationResult> FuncOnDeleteValidation(
            [CanBeNull] TEntity arg1, [NotNull] EntityValidationContext arg2)
        {
            if (null == arg1)
            {
                arg2.AddFailure("*", $"NULL {EntityTypeName} given.");
                return await arg2.GetValidationResult();
            }

            await ValidatePreDelete(arg1, arg2);
            return await arg2.GetValidationResult();
        }

        /// <summary>
        /// Async Validation for Delete Entity
        /// </summary>
        /// <param name="ent">Entity to Delete</param>
        /// <param name="c">Custom Validation Context</param>
        /// <returns>Task</returns>
        protected abstract Task ValidatePreDelete(TEntity ent, EntityValidationContext c);

        private async Task<ValidationResult> FuncOnUpdateValidation(
            [CanBeNull] TEntity arg1, [NotNull] EntityValidationContext arg2)
        {
            if (null == arg1)
            {
                arg2.AddFailure("*", $"NULL {EntityTypeName} given.");
                return await arg2.GetValidationResult();
            }

            await ValidatePreUpdate(arg1, arg2);
            return await arg2.GetValidationResult();
        }

        /// <summary>
        /// Validate on Insert
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>validation result</returns>
        protected async Task<ValidationResult> ValidateInsertAsync(TEntity entity)
        {
            return await _entityValidator.ValidateInsertAsync(entity);
        }

        /// <summary>
        /// Validate on Update
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>validation result</returns>
        protected async Task<ValidationResult> ValidateUpdateAsync(TEntity entity)
        {
            return await _entityValidator.ValidateUpdateAsync(entity);
        }

        /// <summary>
        /// Validate on Delete
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>validation result</returns>
        protected async Task<ValidationResult> ValidateDeleteAsync(TEntity entity)
        {
            return await _entityValidator.ValidateDeleteAsync(entity);
        }


        /// <summary>
        /// Async Validation for Update Entity
        /// </summary>
        /// <param name="ent">Entity to Edit</param>
        /// <param name="c">Custom Validation Context</param>
        /// <returns>Task</returns>
        protected abstract Task ValidatePreUpdate(TEntity ent, EntityValidationContext c);

        private async Task<ValidationResult> FuncOnCreateValidation([CanBeNull] TEntity arg1, [NotNull] EntityValidationContext arg2)
        {
            if (null == arg1)
            {
                arg2.AddFailure("*", $"NULL {EntityTypeName} given.");
                return await arg2.GetValidationResult();
            }


            await ValidatePreInsert(arg1, arg2);
            return await arg2.GetValidationResult();
        }

        /// <summary>
        /// Async Validation for Insert new Entity
        /// </summary>
        /// <param name="ent">Entity to Add</param>
        /// <param name="c">Custom Validation Context</param>
        /// <returns>Task</returns>
        protected abstract Task ValidatePreInsert(TEntity ent, EntityValidationContext c);

        #endregion


        /// <summary>
        /// Persist an Entity to Db
        /// </summary>
        /// <param name="entity">Entity to Add</param>
        /// <returns>Added Entity if valid for insert</returns>
        public async Task<TEntity> CreateEntityAsync([NotNull] TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //entity.Deleted              = false;
            //entity.CreatedTransactionId = Identifier.Uid;
            //entity.UpdatedTransactionId = Identifier.Uid;
            //entity.TenantId             = TenantId;



            var e = await Set.AddAsync(entity);

            if (AutoSaveChanges)
            {
                await Ctx.SaveChangesAsync();
            }

            return e.Entity;
        }


        ///// <summary>
        ///// Persist many entities to Db
        ///// </summary>
        ///// <param name="entities">Entities to add</param>
        ///// <returns>Added Entities if valids for insert </returns>
        //public async Task<TEntity[]> CreateManyEntityAsync([NotNull] IEnumerable<TEntity> entities)
        //{
        //    if (entities is null) throw new ArgumentNullException(nameof(entities));

        //    var toInsert = entities.ToArray();

        //    await Set.AddRangeAsync(toInsert);


        //    if (AutoSaveChanges)
        //        await Ctx.SaveChangesAsync();


        //    var idx = toInsert.Select(x => x.Id).ToArray();
        //    var r0  = await Set.Where(x => idx.Contains(x.Id)).ToArrayAsync();

        //    return r0;
        //}


        /// <summary>
        /// Update an Entity on Db
        /// </summary>
        /// <param name="entity">Entity to Update</param>
        /// <returns>Updated Entity if valid for update</returns>
        [ItemNotNull]
        public async Task<TEntity> UpdateEntityAsync([NotNull] TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            //entity.UpdatedTransactionId = Identifier.Uid;
            Ctx.Entry(entity).State     = EntityState.Modified;

            if (AutoSaveChanges)
            {
                await Ctx.SaveChangesAsync();
            }

            return entity;
        }


        /// <summary>
        /// Mark an Entity as Deleted
        /// </summary>
        /// <param name="entity">Entity to Remove</param>
        /// <returns>Removed entity if valid for delete</returns>
        [ItemNotNull]
        public async Task<TEntity> DeleteEntityAsync([NotNull] TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entity.Deleted              = true;
            
            return await UpdateEntityAsync(entity);
        }


        ///// <summary>
        ///// Async Add new Entity
        ///// </summary>
        ///// <param name="entity">Item to add</param>
        ///// <returns><see cref="Result{TEntity}"/> containing added Item or error</returns>
        //[ItemNotNull]
        //public virtual async Task<IResult<TEntity>> AddAsync([NotNull] TEntity entity)
        //{
        //    var addResult = await CreateEntityAsync(entity);
        //    return ResultFactory.Ok(Identifier, addResult);
        //}


        ///// <summary>
        ///// Async multiple Add new Items
        ///// </summary>
        ///// <param name="entities">Items to add</param>
        ///// <returns><see cref="Result{TContent}"/> containing added Items or error</returns>
        //[ItemNotNull]
        //public async Task<IResult<TEntity[]>> AddRangeAsync([NotNull] IEnumerable<TEntity> entities)
        //{
        //    var addResult = await CreateManyEntityAsync(entities);
        //    return ResultFactory.Ok(Identifier, addResult);
        //}

        /// <summary>
        /// Mark an Entity as Deleted
        /// </summary>
        /// <param name="entity">Entity to Remove</param>
        /// <returns><see cref="PH.Core3.Common.Result"/> containing True or error</returns>
        [ItemNotNull]
        public async Task<IResult<bool>> RemoveAsync([NotNull] TEntity entity)
        {
            var delResult = await DeleteEntityAsync(entity);
            return ResultFactory.TrueResult(Identifier);
        }


        /// <summary>
        /// Async Remove existing Item
        /// </summary>
        /// <param name="id">value of the Id property</param>
        /// <returns><see cref="PH.Core3.Common.Result"/> containing True or error</returns>
        [ItemNotNull]
        public async Task<IResult<bool>> RemoveByIdAsync([NotNull] TKey id)
        {
            

            var en = await FindEntityByIdAsync(id);
            if(null == en)
            {
                return ResultFactory.FalseResult(Identifier, new Error($"{EntityTypeName} with id '{id}' not found"));
            }
            
            return await RemoveAsync(en);
        }


        

        /// <summary>
        /// Init a Scope: on the end of this scope (on <see cref="IDisposable.Dispose"/>) flush changes.
        ///
        /// </summary>
        /// <param name="scopeName">Optional name for scope</param>
        /// <returns>IDisposable Scope</returns>
        public IDisposable BeginFlushChangesScope(string scopeName = "")
        {
            return FlushChangesScope.BeginScope(Identifier, Ctx);
        }


        /// <summary>
        /// Set <see cref="AutoSaveChanges"/> value
        /// </summary>
        /// <param name="status">If True the Service perform SaveChanges after every Add/Update/Delete</param>
        public void SetAutoSaveChangesStatus(bool status)
        {
            AutoSaveChanges = status;
        }

        #endregion


        /// <summary>
        /// Init a Scope in which validation is not performed.
        /// </summary>
        /// <param name="scopeName">Optional Scope Name</param>
        /// <returns>Disposable Scope</returns>
        [NotNull]
        public IDisposable BeginNoValidationScope(string scopeName = "")
        {
            var disposableScope = NoValidationScope.Instance(_logger, Identifier, scopeName);
            
            _crudSettings.InsertValidationEnabled.SetValue(false);
            _crudSettings.UpdateValidationEnabled.SetValue(false);
            _crudSettings.DeleteValidationEnabled.SetValue(false);

            disposableScope.DisposingNoValidationScope = (sender, args) =>
            {
                _entityValidator.InsertValidationEnabled.Reset();
                _entityValidator.UpdateValidationEnabled.Reset();
                _entityValidator.DeleteValidationEnabled.Reset();
                _logger.LogTrace("Reset Validation to initial value");
            };

            return disposableScope;
        }
    }

   
}