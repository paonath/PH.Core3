using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.Identifiers.Services;
using PH.Core3.Common.Models.ViewModels;
using PH.Core3.Common.Services.Components.EF;
using PH.Core3.Common.Services.Components.EF.Crud;
using PH.Core3.Common.Services.Components.EF.Crud.Entities;
using PH.Core3.TestContext;

namespace PH.Core3.Test.WebApp.Services
{
    
    public class NewAlberoDTo : INewDto
    {
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
       
    }

    public class EditAlberoDTo : NewAlberoDTo , IEditDto<Guid>
    {
        /// <summary>
        /// Unique Id of current class
        /// </summary>
        public Guid Id { get; set; }
    }

    public class AlberoDTo : EditAlberoDTo , IDtoResult<Guid>
    {

        public static AlberoDTo ToAlberoDTo(Albero a)
        {
            if (null == a)
                return null;

            return new AlberoDTo()
            {
                Id = a.Id, CategoryId = a.CategoryId, Description = a.Description
            };
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class AlberoService : CrudServiceBase<MyContext,Albero, AlberoDTo, NewAlberoDTo, EditAlberoDTo, Guid>
    {
        /// <summary>
        /// Init new CRUD Service for Insert/Update/Delete 
        /// </summary>
        /// <param name="coreIdentifier">Cross Scope Identifier</param>
        /// <param name="logger">Logger</param>
        /// <param name="ctx">Entity Framework DbContext</param>
        /// <param name="settings">CRUD settings</param>
        /// <param name="tenantId">Tenant Identifier</param>
        public AlberoService([NotNull] IIdentifier coreIdentifier, [NotNull] ILogger<CrudServiceBase<MyContext, Albero, AlberoDTo, NewAlberoDTo, EditAlberoDTo, Guid>> logger
                             , [NotNull] MyContext ctx, [NotNull] TransientCrudSettings settings) : base(coreIdentifier, logger, ctx, settings, ctx.TenantId)
        {
        }

        /// <summary>
        /// Async Validation for Delete Entity
        /// </summary>
        /// <param name="ent">Entity to Delete</param>
        /// <param name="c">Custom Validation Context</param>
        /// <returns>Task</returns>
        protected override async Task ValidatePreDelete(Albero ent, EntityValidationContext c)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Async Validation for Update Entity
        /// </summary>
        /// <param name="ent">Entity to Edit</param>
        /// <param name="c">Custom Validation Context</param>
        /// <returns>Task</returns>
        protected override async Task ValidatePreUpdate(Albero ent, EntityValidationContext c)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Async Validation for Insert new Entity
        /// </summary>
        /// <param name="ent">Entity to Add</param>
        /// <param name="c">Custom Validation Context</param>
        /// <returns>Task</returns>
        protected override async Task ValidatePreInsert(Albero ent, EntityValidationContext c)
        {
           // throw new NotImplementedException();
        }

        protected override async Task<Albero> ParseDtoAsync(NewAlberoDTo dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Merge entity properties with dto properties before perform un update.
        /// </summary>
        /// <param name="e">Entity</param>
        /// <param name="d">Dto</param>
        /// <returns>Entity with changed properties</returns>
        protected override async Task<Albero> MergeWithDtoAsync(Albero e, EditAlberoDTo d)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Transform a <see cref="TEntity">entity</see> to a <see cref="TDto">dto</see> to return to consuming services.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>dto</returns>
        protected override AlberoDTo ToDto(Albero entity)
        {
            return AlberoDTo.ToAlberoDTo(entity);
        }

        /// <summary>
        /// Service Identifier (a int value representing the service and the service name)
        /// </summary>
        public override ServiceIdentifier ServiceIdentifier => new ServiceIdentifier(73, nameof(AlberoService));
    }
}