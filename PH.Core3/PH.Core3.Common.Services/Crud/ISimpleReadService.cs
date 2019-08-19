using System;
using System.Threading.Tasks;
using PH.Core3.Common.Result;

namespace PH.Core3.Common.Services.Crud
{
    /// <summary>
    /// Simple Read Service
    /// </summary>
    /// <typeparam name="T">The Entity Type</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface ISimpleReadService<T, in TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Find  by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns><see cref="PH.Core3.Common.Result"/> instance</returns>
        Task<IResult<T>> EntityFindByIdAsync(TKey id);
        

        /// <summary>
        /// Load All Items
        /// </summary>
        /// <returns><see cref="Result{T}"/> instance</returns>
        Task<IResult<T[]>> EntityLoadAllAsync();

        ///// <summary>
        ///// Load All Items
        ///// </summary>
        ///// <returns><see cref="Result{T}"/> instance</returns>
        //Task<IPagedResult<T>> EntityPagedLoadAllAsync(int pageNumber = 0);

        

        ///// <summary>
        ///// Load Items as Paged Result
        ///// </summary>
        ///// <param name="skipItems">number of items to skip</param>
        ///// <param name="itemsToLoad">number of items to load</param>
        ///// <returns><see cref="PagedResult{T}"/> instance</returns>
        //Task<IPagedResult<T>> EntityLoadAsync(int skipItems, int itemsToLoad);

    }
}