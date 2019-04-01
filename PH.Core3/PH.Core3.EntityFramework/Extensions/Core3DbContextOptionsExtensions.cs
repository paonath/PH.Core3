using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace PH.Core3.EntityFramework
{
    public static class Core3DbContextOptionsExtensions
    {

        public static DbContextOptionsBuilder ConfigureTenant([NotNull] this DbContextOptionsBuilder optionsBuilder,
                                                              [NotNull] string tenant)
        {

            //var extension = GetOrCreateExtension(optionsBuilder).WithConnectionString(connectionString);
            //((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);


            throw new NotImplementedException("to do");
        }

        public static DbContextOptionsBuilder<TContext> ConfigureTenant<TContext>(
            [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder, [NotNull] string tenant)
            where TContext : DbContext
        {
            throw new NotImplementedException("to do");
        }

    }
}