using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace PH.Core3.Common.Extensions
{
    /// <summary>
    /// Enum exttensions methods
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the display name, if any, set by <see cref="DisplayAttribute"/>
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>Display Name or raw ToString()</returns>
        [NotNull]
        public static string GetDisplayName([NotNull] this Enum enumValue)
        {
            var raw = enumValue.ToString();
            var attr = enumValue.GetType()
                                .GetMember(raw)
                                .First()
                                .GetCustomAttribute<DisplayAttribute>();
            return attr?.GetName() ?? raw;
            //.GetName();
        }


        /// <summary>
        /// Gets the description, if any set by <see cref="DescriptionAttribute"/>, otherwise return DisplayName.
        ///
        /// <seealso cref="GetDisplayName"/>
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>Description or Name</returns>
        [NotNull]
        public static string GetDescription([NotNull] this Enum enumValue)
        {
            var raw = enumValue.ToString();
            var attr = enumValue.GetType()
                                .GetMember(raw)
                                .First()
                                .GetCustomAttribute<DescriptionAttribute>();
            if (null == attr)
            {
                return enumValue.GetDisplayName();
            }

            return attr.Description;

            //.GetName();
        }
    }
}