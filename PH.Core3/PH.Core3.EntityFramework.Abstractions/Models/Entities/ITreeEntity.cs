using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace PH.Core3.EntityFramework.Abstractions.Models.Entities
{
    
    /// <summary>
    /// Represent one Item in a Tree
    /// </summary>
    public interface ITreeItem : IEntity
    {
        /// <summary>
        /// Represents the depth on the tree (0 for Root Item)
        /// </summary>
        int EntityLevel { get; set; }
    }

    /// <summary>
    /// Base Interface for <see cref="ITreeItem">tree items</see>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface ITreeEntity<TKey> : ITreeItem, IEntity<TKey>
        where TKey : struct, IEquatable<TKey> 
    {
        /// <summary>
        /// Reference Id for Parent
        ///
        /// Null if current item is a root <see cref="ITreeItem.EntityLevel"/>
        /// </summary>
        [CanBeNull] TKey? ParentId { get; set; } 

        /// <summary>
        /// Reference Id For Root Item
        ///
        /// Equals to Id if current item is root.
        /// </summary>
        [NotNull]TKey RootId { get; set; } 

    }

    /// <summary>
    /// Entity Interface for Tree Items
    /// </summary>
    /// <typeparam name="TEntity">Type of Entity</typeparam>
    /// <typeparam name="TKey">Type of Entity Id Property</typeparam>
    public interface ITreeEntity<TEntity, TKey> : ITreeEntity<TKey>
        where TKey : struct, IEquatable<TKey>
        where TEntity : ITreeEntity<TKey> , IEntity<TKey>
    {
        /// <summary>
        /// Reference to the Parent Entity
        ///
        /// Null if current item is a root <see cref="ITreeItem.EntityLevel"/>
        /// </summary>
        [CanBeNull]
        TEntity Parent { get; set; }

        /// <summary>
        /// Collection of children items
        /// </summary>
        [CanBeNull]
        ICollection<TEntity> Childrens { get; set; }

        /// <summary>
        /// Reference to the Root Item Entity
        ///
        /// Null if current item is a root <see cref="ITreeItem.EntityLevel"/>
        /// </summary>
        [CanBeNull] TEntity RootEntity { get; set; }
    }
}