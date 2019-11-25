using System.ComponentModel.DataAnnotations;

namespace PH.Core3.Common
{
    /// <summary>
    /// Abstraction of Unique Identifier across Scope
    /// </summary>
    public interface IIdentifier 
    {
        /// <summary>
        /// Unique Identifier
        /// </summary>
        [StringLength(128)]
        string Uid { get; }
    }
}
