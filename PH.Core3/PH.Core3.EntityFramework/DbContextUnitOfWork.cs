using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Models.Entities;
using PH.Core3.Common.UnitOfWorkInfrastructure;
using PH.Core3.UnitOfWork;

namespace PH.Core3.EntityFramework
{
   

    //public class DbContextUnitOfWork<TContext> : CoreDisposable , IUnitOfWork
    //where TContext : DbContext , IDisposable 
    //{
    //    private TContext _dbContext;

    //    [NotNull] private readonly IIdentifier _identifier;
    //    [NotNull] private readonly string _tenant;
    //    [NotNull] private readonly string _author;
    //    private readonly IsolationLevel _isolationLevel;
    //    private readonly ILogger<DbContextUnitOfWork<TContext>> _logger;

    //    private IDbContextTransaction _transaction;


    //    protected CancellationTokenSource CancellationTokenSource { get; set; }
    //    public CancellationToken CancellationToken { get; protected set; }


    //    /// <summary>
    //    /// Initialize a new instance of <see cref="CoreDisposable"/>
    //    /// </summary>
    //    public DbContextUnitOfWork([NotNull] IIdentifier identifier, [NotNull] TContext dbContext, [NotNull] string tenant
    //                               , [NotNull] string author, IsolationLevel level, ILogger<DbContextUnitOfWork<TContext>> logger) : base(identifier)
    //    {
    //        if (string.IsNullOrEmpty(tenant))
    //            throw new ArgumentException("Value cannot be null or empty.", nameof(tenant));
    //        if (string.IsNullOrWhiteSpace(tenant))
    //            throw new ArgumentException("Value cannot be null or whitespace.", nameof(tenant));
    //        if (string.IsNullOrEmpty(author))
    //            throw new ArgumentException("Value cannot be null or empty.", nameof(author));
    //        if (string.IsNullOrWhiteSpace(author))
    //            throw new ArgumentException("Value cannot be null or whitespace.", nameof(author));

    //        _identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
    //        _tenant = tenant;
    //        _author = author;
    //        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    //        _dbContext.TenantId = tenant;
    //        _isolationLevel = level;
    //        _logger = logger;
    //    }

    //    /// <summary>
    //    /// Dispose Pattern.
    //    /// This method check if already <see cref="CoreDisposable.Disposed"/> (and set it to True).
    //    /// </summary>
    //    /// <param name="disposing">True if disposing</param>
    //    protected override void Dispose(bool disposing)
    //    {
    //        if (disposing)
    //        {
    //            _dbContext?.Dispose();
    //        }
    //    }

    //    public DbContextUnitOfWork<TContext> Initialize()
    //    {
    //        CancellationTokenSource = new CancellationTokenSource();
    //        CancellationToken       = CancellationTokenSource.Token;

    //        var t = Task.Run(async () =>
    //        {
    //            _transaction = await _dbContext.Database.BeginTransactionAsync(CancellationToken);

    //            var dbContext = (IAuditContext) _dbContext;

    //            await dbContext.TransactionAudits.AddAsync(new TransactionAudit()
    //            {
    //                Id = _identifier.Uid, Author = _author, UtcDateTime = DateTime.UtcNow, TenantId = _tenant
    //            }, CancellationToken);

    //        });

    //        t.Wait(CancellationToken);

           

    //        _logger.LogDebug($"Initialized Uow with {nameof(IIdentifier.Uid)} '{_identifier.Uid}'");

    //        return this;
    //    }

    //    /// <summary>
    //    /// Perform Transaction Commit on a Db.
    //    /// On Error automatically perform a <see cref="IUnitOfWork.Rollback"/>
    //    /// </summary>
    //    public void Commit()
    //    {
    //        Commit("");
    //    }

    //    /// <summary>
    //    /// Perform Transaction Commit on a Db and write a custom log message related to this commit.
    //    /// On Error automatically perform a <see cref="IUnitOfWork.Rollback"/>
    //    /// </summary>
    //    /// <param name="logMessage"></param>
    //    public void Commit(string logMessage)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Rollback changes on Db Transaction.
    //    ///
    //    /// <exception cref="Exception">On rollback error re-trow exception</exception>
    //    /// </summary>
    //    public void Rollback()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Fired On Committed Unit Of Work
    //    /// </summary>
    //    public event EventHandler<UnitOfWorkEventArg> Committed;
    //}
}