using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Scope;
using PH.Core3.Common.UnitOfWorkInfrastructure;
using PH.Core3.EntityFramework.Abstractions.Models.Entities;
using PH.Core3.EntityFramework.Infrastructure;
using PH.Core3.EntityFramework.Mapping;
using PH.Core3.UnitOfWork;

namespace PH.Core3.EntityFramework
{
    /// <summary>
    /// Core Identitity Context
    ///
    /// <see cref="Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext{TUser, TRole, TKey}"/>
    /// </summary>
    /// <typeparam name="TUser">Type of User Entity class</typeparam>
    /// <typeparam name="TRole">Type of Role Entity class</typeparam>
    /// <typeparam name="TKey">Type of User and Role Id Property</typeparam>
    public abstract class IdentityBaseContext<TUser, TRole, TKey> :
        IdentityBaseContextInfrastructure<TUser, TRole, TKey>,
        IAuditContext, IIdentityBaseContext<TUser, TRole, TKey> , IDbContextUnitOfWork
        
        where TUser : IdentityUser<TKey>, IEntity<TKey>
        where TRole : IdentityRole<TKey>, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>Gets the scope dictionary.</summary>
        /// <value>The scope dictionary.</value>
        public Dictionary<int, string> ScopeDictionary { get; }
        private int _scopeCount;

        

        ///// <summary>
        ///// Identifier
        ///// </summary>
        //public IIdentifier Identifier { get; set; }

        /// <summary>Gets or sets the author.</summary>
        /// <value>The author.</value>
        public string Author { get; set; }

        /// <summary>
        /// Ctx Uid
        /// </summary>
        public Guid CtxUid { get; }

        
        /// <summary>
        /// Transaction Audit
        /// </summary>
        public DbSet<TransactionAudit> TransactionAudits { get; set; }


        private IDbContextTransaction _transaction;

        /// <summary>Gets or sets the cancellation token source.</summary>
        /// <value>The cancellation token source.</value>
        public CancellationTokenSource CancellationTokenSource { get; set; }

        /// <summary>Gets or sets the cancellation token.</summary>
        /// <value>The cancellation token.</value>
        public CancellationToken CancellationToken { get; protected set; }

