using System;
using PH.Core3.Common;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.UnitOfWorkInfrastructure;

namespace PH.Core3.UnitOfWork
{
    /// <summary>
    /// Unit Of Work
    /// </summary>
    /// <seealso cref="PH.Core3.Common.CoreSystem.ICoreDisposable" />
    public interface IUnitOfWork : ICoreDisposable 
    {
        /// <summary>Gets the identifier.</summary>
        /// <value>The identifier.</value>
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

        /// <summary>Begins the scope.</summary>
        /// <param name="scopeName">Name of the scope.</param>
        /// <returns></returns>
        IDisposable BeginScope(string scopeName);
    }



}
