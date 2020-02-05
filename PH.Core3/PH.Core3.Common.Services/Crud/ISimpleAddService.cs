using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PH.Results;
using PH.Results.Internals;

namespace PH.Core3.Common.Services.Crud
{
    /// <summary>
    /// Simple Add Service
    /// </summary>
    /// <typeparam name="T">The Entity Type</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface ISimpleAddService<T, in TKey> : ISimpleReadService<T, TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Async Add new Item
        /// </summary>
        /// <param name="entity">Item to Add</param>
        /// <returns><see cref="Result{TContent}"/> containing added Item or error</returns>
        Task<IResult<T>> EntityAddAsync([NotNull] T entity);
    }
}