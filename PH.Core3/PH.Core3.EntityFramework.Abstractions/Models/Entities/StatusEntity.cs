using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace PH.Core3.EntityFramework.Abstractions.Models.Entities
{
    /// <summary>
    /// Entity Base auto-filtered based on UtcValidFrom and UtcValidUntil
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <seealso cref="Entity{TKey}" />
    /// <seealso cref="IStatusEntity{TKey}" />
    /// <seealso cref="IStatusEntity" />
    public abstract class StatusEntity<TKey> : Entity<TKey>, IStatusEntity<TKey>, IStatusEntity
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the UTC valid from.  
        /// </summary>
        /// <value>
        /// The UTC valid from.
        /// </value>
        [Required]
        public DateTime UtcValidFrom { get; set; }

        /// <summary>
        /// Gets or sets the UTC valid until.
        /// </summary>
        /// <value>
        /// The UTC valid until.
        /// </value>
        public DateTime? UtcValidUntil { get; set; }

        /// <summary>
        /// Gets the validity of current entity. Lazy evaluated
        /// </summary>
        /// <value>
        /// <c>true</c> if valid; otherwise, <c>false</c>.
        /// </value>
        [NotNull]
        [NotMapped]
        public Lazy<bool> Valid => GetValidity();

        /// <summary>
        /// Gets the validity.
        /// </summary>
        /// <returns>        
        /// <c>true</c> if valid; otherwise, <c>false</c>.
        /// </returns>
        [NotNull]
        protected virtual Lazy<bool> GetValidity()
        {
            var r = new Lazy<bool>(() => UtcValidFrom <= DateTime.UtcNow && (UtcValidUntil.HasValue == false || UtcValidUntil.Value >= DateTime.UtcNow));
            return r;
        }
    }
}