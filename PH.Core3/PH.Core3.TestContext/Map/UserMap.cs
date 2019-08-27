using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PH.UowEntityFramework.EntityFramework.Mapping;

namespace PH.Core3.TestContext.Map
{

    internal class TestDataMap : EntityMap<TestData, Guid>
    {


    }

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

}