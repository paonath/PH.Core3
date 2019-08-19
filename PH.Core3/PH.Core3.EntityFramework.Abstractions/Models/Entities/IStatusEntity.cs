using System;
using System.ComponentModel.DataAnnotations;

namespace PH.Core3.EntityFramework.Abstractions.Models.Entities
{
    /// <summary>
    /// Entity Base auto-filtered based on UtcValidFrom and UtcValidUntil
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <seealso cref="IEntity" />
    public interface IStatusEntity<TKey> : IEntity<TKey> , IStatusEntity
        where TKey : IEquatable<TKey>
    { }

    /// <summary>
    /// Entity Base auto-filtered based on <see cref="UtcValidFrom"/> and <see cref="UtcValidUntil"/>
    /// </summary>
    /// <seealso cref="IEntity" />
    public interface IStatusEntity : IEntity
    {

        /// <summary>
        /// Gets or sets the UTC valid from.  
        /// </summary>
        /// <value>
        /// The UTC valid from.
        /// </value>
        [Required]
        DateTime UtcValidFrom { get; set; }

        /// <summary>
        /// Gets or sets the UTC valid until.
        /// </summary>
        /// <value>
        /// The UTC valid until.
        /// </value>
        DateTime? UtcValidUntil { get; set; }

        /// <summary>
        /// Gets the validity of current entity. Lazy evaluated
        /// </summary>
        /// <value>
        ///   <c>true</c> if valid; otherwise, <c>false</c>.
        /// </value>
        Lazy<bool> Valid { get;  }
    }
}