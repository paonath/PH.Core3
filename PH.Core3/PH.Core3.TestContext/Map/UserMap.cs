using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PH.Core3.EntityFramework.Mapping;

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

    internal class CategoryMap : TreeEntityMap<Category, Guid>
    {
    }
}