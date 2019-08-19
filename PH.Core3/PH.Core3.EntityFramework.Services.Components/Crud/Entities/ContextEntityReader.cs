using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.Result;
using PH.Core3.Common.Services.Components.Crud;
using PH.Core3.Common.Services.Crud;
using PH.Core3.Common.Settings;
using PH.Core3.EntityFramework.Abstractions.Models.Entities;

namespace PH.Core3.EntityFramework.Services.Components.Crud.Entities
{
    /// <summary>
    /// Read Context Service
    /// </summary>
    /// <typeparam name="TContext">The type of the context.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <seealso cref="PH.Core3.EntityFramework.Services.Components.ContextServiceBase{TContext}" />
    public abstract class ContextEntityReader<TContext, TEntity, TKey> 
        : ContextServiceBase<TContext> , ISimpleReadService<TEntity,TKey>
        where TContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {

        private readonly ILogger<ContextEntityReader<TContext, TEntity, TKey>> _logger;
        private readonly TransientReadSettings _readSettings;


        /// <summary>
        /// Get or Set se Number of items retrieved by a LoadAll, or other paginated method.
        /// -1 For disabling pagination
        /// </summary>
        public SettingVariable<int> ItemsPaginationSize => _readSettings.ItemsPaginationSize;


        /// <summary>
        /// Initialize a new service instance for work with <see cref="DbContext"/>
        /// </summary>
        /// <param name="coreIdentifier">Identifier</param>
        /// <param name="ctx"><see cref="DbContext"/> db context</param>
        /// <param name="settings">read settings</param>
        /// <param name="tenantId">Tenant Identifier</param>
        /// <param name="logger"></param>
        protected ContextEntityReader([NotNull] IIdentifier coreIdentifier, [NotNull] TContext ctx, [NotNull] TransientReadSettings settings, [NotNull] string tenantId, ILogger<ContextEntityReader<TContext, TEntity, TKey>> logger) : base(coreIdentifier, ctx, tenantId)
        {
            _logger = logger;
            _readSettings = settings;

            Set = ctx.Set<TEntity>();
            var ent = Activator.CreateInstance<TEntity>();
            EntityTypeName = ent.GetType().Name;
        }

        /// <summary>
        ///     DbSet
        /// </summary>
        protected readonly DbSet<TEntity> Set;


        /// <summary>
        /// Entity Type Name
        /// </summary>
        protected readonly string EntityTypeName;



        /// <summary>
        /// Find Entity by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns><see cref="Task{TEntity}"/> instance</returns>
        protected virtual async Task<TEntity> FindEntityByIdAsync([NotNull] TKey id)
        {
           

            var pending = Ctx.ChangeTracker.Entries<TEntity>().FirstOrDefault(x => x.Entity.Id.Equals(id));
            if (null != pending)
            {
                return pending.Entity;
            }

            return await Set.FirstOrDefaultAsync(x => x.Id.Equals(id));
            
        }

        /// <summary>
        /// Find  by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns><see cref="PH.Core3.Common.Result"/> instance</returns>
        [ItemNotNull]
        public async Task<IResult<TEntity>> EntityFindByIdAsync([NotNull] TKey id)
        {
            var e = await FindEntityByIdAsync(id);
            if (null == e)
            {
                return ResultFactory.Fail<TEntity>(Identifier,
                                                       new Error($"{EntityTypeName} with id '{id}' not found"));
            }
            else
            {
                return ResultFactory.Ok(Identifier, e);
            }
        }

        /// <summary>
        /// Load All Items
        /// </summary>
        /// <returns><see cref="Result{T}"/> instance</returns>
        [ItemNotNull]
        public async Task<IResult<TEntity[]>> EntityLoadAllAsync()
        {
            //var c = await EntityLoadAsync(-1, -1);
            //return c;
            var arr = await Set.ToArrayAsync();
            return ResultFactory.Ok(Identifier, arr);

        }

        ///// <summary>
        ///// Load All Items
        ///// </summary>
        ///// <returns><see cref="IPagedResult{T}"/> instance</returns>
        //[ItemNotNull]
        //public async Task<IPagedResult<TEntity>> EntityPagedLoadAllAsync(int pageNumber = 0)
        //{
        //    if (ItemsPaginationSize.Value == -1)
        //    {
        //        return await EntityLoadAsync(-1, -1);
        //    }

        //    var itemsToSkip = pageNumber * ItemsPaginationSize.Value;
        //    return await EntityLoadAsync(itemsToSkip, ItemsPaginationSize.Value);
        //}


        ///// <summary>
        ///// Load All Items
        ///// </summary>
        ///// <returns><see cref="Result{T}"/> instance</returns>
        //public async Task<IResult<TEntity[]>> EntityLoadAllAsync()
        //{
        //    var r = await Set.ToArrayAsync();
        //    return ResultFactory.Ok(Identifier, r);
        //}



        
        ///// <summary>
        ///// Load Items as Paged Result
        ///// </summary>
        ///// <param name="skipItems">number of items to skip</param>
        ///// <param name="itemsToLoad">number of items to load</param>
        ///// <returns><see cref="PagedResult{TContent}"/> instance</returns>
        //[ItemNotNull]
        //public async Task<IPagedResult<TEntity>> EntityLoadAsync(int skipItems, int itemsToLoad)
        //{
        //    var c = await Set.LongCountAsync();
        //    if (c == 0)
        //    {
        //        return ResultFactory.PagedEmpty<TEntity>(Identifier);
        //    }


        //    TEntity[] all        = null;
        //    int       pageNumber = 0;

        //    if (skipItems == -1 && itemsToLoad == -1)
        //    {
        //        all        = await Set.ToArrayAsync();
        //        pageNumber = -1;
        //    }
        //    else
        //    {
        //        if (skipItems == 0)
        //        {
        //            pageNumber = 0;
        //        }
        //        else
        //        {
        //            pageNumber = (int) (c / skipItems);
        //        }

        //        all = await Set.Skip(skipItems).Take(itemsToLoad).ToArrayAsync();
        //    }

            
        //    return ResultFactory.PagedOk(Identifier, all, c, pageNumber, itemsToLoad);
        //}


        /*
        /// <summary>
        /// Load Items as Paged Result using default pagination setting
        /// </summary>
        /// <param name="pageNumber">page number</param>
        /// <returns><see cref="PagedResult{T}"/> instance</returns>
        public async Task<IPagedResult<TEntity>> EntityLoadAsync(int pageNumber)
        {
            throw new NotImplementedException();
        }
        */

        /*
        /// <summary>
        /// Load Items as Paged Result
        /// </summary>
        /// <param name="skipItems">number of items to skip</param>
        /// <param name="itemsToLoad">number of items to load</param>
        /// <returns><see cref="PagedResult{T}"/> instance</returns>
        public async Task<IPagedResult<TEntity>> EntityLoadAsync(int skipItems, int itemsToLoad)
        {
            throw new NotImplementedException();
        }*/
    }
}