using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Models.Entities;

namespace PH.Core3.Common.Services.Components.EF.Crud.Entities
{
    public class ContextEntityReader<TContext, TEntity, TKey> : ContextServiceBase<TContext> 
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {

        private readonly ILogger<ContextEntityReader<TContext, TEntity, TKey>> _logger;

        /// <summary>
        /// Initialize a new service instance for work with <see cref="DbContext"/>
        /// </summary>
        /// <param name="coreIdentifier">Identifier</param>
        /// <param name="ctx"><see cref="DbContext"/> db context</param>
        /// <param name="tenantId">Tenant Identifier</param>
        /// <param name="logger"></param>
        public ContextEntityReader([NotNull] IIdentifier coreIdentifier, [NotNull] TContext ctx, [NotNull] string tenantId, ILogger<ContextEntityReader<TContext, TEntity, TKey>> logger) : base(coreIdentifier, ctx, tenantId)
        {
            _logger = logger;
            Set = ctx.Set<TEntity>();
            var ent = Activator.CreateInstance<TEntity>();
            EntityTypeName = ent.GetType().Name;
        }

        /// <summary>
        ///     DbSet
        /// </summary>
        protected internal readonly DbSet<TEntity> Set;


        /// <summary>
        /// Entity Type Name
        /// </summary>
        protected internal readonly string EntityTypeName;



        /// <summary>
        /// Find Entity by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns><see cref="Task{TEntity}"/> instance</returns>
        protected virtual async Task<TEntity> FindEntityByIdAsync([NotNull] TKey id)
        {
           

            var pending = Ctx.ChangeTracker.Entries<TEntity>().FirstOrDefault(x => x.Entity.Id.Equals(id));
            if (null != pending)
                return pending.Entity;

            return await Set.FirstOrDefaultAsync(x => x.Id.Equals(id));
            
        }
    }
}