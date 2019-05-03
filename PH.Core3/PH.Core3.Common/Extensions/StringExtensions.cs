using System;
using JetBrains.Annotations;

namespace PH.Core3.Common.Extensions
{
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Combine string.IsNullOrEmpty(value) and string.IsNullOrWhiteSpace(value)
        /// </summary>
        /// <param name="value">string to check</param>
        /// <returns>True if Null or Empty or WhiteSpace</returns>
        public static bool IsNullString([CanBeNull] string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;
            if (string.IsNullOrWhiteSpace(value))
                return true;

            return false;
        }
    }
}