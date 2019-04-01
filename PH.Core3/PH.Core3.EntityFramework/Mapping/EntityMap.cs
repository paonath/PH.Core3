using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PH.Core3.Common.Models.Entities;

namespace PH.Core3.EntityFramework.Mapping
{
    public abstract class EntityMap : IEntityMap
    {
        public abstract Type GetEntityType();
        
    }

     /// <inheritdoc cref="IEntityMap{TEntity,TKey}" />
    public abstract class EntityMap<TEntity, TKey> : EntityMap, IEntityMap<TEntity, TKey>, IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        public Type EntityType { get; }

        /// <inheritdoc />
        protected EntityMap()
        {
            EntityType = Activator.CreateInstance(typeof(TEntity)).GetType();
        }

       

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
                throw new ArgumentNullException(nameof(builder));

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

}