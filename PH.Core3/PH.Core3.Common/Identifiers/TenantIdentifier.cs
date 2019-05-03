using JetBrains.Annotations;

namespace PH.Core3.Common.Identifiers
{
    /// <summary>
    /// Tenant Identifier across scope
    /// </summary>
    /// <seealso cref="PH.Core3.Common.Identifiers.Identifier" />
    /// <seealso cref="PH.Core3.Common.IPerTenantIdentifier" />
    public class TenantIdentifier : Identifier, IPerTenantIdentifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantIdentifier"/> class.
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <param name="tenantId">The tenant identifier.</param>
        public TenantIdentifier([NotNull] string uid, [NotNull] string tenantId) 
            : base(uid)
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
            return ($"{typeof(TenantIdentifier)} {ToString()}").GetHashCode();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Uid: '{Uid}' - Tenant '{TenantId}' - Generated on '{UtcGenerated:O}' - Guid '{BaseIdentifierGuid}'";
        }
    }
}