using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.Models.Entities;
using PH.Core3.Common.Models.ViewModels;
using PH.Core3.Common.Models.ViewModels.Tree;
using PH.Core3.Common.Result;
using PH.Core3.Common.Services.Components.Crud;
using PH.Core3.Common.Services.Crud;

namespace PH.Core3.EntityFramework.Services.Components.Crud
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TNewDto"></typeparam>
    /// <typeparam name="TEditDto"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class
        TreeCrudServiceBase<TContext, TEntity, TDto, TNewDto, TEditDto, TKey> 
        : CrudServiceBase<TContext,TEntity, TDto, TNewDto, TEditDto, TKey> 
          , ITreeCrudService<TDto, TNewDto, TEditDto, TKey>
       

        where TContext : DbContext
        where TEntity :  class, ITreeEntity<TEntity,TKey>, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>
        where TNewDto : ITreeNewDto<TKey>,INewDto
        where TEditDto :  TNewDto, IEditTreeDto<TKey>, IEditDto<TKey>
        where TDto :   TEditDto, IDtoResult<TKey>

    {
        /// <summary>
        /// Init new CRUD Service for Insert/Update/Delete 
        /// </summary>
        /// <param name="coreIdentifier">Cross Scope Identifier</param>
        /// <param name="logger">Logger</param>
        /// <param name="ctx">Entity Framework DbContext</param>
        /// <param name="settings">CRUD settings</param>
        /// <param name="tenantId">Tenant Identifier</param>
        protected TreeCrudServiceBase([NotNull] IIdentifier coreIdentifier
                                      , [NotNull] ILogger<CrudServiceBase<TContext, TEntity, TDto, TNewDto, TEditDto, TKey>> logger
                                      , [NotNull] TContext ctx
                                      , [NotNull] TransientCrudSettings settings
                                      , [NotNull] string tenantId) 
            : base(coreIdentifier, logger, ctx, settings, tenantId)
        {
        }


        /// <summary>
        /// Parse dto and return new entity for insert.
        ///
        /// This method do not perform Update on Db: just made changes on entity and check if is valid for insert.
        /// </summary>
        /// <param name="dto">dto to add</param>
        /// <returns>Entity for insert</returns>
        protected override async Task<(TEntity EntityToInsert, ValidationResult InsertValidationResult)> ParseDtoAndValidateAsync(TNewDto dto)
        {
            var e = await ParseDtoAsync(dto);
            e.EntityLevel = 0;
            e.RootId = e.Id;

            if (e.ParentId.HasValue)
            {
                var parent = await FindEntityByIdAsync(e.ParentId.Value);
                e.Parent = parent;
                e.RootEntity = parent.RootEntity;
                int eLevel = parent.EntityLevel + 1;
                e.EntityLevel = eLevel;
            }
            

            var v = await ValidateInsertAsync(e);
            return (e, v);
        }

        /// <summary>
        /// Load All Items as Tree
        /// </summary>
        /// <returns><see cref="Result{TContent}"/> instance</returns>
        [ItemNotNull]
        public async Task<IResult<TDto[]>> LoadAllAsTreeAsync()
        {
            int p = 0;

            var all = await Set.ToArrayAsync();
            var res = all.Where(x => x.EntityLevel == p).Select(ToDto).ToArray();
            return ResultFactory.Ok(Identifier,res);


        }


         

    }

}