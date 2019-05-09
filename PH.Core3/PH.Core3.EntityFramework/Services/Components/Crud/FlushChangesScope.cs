using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PH.Core3.Common;
using PH.Core3.Common.CoreSystem;

namespace PH.Core3.EntityFramework.Services.Components.Crud
{
    /// <summary>
    /// Flush Changes Scope
    /// On Disposing scope perform a save changes on db.
    /// </summary>
    /// <seealso cref="PH.Core3.Common.CoreSystem.CoreDisposable" />
    public class FlushChangesScope : CoreDisposable
    {
        private readonly DbContext _dbContext;
        /// <summary>
        /// Initialize a new instance of <see cref="CoreDisposable"/>
        /// </summary>
        private FlushChangesScope(IIdentifier identifier, DbContext dbContext) : base(identifier)
        {
            _dbContext = dbContext;
        }

        /// <summary>Begins the scope.</summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="dbContext">The database context.</param>
        /// <returns></returns>
        [NotNull]
        public static FlushChangesScope BeginScope(IIdentifier identifier, DbContext dbContext)
        {
            return new FlushChangesScope(identifier,dbContext);
        }

        /// <summary>
        /// Dispose Pattern.
        /// This method check if already <see cref="CoreDisposable.Disposed"/> (and set it to True).
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected override void Dispose(bool disposing)
        {
            var t = _dbContext.SaveChangesAsync();
            t.Wait();

            //
        }
    }
}