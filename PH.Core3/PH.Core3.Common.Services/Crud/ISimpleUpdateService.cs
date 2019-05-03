using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PH.Core3.Common.Result;

namespace PH.Core3.Common.Services.Crud
{
    /// <summary>
    /// Simple Update Service
    /// </summary>
    /// <typeparam name="T">The Entity Type</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface ISimpleUpdateService<T, in TKey> : ISimpleReadService<T, TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Async update a entity
        /// </summary>
        /// <param name="entity">Content to Update</param>
        /// <returns><see cref="PH.Core3.Common.Result"/> result</returns>
        Task<IResult<T>> EntityUpdateAsync([NotNull] T entity);

    }
}