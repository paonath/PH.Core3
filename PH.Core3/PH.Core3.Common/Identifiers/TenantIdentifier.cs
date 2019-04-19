using JetBrains.Annotations;

namespace PH.Core3.Common.Identifiers
{
    public class TenantIdentifier : Identifier, IPerTenantIdentifier
    {
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