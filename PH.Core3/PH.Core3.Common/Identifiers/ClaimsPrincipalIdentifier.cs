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

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return ($"{typeof(ClaimsPrincipalIdentifier)} {ToString()}").GetHashCode();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Uid: '{Uid}' - Name '{Name}' - Generated on '{UtcGenerated:O}' - Guid '{BaseIdentifierGuid}'";
        }
    }
}