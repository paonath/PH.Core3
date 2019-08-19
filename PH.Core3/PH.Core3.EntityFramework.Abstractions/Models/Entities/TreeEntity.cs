using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PH.Core3.EntityFramework.Abstractions.Models.Entities
{
    /// <summary>
    /// Entity abstraction for Tree Items
    /// </summary>
    /// <typeparam name="TKey">struct type of Id Property</typeparam>
    /// <typeparam name="TEntity">Entity vtype</typeparam>
    public abstract class TreeEntity<TEntity, TKey> : Entity<TKey> , ITreeEntity<TEntity, TKey>
        where TEntity : ITreeEntity<TKey> , IEntity<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Represents the depth on the tree (0 for Root Item)
        /// </summary>
        [Required]
        public int EntityLevel { get; set; }

        /// <summary>
        /// Reference Id for Parent
        ///
        /// Null if current item is a root <see cref="EntityLevel"/>
        /// </summary>
        public TKey? ParentId { get; set; }

        /// <summary>
        /// Reference Id For Root Item
        ///
        /// Equals to Id if current item is Root
        /// </summary>
        public TKey RootId { get; set; }

        /// <summary>
        /// Reference to the Parent Entity
        ///
        /// Null if current item is a root <see cref="EntityLevel"/>
        /// </summary>
        public TEntity Parent { get; set; }

        /// <summary>
        /// Collection of children items
        /// </summary>
        public ICollection<TEntity> Childrens { get; set; }

        /// <summary>
        /// Reference to the Root Item Entity
        ///
        /// Null if current item is a root <see cref="EntityLevel"/>
        /// </summary>
        [ForeignKey("RootId")]
        public TEntity RootEntity { get; set; }
    }
}