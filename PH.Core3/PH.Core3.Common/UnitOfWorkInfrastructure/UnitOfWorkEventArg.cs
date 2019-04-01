using System;

namespace PH.Core3.Common.UnitOfWorkInfrastructure
{
    /// <summary>
    /// Event Argument fired on Commit a <see cref="IUnitOfWork"/>
    /// </summary>
    public class UnitOfWorkEventArg : Core3.Common.CoreSystem.CoreEventArgs 
    {

        /// <summary>
        /// Initialize a new instance of UnitOfWorkEventArg class. 
        /// </summary>
        /// <param name="identifier"></param>
        public UnitOfWorkEventArg(IIdentifier identifier) : base(identifier)
        {
        }
    }
}