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
}