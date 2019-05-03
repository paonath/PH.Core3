using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PH.Core3.Common.Models.Entities;
using PH.Core3.EntityFramework.Mapping;
using NewArrayExpression = Castle.DynamicProxy.Generators.Emitters.SimpleAST.NewArrayExpression;

namespace PH.Core3.TestContext.Map
{
    internal class UserMap :  EntityMap<User,string>
        //IEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            }

       
       
    }

    internal class RoleMap : EntityMap<Role,string> //IEntityTypeConfiguration<Role>
    {
       
        

    }


    internal class AlberoMap : TreeEntityMap<Albero, Guid>
    {
        public override void Configure(EntityTypeBuilder<Albero> builder)
        {
            base.Configure(builder);

            builder.HasQueryFilter(c => c.Description != "" && c.EntityLevel > -1 && c.Deleted != true );
            

            builder
                .HasOne(x => x.Category)
                .WithMany(x => x.Alberi)
                .HasForeignKey(x => x.CategoryId)
                .IsRequired(false);
        }
    }

    public class EnumValueConverter<TEnum> : ValueConverter<TEnum, EntityEnum>
        where TEnum : struct, System.Enum
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter`2" /> class.
        /// </summary>
        /// <param name="convertToProviderExpression"> An expression to convert objects when writing data to the store. </param>
        /// <param name="convertFromProviderExpression"> An expression to convert objects when reading data from the store. </param>
        /// <param name="mappingHints">
        ///     Hints that can be used by the <see cref="T:Microsoft.EntityFrameworkCore.Storage.ITypeMappingSource" /> to create data types with appropriate
        ///     facets for the converted data.
        /// </param>
        public EnumValueConverter(Func<int, TEnum> intToEnumFnc,
            [CanBeNull] ConverterMappingHints mappingHints = null) 
            : base(ConvertEnumToEntityEnum(), ConvertEntityEnumToEnum(intToEnumFnc), mappingHints)
        {
        }

        [NotNull]
        public static Expression<Func<TEnum, EntityEnum>> ConvertEnumToEntityEnum()
        {
            return ten => new EntityEnum() {Id = Convert.ToInt32(ten), Value = $"{ten}", Description = ""};
        }

        public static Expression<Func<EntityEnum, TEnum>> ConvertEntityEnumToEnum(Func<int, TEnum> intToEnumFnc)
        {


            return EntityEnum => intToEnumFnc.Invoke(EntityEnum.Id);
        }
    }

    internal class CategoryMap : TreeEntityMap<Category, Guid>
    {
        /// <summary>
        ///     Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder"> The builder to be used to configure the entity type. </param>
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);


            builder.Property(x => x.Colore).HasConversion(new EnumValueConverter<ColorEnum>(i => (ColorEnum) i));

        }
    }
}