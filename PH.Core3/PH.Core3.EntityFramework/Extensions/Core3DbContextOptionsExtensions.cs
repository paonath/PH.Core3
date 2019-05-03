using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace PH.Core3.EntityFramework.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class Core3DbContextOptionsExtensions
    {
        /// <summary>Configures the tenant.</summary>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="tenant">The tenant.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">to do</exception>
        public static DbContextOptionsBuilder ConfigureTenant([NotNull] this DbContextOptionsBuilder optionsBuilder,
                                                              [NotNull] string tenant)
        {

            //var extension = GetOrCreateExtension(optionsBuilder).WithConnectionString(connectionString);
            //((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);


            throw new NotImplementedException("to do");
        }

        /// <summary>Configures the tenant.</summary>
        /// <typeparam name="TContext">The type of the context.</typeparam>
        /// <param name="optionsBuilder">The options builder.</param>
        /// <param name="tenant">The tenant.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">to do</exception>
        public static DbContextOptionsBuilder<TContext> ConfigureTenant<TContext>(
            [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder, [NotNull] string tenant)
            where TContext : DbContext
        {
            throw new NotImplementedException("to do");
        }

    }
}