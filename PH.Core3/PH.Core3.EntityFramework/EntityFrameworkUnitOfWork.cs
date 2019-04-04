using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.UnitOfWorkInfrastructure;
using PH.Core3.UnitOfWork;

namespace PH.Core3.EntityFramework
{
    public interface IDbContextUnitOfWork : IUnitOfWork
    {
        ILogger UowLogger { get; set; }

        bool Initialized { get; }
        IDbContextUnitOfWork Initialize();
    }

    public sealed class  EntityFrameworkUnitOfWork : CoreDisposable , IUnitOfWork
    {
        private IDbContextUnitOfWork _dbUow;
        private ILogger _logger;

        /// <summary>
        /// Initialize a new instance of <see cref="CoreDisposable"/>
        /// </summary>
        public EntityFrameworkUnitOfWork([NotNull] IDbContextUnitOfWork efContextUnitOfWork, [NotNull] ILogger logger) 
            : base(efContextUnitOfWork.Identifier)
        {
            if (efContextUnitOfWork is null) 
                throw new ArgumentNullException(nameof(efContextUnitOfWork));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            efContextUnitOfWork.UowLogger = _logger;

            _dbUow = efContextUnitOfWork.Initialize();
            
            _dbUow.Committed += _dbUow_Committed;
        }

        private void _dbUow_Committed(object sender, UnitOfWorkEventArg e)
        {
           Committed?.Invoke(sender,e);
        }

        /// <summary>
        /// Dispose Pattern.
        /// This method check if already <see cref="CoreDisposable.Disposed"/> (and set it to True).
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected sealed override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbUow?.Dispose();
            }
        }

        public IIdentifier Identifier => _dbUow.Identifier;

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
        public void Commit(string logMessage)
        {
            _dbUow.Commit(logMessage);
        }

        /// <summary>
        /// Rollback changes on Db Transaction.
        ///
        /// <exception cref="Exception">On rollback error re-trow exception</exception>
        /// </summary>
        public void Rollback()
        {
            _dbUow.Rollback();
        }

        /// <summary>
        /// Fired On Committed Unit Of Work
        /// </summary>
        public event EventHandler<UnitOfWorkEventArg> Committed;
        public IDisposable BeginScope(string scopeName)
        {
            return _dbUow.BeginScope(scopeName);
        }
    }
}