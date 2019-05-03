using System;
using Microsoft.EntityFrameworkCore;
using PH.Core3.Common.Models.Entities;

namespace PH.Core3.EntityFramework.Mapping
{
    /// <summary>
    /// Allows configuration for an entity type to be factored into a separate class
    /// </summary>
    public interface IEntityMap
    {
        /// <summary>Gets the type of the entity.</summary>
        /// <returns></returns>
        Type GetEntityType();

    }

    public interface IEntityEnumMap<TEnum> : IEntityTypeConfiguration<TEnum>, IEntityMap
        where TEnum : EntityEnum, IEntityEnum
    {

    }

    /// <summary>
    /// Allows configuration for an entity type to be factored into a separate class,
    /// rather than in-line in Microsoft.EntityFrameworkCore.DbContext.OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder).
    /// Implement this interface, applying configuration for the entity in the Microsoft.EntityFrameworkCore.IEntityTypeConfiguration`1.Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder{`0})
    /// method, and then apply the configuration to the model using Microsoft.EntityFrameworkCore.ModelBuilder.ApplyConfiguration``1(Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{``0})
    /// in Microsoft.EntityFrameworkCore.DbContext.OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder).
    ///
    /// <see cref="IEntityTypeConfiguration{TEntity}"/>
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be configured.</typeparam>
    /// <typeparam name="TKey">The entity Id type to be configured.</typeparam>
    public interface IEntityMap<TEntity, TKey> : IEntityTypeConfiguration<TEntity> , IEntityMap
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {

    }
}