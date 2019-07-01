using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.Identifiers.Services;
using PH.Core3.Common.Models.ViewModels;
using PH.Core3.Common.Services.Components.Crud;
using PH.Core3.Common.Services.Components.Crud.Entities;
using PH.Core3.EntityFramework.Services.Components.Crud;
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

        [CanBeNull]
        public static AlberoDTo ToAlberoDTo([CanBeNull] Albero a)
        {
            if (null == a)
            {
                return null;
            }

            return new AlberoDTo()
            {
                Id = a.Id, CategoryId = a.CategoryId, Description = a.Description, UtcLastUpdated = a.UpdatedTransaction?.UtcDateTime
            };
        }

        /// <summary>Gets the UTC last updated date and time for current entity.</summary>
        /// <value>The UTC last updated.</value>
        public DateTime? UtcLastUpdated { get; protected set; }
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
        [NotNull]
        protected override Task ValidatePreDelete(Albero ent, EntityValidationContext c)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Async Validation for Update Entity
        /// </summary>
        /// <param name="ent">Entity to Edit</param>
        /// <param name="c">Custom Validation Context</param>
        /// <returns>Task</returns>
        [NotNull]
        protected override  Task ValidatePreUpdate(Albero ent, EntityValidationContext c)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Async Validation for Insert new Entity
        /// </summary>
        /// <param name="ent">Entity to Add</param>
        /// <param name="c">Custom Validation Context</param>
        /// <returns>Task</returns>
        [NotNull]
        protected override Task ValidatePreInsert(Albero ent, EntityValidationContext c)
        {
           // throw new NotImplementedException();
           return Task.CompletedTask;
        }

        protected override  Task<Albero> ParseDtoAsync(NewAlberoDTo dto)
        {
            throw new NotImplementedException();
            //return Task.CompletedTask;
        }

        /// <summary>
        /// Merge entity properties with dto properties before perform un update.
        /// </summary>
        /// <param name="e">Entity</param>
        /// <param name="d">Dto</param>
        /// <returns>Entity with changed properties</returns>
        protected override  Task<Albero> MergeWithDtoAsync(Albero e, EditAlberoDTo d)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Transform a entity to a dto to return to consuming services.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>dto</returns>
        [CanBeNull]
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