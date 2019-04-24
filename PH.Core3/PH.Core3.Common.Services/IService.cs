using PH.Core3.Common.Identifiers.Services;

namespace PH.Core3.Common.Services
{
    /// <summary>
    /// Generic Service abstraction
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Per Scope Transient Identifier
        /// </summary>
        IIdentifier Identifier { get; }

        /// <summary>
        /// Service Identifier (a int value representing the service and the service name)
        /// </summary>
        ServiceIdentifier ServiceIdentifier { get; }
    }

    
}