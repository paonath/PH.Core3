using System.Security.Claims;
using JetBrains.Annotations;

namespace PH.Core3.Common.Identifiers
{
    public class ClaimsPrincipalTenantIdentifier : ClaimsPrincipalIdentifier, IPerTenantIdentifier
    {
        public ClaimsPrincipalTenantIdentifier([NotNull] string uid, [NotNull] string tenantId,[CanBeNull] ClaimsPrincipal principal) : base(uid, principal)
        {
            TenantId = tenantId;
        }

        /// <summary>
        /// Tenant Identifier
        /// </summary>
        public string TenantId { get; }
    }
}