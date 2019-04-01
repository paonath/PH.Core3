using System;
using PH.Core3.Common;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.UnitOfWorkInfrastructure;

namespace PH.Core3.UnitOfWork
{
    public interface IUnitOfWork : ICoreDisposable 
    {
        IIdentifier Identifier { get; }

        /// <summary>
        /// Perform Transaction Commit on a Db.
        /// On Error automatically perform a <see cref="Rollback"/>
        /// </summary>
        void Commit();

        /// <summary>
        /// Perform Transaction Commit on a Db and write a custom log message related to this commit.
        /// On Error automatically perform a <see cref="Rollback"/>
        /// </summary>
        /// <param name="logMessage"></param>
        void Commit(string logMessage);

        /// <summary>
        /// Rollback changes on Db Transaction.
        ///
        /// <exception cref="Exception">On rollback error re-trow exception</exception>
        /// </summary>
        void Rollback();

        /// <summary>
        /// Fired On Committed Unit Of Work
        /// </summary>
        event EventHandler<UnitOfWorkEventArg> Committed;

        IDisposable BeginScope(string scopeName);
    }



}
