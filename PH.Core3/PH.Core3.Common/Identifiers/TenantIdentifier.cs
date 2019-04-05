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
    }
}