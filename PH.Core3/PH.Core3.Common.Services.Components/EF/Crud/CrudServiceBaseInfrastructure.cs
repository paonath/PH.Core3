using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Models.Entities;
using PH.Core3.Common.Models.ViewModels;
using PH.Core3.Common.Result;
using PH.Core3.Common.Services.Components.EF.Crud.Entities;

namespace PH.Core3.Common.Services.Components.EF.Crud
{
    /// <summary>
    /// CRUD Service Infrastructure
    /// </summary>
    /// <typeparam name="TContext">Type of DbContext</typeparam>
    /// <typeparam name="TEntity">Type Of Entity</typeparam>
    /// <typeparam name="TDto">Type of Result Dto</typeparam>
    /// <typeparam name="TNewDto">Type of Insert Dto</typeparam>
    /// <typeparam name="TEditDto">Type of Edit Dto</typeparam>
    /// <typeparam name="TKey">Type of Id Property of bot Entity and Dto</typeparam>
    public abstract class CrudServiceBaseInfrastructure<TContext, TEntity, TDto, TNewDto, TEditDto, TKey>
        : ContextEntityPersister<TContext, TEntity, TKey>
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>

        where TNewDto : IDto, INewDto
        where TEditDto : TNewDto, IEditDto<TKey>, IDto<TKey>
        where TDto :  IDtoResult<TKey>,  TEditDto, IDto<TKey>, IIdentifiable<TKey>
    {
        /// <summary>
        /// <see cref="IDto">DTO</see> Type Name
        /// </summary>
        protected internal readonly string DtoTypeName;


        /// <summary>
        /// Init new instance of Service for CRUD Entity
        /// </summary>
        /// <param name="coreIdentifier">Cross-Scope Identifier</param>
        /// <param name="logger">Logger</param>
        /// <param name="ctx">Entity Framework Db Context</param>
        /// <param name="settings">CRUD Settings</param>
        /// <param name="tenantId">Tenant Identifier</param>
        protected CrudServiceBaseInfrastructure([NotNull] IIdentifier coreIdentifier
                                                , [NotNull] ILogger<CrudServiceBaseInfrastructure<TContext, TEntity, TDto, TNewDto, TEditDto, TKey>> logger
                                                , [NotNull] TContext ctx
                                                , [NotNull] TransientCrudSettings settings
                                                , [NotNull] string tenantId) 
            : base(coreIdentifier, logger, ctx,  settings,tenantId)
        {
            var dt = Activator.CreateInstance<TDto>();
            DtoTypeName = dt?.GetType()?.Name;
        }


        protected abstract Task<TEntity> ParseDtoAsync(TNewDto dto);

        /// <summary>
        /// Parse <see cref="TNewDto">dto</see> and return new <see cref="TEntity">entity</see> for insert.
        ///
        /// This method do not perform Update on Db: just made changes on entity and check if is valid for insert.
        /// </summary>
        /// <param name="dto">dto to add</param>
        /// <returns>Entity for insert</returns>
        protected internal virtual async 
            Task<(TEntity EntityToInsert, ValidationResult InsertValidationResult)>
            ParseDtoAndValidateAsync(TNewDto dto)
        {
            var e = await ParseDtoAsync(dto);
            var v = await ValidateInsertAsync(e);
            return (e, v);
        }


        /// <summary>
        /// Merge entity properties with dto properties before perform un update.
        /// </summary>
        /// <param name="e">Entity</param>
        /// <param name="d">Dto</param>
        /// <returns>Entity with changed properties</returns>
        protected abstract Task<TEntity> MergeWithDtoAsync(TEntity e, TEditDto d);

        protected internal async Task<TEntity> MergeWithInterfaceDtoAsync(TEntity e, TEditDto d)
        {
            return await MergeWithDtoAsync(e, d);
        }


        /// <summary>
        /// Merge changes between <see cref="TEditDto">dto</see> and <see cref="TEntity">entity</see> for update.
        /// This method do not perform Update on Db: just made changes on entity and check if is valid for updating.
        /// </summary>
        /// <param name="e">entity to update</param>
        /// <param name="d">dto</param>
        /// <returns>entity ready for update</returns>
        protected internal virtual async 
            Task<(TEntity EntityToUpdate, ValidationResult UpdateValidationResult)>
            MergeWithDtoAndValidateAsync(TEntity e, TEditDto d)
        {
            var u = await MergeWithDtoAsync(e,d);
            var v = await ValidateUpdateAsync(e);
            return (e, v);
        }


        /// <summary>
        /// Retrieve Entity for Delete.
        ///
        /// This method do not perform Update on Db: just retrieve entity and check if is valid for delete.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal virtual async
            Task<(TEntity EntityToDelete, ValidationResult DeleteValidationResult)>
            GetEntityForDeleteAsync([NotNull] TKey id)
        {
            var e = await FindEntityByIdAsync(id);
            var v = await ValidateDeleteAsync(e);

            return (e, v);
        }

        /// <summary>
        /// Transform a <see cref="TEntity">entity</see> to a <see cref="TDto">dto</see> to return to consuming services.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>dto</returns>
        protected internal abstract TDto ToDto(TEntity entity);

        /// <summary>
        /// Wrap a <see cref="TEntity">entity</see> to a <see cref=" Result{TDto}">dto</see> to return to consuming services.
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>Result </returns>
        [NotNull]
        protected internal abstract IResult<TDto> ToResultOkDto([CanBeNull] TEntity entity);

    }
}