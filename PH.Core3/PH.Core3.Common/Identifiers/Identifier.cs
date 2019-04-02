using System.Security.Claims;
using JetBrains.Annotations;

namespace PH.Core3.Common.Identifiers
{
    public class Identifier : IIdentifier
    {
        public Identifier([NotNull] string uid)
        {
            Uid = uid;
        }

        /// <summary>
        /// Unique Identifier
        /// </summary>
        public string Uid { get; }
    }

    public class ClaimsPrincipalIdentifier : Identifier, IIdentifier
    {
        public string Name { get; }
        public ClaimsPrincipal Principal { get; }


        public ClaimsPrincipalIdentifier([NotNull] string uid, [CanBeNull] ClaimsPrincipal principal) 
            : base(uid)
        {
            Principal = principal;
            Name = principal?.Identity?.Name;
        }
    }
}