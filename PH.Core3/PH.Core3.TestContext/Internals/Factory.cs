using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PH.Core3.TestContext.Internals
{
    internal class ContextContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        #region Implementation of IDesignTimeDbContextFactory<out PerTenantContext>

        [NotNull]
        public MyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
           //optionsBuilder.UseMySql("server=localhost;database=ctx_core3;user=dev;password=dev;SslMode=none");

           optionsBuilder.UseSqlServer("Server=192.168.3.83\\SQLEXPRESS;Database=ctx_core3;User Id=dev;Password=dev;MultipleActiveResultSets=true");

            return new MyContext(optionsBuilder.Options);

        }

        #endregion
    }
}