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
        /// Find <see cref="TDto"/> by Id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns><see cref="PH.Core3.Common.Result"/> instance</returns>
        Task<IResult<TDto>> FindByIdAsync(TKey id);
        
        /// <summary>
        /// Load All <see cref="TDto"/> Items
        /// </summary>
        /// <returns><see cref="Result{TDto}"/> instance</returns>
        Task<IResult<TDto[]>> LoadAllAsync();
    }
}