using System;
using JetBrains.Annotations;
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
        [NotNull]
        public static string NextStringId()
        {
            return NewId.Next().ToString("N").ToUpperInvariant();
        }

        /// <summary>Nexts the sortable identifier long string as combine of NewId and current UTc DateTime.
        ///the output is a string length 55 chars eg. CB190000F45B98E7CD8A08D6FFB847C320190703T1313462598038Z
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public static string NextSortableIdLognString()
        {
            var s= $"{DateTime.UtcNow:O}"
                   .Replace("-", "")
                   .Replace(":", "")
                   .Replace(".", "")
                   .Replace(" ", "")
                   .Replace("+", "");
            return $"{s}{NextStringId()}";
        }

    }
}