        private TransactionAudit _transactionAudit;


        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityBaseContext{TUser, TRole, TKey}"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <exception cref="ArgumentNullException">options</exception>
        protected IdentityBaseContext([NotNull] DbContextOptions options)
            : base(options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Disposed                = false;
            Initialized             = false;
            CtxUid = NewId.NextGuid(); //Guid.NewGuid();
            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken       = CancellationTokenSource.Token;
            ScopeDictionary         = new Dictionary<int, string>();
            _scopeCount             = 0;
        }



        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the database.
        /// </returns>
        /// <remarks>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        /// changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </remarks>
        public sealed override int SaveChanges()
        {
            return base.SaveBaseChanges(Identifier, Author);
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges" /> is called after the changes have
        /// been sent successfully to the database.</param>
        /// <returns>
        /// The number of state entries written to the database.
        /// </returns>
        /// <remarks>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        /// changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </remarks>
        public sealed override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveBaseChanges(Identifier, Author, acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the
        /// number of state entries written to the database.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        /// changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </para>
        /// <para>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </para>
        /// </remarks>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveBaseChangesAsync(Identifier, Author, cancellationToken);
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges" /> is called after the changes have
        /// been sent successfully to the database.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the
        /// number of state entries written to the database.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        /// changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </para>
        /// <para>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </para>
        /// </remarks>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveBaseChangesAsync(Identifier, Author, acceptAllChangesOnSuccess, cancellationToken);
        }


        #region Disposing 

        /// <summary>
        /// If Dispose already performed
        /// </summary>
        public bool Disposed { get; protected set; }

        /// <summary>Occurs when [disposed evt].</summary>
        public event EventHandler<CoreDisposableEventArgs> DisposedEvt;

        /// <summary>
        /// Dispose Pattern.
        /// This method check if already <see cref="Disposed"/> (and set it to True).
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !Disposed)
            {
                base.Dispose();

                Disposed = true;
                DisposedEvt?.Invoke(this, new CoreDisposableEventArgs(Identifier));
            }
        }

        /// <summary>
        ///     Releases the allocated resources for this context.
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion


        #region Db Contenxt and Config

        /// <summary>
        /// Configures the schema needed for the identity framework. 
        /// </summary>
        /// <param name="builder">
        /// The builder being used to construct the model for this context.
        /// </param>
        protected sealed override void OnModelCreating([NotNull] ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            OnCustomModelCreating(builder);

            builder.ApplyConfiguration<TransactionAudit>(new TransactionAuditMap());
            AssignTenantAndOtherQueryFilterOnModelCreating(builder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public abstract void OnCustomModelCreating([NotNull] ModelBuilder builder);


        /// <summary>
        /// Assign Query Filters base and other set on <see cref="IEntityTypeConfiguration{TEntity}">map</see>
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AssignTenantAndOtherQueryFilterOnModelCreating([NotNull] ModelBuilder builder)
        {
            var allTypes = ScanAssemblyTypes();
            var tt       = GetAllEntityTypes(allTypes);



            foreach (Type entityType in tt)
            {
                var entityName     = entityType.Name;
                var entityFullName = entityType?.FullName ?? entityName;

                try
                {

                    #region QueryFilters

                    
                    var paramName      = $"{entityName}_p";

                    var entityTypeFromModel = builder.Model.FindEntityType(entityFullName);
                    var queryFilter         = entityTypeFromModel.QueryFilter;
                    var queryParam          = queryFilter?.Parameters.FirstOrDefault();
                    if (null != queryParam)
                    {
                        paramName = queryParam.Name;
                    }


                    ParameterExpression paramExpr = Expression.Parameter(entityType, paramName);
                    Expression bodyTenant = Expression.Equal(Expression.Property(paramExpr, "TenantId"),
                                                             Expression.Constant(TenantId)
                                                            );
                    Expression bodyDeleted = Expression.Equal(Expression.Property(paramExpr, "Deleted"),
                                                              Expression.Constant(false)
                                                             );
                    Expression body = null;
                    if (null != queryFilter)
                    {
                        var res = Expression.Lambda(Expression.Invoke(queryFilter, paramExpr), paramExpr);


                        body = Expression.AndAlso(Expression.AndAlso(bodyDeleted, bodyTenant), res.Body);
                    }
                    else
                    {
                        body = Expression.AndAlso(bodyDeleted, bodyTenant);
                    }


                    var name = $"TenantQueryFilter_{entityName}";

                    LambdaExpression lambdaExpr = Expression.Lambda(body, name,
                                                                    new List<ParameterExpression>() { paramExpr }
                                                                   );

                    var entyTYpeBuilder = builder.Entity(entityType);


                    entyTYpeBuilder.HasQueryFilter(lambdaExpr);

                    #endregion

                }
                catch (Exception e)
                {
                    var err = $"Error configuring '{entityName}'";
                    Logger.LogCritical(err, e);
                    throw new Exception(err, e);
                }

            }
        }


        //[NotNull]
        //protected virtual Type[] ScanAssemblyTypes()
        //{
        //    return GetType().Assembly.GetTypes();
        //}

        /// <summary>Scans the assembly types.</summary>
        /// <returns></returns>
        protected abstract Type[] ScanAssemblyTypes();

        /// <summary>Gets all entity types.</summary>
        /// <param name="allTypes">All types.</param>
        /// <returns></returns>
        [NotNull]
        protected virtual Type[] GetAllEntityTypes([NotNull] Type[] allTypes)
        {
            var entityTypes = allTypes.Where(x => x.IsClass && !x.IsAbstract && typeof(IEntity).IsAssignableFrom(x))
                                      .OrderBy(x => x.Name).ToArray();

            return entityTypes;
        }








        #endregion

        #region Unit Of Work 

        /// <summary>Begins the transaction.</summary>
        public void BeginTransaction()
        {
            DisposeTransaction();

            var t = Task.Run(async () =>
            {
                _transaction = await Database.BeginTransactionAsync(CancellationToken);
                var tyAudit = new TransactionAudit()
                {
                    Id = Identifier.Uid, Author = Author, UtcDateTime = DateTime.UtcNow, TenantId = TenantId
                };
                var ty = await TransactionAudits.AddAsync(tyAudit, CancellationToken);
                _transactionAudit = ty.Entity;

            });

            t.Wait(CancellationToken);

           

            
            UowLogger?.LogDebug($"Initialized Uow with {nameof(IIdentifier.Uid)} '{Identifier.Uid}'");
            
        }

        /// <summary>Disposes the transaction.</summary>
        public void DisposeTransaction()
        {
            _transaction?.Dispose();
        }

        /// <summary>
        /// Perform Transaction Commit on a Db.
        /// On Error automatically perform a <see cref="IUnitOfWork.Rollback"/>
        /// </summary>
        public void Commit()
        {
            Commit("");
        }

        /// <summary>
        /// Perform Transaction Commit on a Db and write a custom log message related to this commit.
        /// On Error automatically perform a <see cref="IUnitOfWork.Rollback"/>
        /// </summary>
        /// <param name="logMessage"></param>
        public void Commit([CanBeNull] string logMessage)
        {
            var transactionCommitMessage = "";
            if (!string.IsNullOrEmpty(logMessage) && !string.IsNullOrWhiteSpace(logMessage))
            {
                UowLogger?.LogInformation($"Commit - {logMessage}");

                transactionCommitMessage = logMessage.Trim();
                if (transactionCommitMessage.Length > 500)
                {
                    transactionCommitMessage = transactionCommitMessage.Substring(0, 499);
                }
            }

            if (Changecount == 0)
            {
                UowLogger?.LogTrace("No changes to commit");
                Committed?.Invoke(this, new UnitOfWorkEventArg(Identifier));
                return;
            }

            var d = DateTime.UtcNow - _transactionAudit.UtcDateTime;
            _transactionAudit.MillisecDuration = d.TotalMilliseconds;
            _transactionAudit.TenantId = TenantId;


            if (transactionCommitMessage != "")
            {
                _transactionAudit.CommitMessage = transactionCommitMessage;
            }

            if (ScopeDictionary.Count > 0)
            {
                var s = string.Join(" => "
                                    , ScopeDictionary.OrderBy(x => x.Key).Select(x => x.Value).ToArray());
                if (s.Length > 500)
                {
                    s = $"{s.Substring(0, 497)}...";
                }

                _transactionAudit.Scopes = s;
            }

           

            var t = Task.Run(async () =>
            {
                var progr = await TransactionAudits.MaxAsync(x => x.Progr, CancellationToken);
                _transactionAudit.Progr = 1 + progr;

                TransactionAudits.Update(_transactionAudit);
                var t2 = await SaveChangesAsync(CancellationToken);

            });

            t.Wait(CancellationToken);



            UowLogger?.LogDebug($"Commit Transaction '{Identifier.Uid}'");
            try
            {
                _transaction.Commit();
                Committed?.Invoke(this, new UnitOfWorkEventArg(Identifier));
            }
            catch (Exception e)
            {
                UowLogger?.LogCritical(e, $"Error committing Transaction '{Identifier.Uid}': {e.Message}");
                Rollback();
                UowLogger?.LogInformation("Re-throw exception");
                throw;
            }
        }

        /// <summary>
        /// Rollback changes on Db Transaction.
        ///
        /// <exception cref="Exception">On rollback error re-trow exception</exception>
        /// </summary>
        public void Rollback()
        {
            try
            {
                _transaction.Rollback();
                UowLogger?.LogTrace("Rollback");
            }
            catch (Exception e)
            {
                UowLogger?.LogCritical($"Error on Rollback: {e.Message}", e);
                throw;
            }
        }

        /// <summary>
        /// Fired On Committed Unit Of Work
        /// </summary>
        public event EventHandler<UnitOfWorkEventArg> Committed;

        /// <summary>Begins the scope.</summary>
        /// <param name="scopeName">Name of the scope.</param>
        /// <returns></returns>
        [NotNull]
        public IDisposable BeginScope([NotNull] string scopeName)
        {
            _scopeCount++;
            ScopeDictionary.Add(_scopeCount, scopeName);
            return NamedScope.Instance(scopeName, Logger);
        }

        #endregion

        /// <summary>Gets or sets the uow logger.</summary>
        /// <value>The uow logger.</value>
        public ILogger UowLogger { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IdentityBaseContext{TUser, TRole, TKey}"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
        public bool Initialized { get; protected set; }

        /// <summary>Initializes this instance.</summary>
        /// <returns>IDbContextUnitOfWork instance initialized</returns>
        [NotNull]
        IDbContextUnitOfWork IDbContextUnitOfWork.Initialize()
        {
            return InitializeSelf();
        }

        /// <summary>
        /// Init Method
        /// </summary>
        /// <returns>Instance of initialized Service</returns>
        [NotNull]
        public IdentityBaseContext<TUser, TRole, TKey> Initialize()
        {
            return InitializeSelf();
        }

        [NotNull]
        private IdentityBaseContext<TUser, TRole, TKey> InitializeSelf()
        {
            if (!Initialized)
            {
                Initialized = true;
                BeginTransaction();

            }

            
            return this;
        }
    }


    /// <summary>
    ///  Core Identitity Context
    /// </summary>
    /// <typeparam name="TUser">Type of User Entity class</typeparam>
    /// <typeparam name="TRole">Type of Role Entity class</typeparam>
    public abstract class
        IdentityBaseContext<TUser, TRole> : IdentityBaseContext<TUser, TRole, string>, ITenantContext , IDbContextUnitOfWork
        where TUser : UserEntity, IEntity<string>
        where TRole : RoleEntity, IEntity<string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        protected IdentityBaseContext([NotNull] DbContextOptions options)
            : base(options)
        {
        }
    }
}