using System.ComponentModel.DataAnnotations;

namespace PH.Core3.Common
{
    /// <summary>
    /// Abstraction of Unique Identifier across Scope With Tenant Id
    /// </summary>
    public interface IPerTenantIdentifier
    {
        /// <summary>
        /// Tenant Identifier
        /// </summary>
        [StringLength(128)]
        string TenantId { get; }
    }
}