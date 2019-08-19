using System;
using System.Threading.Tasks;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Models.ViewModels;
using PH.Core3.Common.Result;

namespace PH.Core3.Common.Services.Crud
{
    /// <summary>
    /// Service For Read-Only operation.
    ///
    /// 
    /// </summary>
    /// <typeparam name="TDto">Type of the Content to Read</typeparam>
    /// <typeparam name="TKey">Type of the Content Id Property</typeparam>
    public interface IReadOnlyService<TDto, in TKey> : IService
        where TDto : IDtoResult<TKey>, IDto<TKey>, IIdentifiable<TKey> 
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Find  by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns><see cref="PH.Core3.Common.Result"/> instance</returns>
        Task<IResult<TDto>> FindByIdAsync(TKey id);
        
        /// <summary>
        /// Load All Items
        /// </summary>
        /// <returns><see cref="Result{TDto}"/> instance</returns>
        Task<IResult<TDto[]>> LoadAllAsync();

        ///// <summary>
        ///// Load Items as Paged Result using default pagination setting
        ///// </summary>
        ///// <param name="pageNumber">page number</param>
        ///// <returns><see cref="PagedResult{TContent}"/> instance</returns>
        //Task<IPagedResult<TDto>> LoadAsync(int pageNumber);

        ///// <summary>
        ///// Load Items as Paged Result
        ///// </summary>
        ///// <param name="skipItems">number of items to skip</param>
        ///// <param name="itemsToLoad">number of items to load</param>
        ///// <returns><see cref="PagedResult{TContent}"/> instance</returns>
        //Task<IPagedResult<TDto>> LoadAsync(int skipItems, int itemsToLoad);
    }
}