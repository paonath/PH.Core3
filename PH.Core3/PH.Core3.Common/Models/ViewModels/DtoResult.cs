using System;

namespace PH.Core3.Common.Models.ViewModels
{
    /// <summary>
    /// abstract DTO result
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <seealso cref="PH.Core3.Common.Models.ViewModels.IDtoResult{TKey}" />
    public abstract class DtoResult<TKey> : IDtoResult<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Unique Id of current class
        /// </summary>
        public TKey Id { get; set; }


        /// <summary>Gets the UTC last updated date and time for current entity.</summary>
        /// <value>The UTC last updated.</value>
        public DateTime? UtcLastUpdated { get; protected set; }
    }
}