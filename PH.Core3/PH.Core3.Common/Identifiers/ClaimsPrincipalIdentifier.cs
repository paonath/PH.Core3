using System.Security.Claims;
using JetBrains.Annotations;

namespace PH.Core3.Common.Identifiers
{
    public class ClaimsPrincipalIdentifier : Identifier, IIdentifier
    {
        /// <summary>
        /// Identity Name
        /// </summary>
        public string Name => Principal?.Identity?.Name;

        /// <summary>
        /// ClaimsPrincipal 
        /// </summary>
        public ClaimsPrincipal Principal { get; }


        public ClaimsPrincipalIdentifier([NotNull] string uid, [CanBeNull] ClaimsPrincipal principal) 
            : base(uid)
        {
            Principal = principal;
        }
    }
}