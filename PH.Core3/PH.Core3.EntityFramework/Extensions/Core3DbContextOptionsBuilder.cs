using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PH.Core3.EntityFramework
{
    public class Core3DbContextOptionsBuilder : RelationalDbContextOptionsBuilder<Core3DbContextOptionsBuilder,
        Core3DbOptionsExtension>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Microsoft.EntityFrameworkCore.Infrastructure.RelationalDbContextOptionsBuilder`2" /> class.
        /// </summary>
        /// <param name="optionsBuilder"> The core options builder. </param>
        public Core3DbContextOptionsBuilder([NotNull] DbContextOptionsBuilder optionsBuilder) : base(optionsBuilder)
        {
            
        }


        public virtual Core3DbContextOptionsBuilder Tenant(string tenant)
            => WithOption(e => e.WithTenant(tenant));

    }
}