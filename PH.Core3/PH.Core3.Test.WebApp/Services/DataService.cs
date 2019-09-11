using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.Identifiers.Services;
using PH.Core3.Common.Services.Components.Crud;
using PH.Core3.Common.Services.Components.Crud.Entities;
using PH.Core3.EntityFramework.Services.Components.Crud;
using PH.Core3.TestContext;

namespace PH.Core3.Test.WebApp.Services
{
    public class DataService : CrudServiceBase<MyContext, TestData, TestDataDto, NewTestDataDto, EditTestDataDto, Guid>

    {
        /// <summary>
        /// Init new CRUD Service for Insert/Update/Delete 
        /// </summary>
        /// <param name="coreIdentifier">Cross Scope Identifier</param>
        /// <param name="logger">Logger</param>
        /// <param name="ctx">Entity Framework DbContext</param>
        /// <param name="settings">CRUD settings</param>
        public DataService([NotNull] IIdentifier coreIdentifier, [NotNull] ILogger<CrudServiceBase<MyContext, TestData, TestDataDto, NewTestDataDto, EditTestDataDto, Guid>> logger, [NotNull] MyContext ctx, [NotNull] TransientCrudSettings settings) : base(coreIdentifier, logger, ctx, settings)
        {
        }

        /// <summary>
        /// Service Identifier (a int value representing the service and the service name)
        /// </summary>
        public override ServiceIdentifier ServiceIdentifier { get; }

        /// <summary>
        /// Async Validation for Delete Entity
        /// </summary>
        /// <param name="ent">Entity to Delete</param>
        /// <param name="c">Custom Validation Context</param>
        /// <returns>Task</returns>
        protected override  Task ValidatePreDelete(TestData ent, EntityValidationContext c)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Async Validation for Update Entity
        /// </summary>
        /// <param name="ent">Entity to Edit</param>
        /// <param name="c">Custom Validation Context</param>
        /// <returns>Task</returns>
        protected override  Task ValidatePreUpdate(TestData ent, EntityValidationContext c)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Async Validation for Insert new Entity
        /// </summary>
        /// <param name="ent">Entity to Add</param>
        /// <param name="c">Custom Validation Context</param>
        /// <returns>Task</returns>
        protected override  Task ValidatePreInsert(TestData ent, EntityValidationContext c)
        {
            return Task.CompletedTask;
        }

        /// <summary>Parses the dto asynchronous.</summary>
        /// <param name="dto">The dto.</param>
        /// <returns></returns>
        protected override Task<TestData> ParseDtoAsync([NotNull] NewTestDataDto dto)
        {
            return Task.FromResult( new TestData() {Name = dto.data});
        }

        /// <summary>
        /// Merge entity properties with dto properties before perform un update.
        /// </summary>
        /// <param name="e">Entity</param>
        /// <param name="d">Dto</param>
        /// <returns>Entity with changed properties</returns>
        protected override  Task<TestData> MergeWithDtoAsync(TestData e, EditTestDataDto d)
        {
            e.Name = d.data;
            return Task.FromResult(e);
        }

        /// <summary>
        /// Transform a entity to a dto to return to consuming services.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>dto</returns>
        protected override TestDataDto ToDto(TestData entity)
        {
            return new TestDataDto()
                {data = entity.Name, Id = entity.Id, UtcLastUpdated = entity.UpdatedTransaction.UtcDateTime};
        }
    }
}