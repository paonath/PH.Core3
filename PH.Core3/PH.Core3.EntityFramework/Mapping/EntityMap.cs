﻿using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PH.Core3.Common.Models.Entities;

namespace PH.Core3.EntityFramework.Mapping
{
    /// <summary>
    /// Allows configuration for an entity type to be factored into a separate class
    /// </summary>
    /// <seealso cref="PH.Core3.EntityFramework.Mapping.IEntityMap" />
    public abstract class EntityMap : IEntityMap
    {
        /// <summary>Gets the type of the entity.</summary>
        /// <returns></returns>
        public abstract Type GetEntityType();
        
    }

    /// <summary>
    /// Allows configuration for an entity type to be factored into a separate class
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <seealso cref="PH.Core3.EntityFramework.Mapping.IEntityMap" />
    public abstract class EntityMap<TEntity, TKey> : EntityMap, IEntityMap<TEntity, TKey>, IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        /// <summary>Gets the type of the entity.</summary>
        /// <value>The type of the entity.</value>
        public Type EntityType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMap{TEntity, TKey}"/> class.
        /// </summary>
        protected EntityMap()
        {
            EntityType = Activator.CreateInstance(typeof(TEntity)).GetType();
        }


        /// <summary>Gets the type of the entity.</summary>
        /// <returns></returns>
        public override Type GetEntityType()
        {
            return EntityType;
        }

        /// <summary>
        ///     Configures the entity of type <typeparamref name="TEntity" />.
        ///
        /// Override Configure Method calling base!
        /// </summary>
        /// <param name="builder"> The builder to be used to configure the entity type. </param>
        public virtual void Configure([NotNull] EntityTypeBuilder<TEntity> builder)
        {
            
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }



            builder.Property(x => x.Id)
                   .IsRequired(true);

            builder.Property(x => x.TenantId)
                   .IsRequired(true);
            
            builder.Property(x => x.Deleted)
                   .HasColumnName("Deleted")
                   .IsRequired(true);
            
            builder.Property(x => x.DeletedTransactionId)
                   .HasColumnName("DeletedTransactionId");
            
           

            builder.Property(x => x.CreatedTransactionId)
                   .HasColumnName("CreatedTransactionId");




            builder.Property(x => x.UpdatedTransactionId)
                   .HasColumnName("UpdatedTransactionId");


            builder.Property(x => x.Timestamp)
                   .HasColumnName("Timestamp")
                   .IsConcurrencyToken(true)
                   .IsRowVersion();

            builder
                .HasIndex(i => new
                {
                    i.Id,
                    i.Deleted,
                    i.CreatedTransactionId
                }).IsUnique(false);
            
            builder
                .HasIndex(i => i.UpdatedTransactionId)
                .IsUnique(false);
            builder
                .HasIndex(i => i.DeletedTransactionId)
                .IsUnique(false);

        }
    }


    /// <summary>
    ///     Configures the entity of type <typeparamref name="TEntity" />.
    ///
    /// Override Configure Method calling base!
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <seealso cref="PH.Core3.EntityFramework.Mapping.EntityMap{TEntity, TKey}" />
    public abstract class StatusEntityMap<TEntity, TKey> : EntityMap<TEntity, TKey>
        where TEntity : class, IStatusEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        ///     Configures the entity of type <typeparamref name="TEntity" />.
        ///
        /// Override Configure Method calling base!
        /// </summary>
        /// <param name="builder"> The builder to be used to configure the entity type. </param>
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            base.Configure(builder);

            builder.Property(x => x.UtcValidFrom)
                   .HasColumnName("UtcValidFrom")
                   .IsRequired(true);


            builder
                .HasIndex(i => new
                {
                    i.Id,
                    i.UtcValidFrom,
                    i.UtcValidUntil
                }).IsUnique(true);

        }
    }
}