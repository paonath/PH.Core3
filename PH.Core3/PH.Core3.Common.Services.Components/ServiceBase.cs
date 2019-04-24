using System;
using PH.Core3.Common.Identifiers.Services;

namespace PH.Core3.Common.Services.Components
{

    /// <summary>
    /// Generic Service abstraction class
    /// </summary>
    public abstract class ServiceBase : IService
    {
        /// <summary>
        /// Init new Service Instance
        /// </summary>
        /// <param name="identifier">Per Scope Transient Identifier</param>
        protected ServiceBase(IIdentifier identifier)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Per Scope Transient Identifier
        /// </summary>
        public IIdentifier Identifier { get; }

        /// <summary>
        /// Service Identifier (a int value representing the service and the service name)
        /// </summary>
        public abstract ServiceIdentifier ServiceIdentifier { get; }
        
    }
}
