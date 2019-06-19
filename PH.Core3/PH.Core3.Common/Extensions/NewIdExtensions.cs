using MassTransit;

namespace PH.Core3.Common.Extensions
{
    /// <summary>
    /// Other methods for <see cref="NewId"/>
    /// </summary>
    public static class NewIdExtensions
    {
        /// <summary>Get the Next string identifier.</summary>
        /// <returns></returns>
        public static string NextStringId()
        {
            return NewId.Next().ToString("N").ToUpperInvariant();
        }

    }
}