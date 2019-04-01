using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PH.Core3.TestContext.Map;

namespace PH.Core3.TestContext
{
    public class MyContext : PH.Core3.EntityFramework.IdentityBaseContext<User,Role,string>
    {
        public DbSet<Albero> Alberi { get; set; }
        public DbSet<Category> Categories { get; set; }

        

        /// <inheritdoc />
        public MyContext([NotNull] DbContextOptions options) 
            : base(options)
        {
            TenantId = "MyTenant";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public override void OnCustomModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMap());
            builder.ApplyConfiguration(new RoleMap());
            builder.ApplyConfiguration(new AlberoMap());
            builder.ApplyConfiguration(new CategoryMap());

        }
    }
}