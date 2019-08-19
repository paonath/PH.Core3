using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.Models.ViewModels;
using PH.Core3.Common.Models.ViewModels.Tree;
using PH.Core3.Common.Services.Components.Crud;
using PH.Core3.EntityFramework.Abstractions.Models.Entities;

namespace PH.Core3.EntityFramework.Services.Components.Crud
{
    /// <summary>
    /// Service Simplified for CRUD (Create/Read/Update/Delete) Operations on Tree Items.
    /// </summary>
    /// <typeparam name="TContext">Type of DbContext</typeparam>
    /// <typeparam name="TEntity">Type Of Entity</typeparam>
    /// <typeparam name="TDto">Type of the Result</typeparam>
    /// <typeparam name="TKey">Type of the Result and Edit Id Property (this must be a struct, e.g. a Guid)</typeparam>
    public abstract class
        TreeCrudService<TContext, TEntity, TDto, TKey> : TreeCrudServiceBase<TContext, TEntity, TDto, TDto, TDto, TKey>
        where TContext : DbContext
        where TEntity : class, ITreeEntity<TEntity, TKey>, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>
        // where TDto : class, IDtoResult<TKey>, IEditDto<TKey>, INewDto, IDto<TKey>, IIdentifiable<TKey>, IEditTreeDto<TKey>, ITreeItemDto<TDto, TKey>, ITreeDto

        where TDto : DtoResult<TKey>, ITreeNewDto<TKey>,INewDto, IEditTreeDto<TKey>, IDtoResult<TKey>

    {
        /// <summary>
        /// Init new CRUD Service for Insert/Update/Delete 
        /// </summary>
        /// <param name="coreIdentifier">Cross Scope Identifier</param>
        /// <param name="logger">Logger</param>
        /// <param name="ctx">Entity Framework DbContext</param>
        /// <param name="settings">CRUD settings</param>
        /// <param name="tenantId">Tenant Identifier</param>
        protected TreeCrudService([NotNull] IIdentifier coreIdentifier, [NotNull] ILogger<CrudServiceBase<TContext, TEntity, TDto, TDto, TDto, TKey>> logger
                                  , [NotNull] TContext ctx,  [NotNull] TransientCrudSettings settings, [NotNull] string tenantId) 
            : base(coreIdentifier, logger, ctx, settings, tenantId)
        {
        }

        

        private async Task<TEntity> MergeWithClassDtoAsync(TEntity e, TDto d)
        {
            return await MergeWithInterfaceDtoAsync(e,d);

        }

        


    }
}