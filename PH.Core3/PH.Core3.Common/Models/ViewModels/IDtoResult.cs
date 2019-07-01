using System;

namespace PH.Core3.Common.Models.ViewModels
{
    /// <summary>
    /// Interface for DTO result
    ///
    /// <seealso cref="IEditDto{TKey}"/>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IDtoResult<TKey> : IEditDto<TKey>
        where TKey : IEquatable<TKey>
    {


        /// <summary>Gets the UTC last updated date and time for current entity.</summary>
        /// <value>The UTC last updated.</value>
        DateTime? UtcLastUpdated { get; }
    }
}