using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PH.Core3.TestContext.Map;
using PH.UowEntityFramework.EntityFramework;
using PH.UowEntityFramework.EntityFramework.Abstractions.Models;

namespace PH.Core3.TestContext
{

    public class TestData : Entity<Guid>
    {
        public string Name { get; set; }
    }


    public class MyContext : IdentityBaseContext<User,Role,string> 
    {
        //public DbSet<Albero> Alberi { get; set; }
        //public DbSet<Category> Categories { get; set; }

        public DbSet<TestData> Data { get; set; }

        /// <inheritdoc />
        public MyContext([NotNull] DbContextOptions options) 
            : base(options)
        {
            //CurrentTenantSelectedName = "MyTenant";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public override void OnCustomModelCreating(ModelBuilder builder)
        {
            //builder.ForSqlServerUseSequenceHiLo("PaoloTestHiLo");
            


            builder.ApplyConfiguration(new UserMap());
            builder.ApplyConfiguration(new RoleMap());
            builder.ApplyConfiguration(new TestDataMap());
            //builder.ApplyConfiguration(new AlberoMap());
            //builder.ApplyConfiguration(new CategoryMap());

        }

        [NotNull]
        protected override Type[] ScanAssemblyTypes()
        {
            var a = GetType().Assembly.GetTypes();
            return a;
        }
    }
}