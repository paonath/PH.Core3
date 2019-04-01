using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PH.Core3.TestContext.Internals
{
    internal class ContextContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        #region Implementation of IDesignTimeDbContextFactory<out PerTenantContext>

        public MyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseMySql("server=localhost;database=ctx_core3;user=dev;password=dev;SslMode=none");

            return new MyContext(optionsBuilder.Options);

        }

        #endregion
    }
}