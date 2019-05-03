using System.Security.Claims;
using JetBrains.Annotations;

namespace PH.Core3.Common.Identifiers
{
    /// <summary>
    /// Unique Identifier with Tenant id across Scope based on ClaimsPrincipal identity 
    /// </summary>
    /// <seealso cref="PH.Core3.Common.Identifiers.ClaimsPrincipalIdentifier" />
    /// <seealso cref="PH.Core3.Common.IPerTenantIdentifier" />
    public class ClaimsPrincipalTenantIdentifier : ClaimsPrincipalIdentifier, IPerTenantIdentifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimsPrincipalTenantIdentifier"/> class.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="principal">The principal.</param>
        public ClaimsPrincipalTenantIdentifier([NotNull] string uid, [NotNull] string tenantId,[CanBeNull] ClaimsPrincipal principal) : base(uid, principal)
        {
            TenantId = tenantId;
        }

        /// <summary>
        /// Tenant Identifier
        /// </summary>
        public string TenantId { get; }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return ($"{typeof(ClaimsPrincipalTenantIdentifier)} {ToString()}").GetHashCode();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Uid: '{Uid}' - Tenant '{TenantId}' - Generated on '{UtcGenerated:O}' - Guid '{BaseIdentifierGuid}'";
        }
    }
}