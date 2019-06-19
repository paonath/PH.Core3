using System;
using System.Security.Cryptography;
using System.Text;
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
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            return false;
        }

        /// <summary>Gets the MD5 of current string.</summary>
        /// <param name="stringvalue">The string value.</param>
        /// <returns>MD5 value of given string</returns>
        /// <exception cref="ArgumentNullException">Thrown if given string is null or empty or white-space</exception>
        [NotNull]
        public static string GetMd5([NotNull] this string stringvalue)
        {
            if (IsNullString(stringvalue))
            {
                throw new ArgumentNullException(nameof(stringvalue));
            }
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(stringvalue);
                byte[] hashBytes  = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb2 = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb2.Append(hashBytes[i].ToString("X2"));
                }

                return sb2.ToString();
            }
        }
    }
}