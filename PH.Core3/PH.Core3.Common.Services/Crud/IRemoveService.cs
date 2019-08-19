using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Models.ViewModels;
using PH.Core3.Common.Result;

namespace PH.Core3.Common.Services.Crud
{
    /// <summary>
    /// Service for Deleting exsisting Items
    /// </summary>
    /// <typeparam name="TDto">Type of the Item to remove</typeparam>
    /// <typeparam name="TKey">Type of the Result Id Property</typeparam>
    public interface IRemoveService<TDto, in TKey> :
        IReadOnlyService<TDto, TKey>
        , ICrudAbstractionBase
        where TDto : IDtoResult<TKey>, IDto<TKey>, IIdentifiable<TKey> 
        where TKey : IEquatable<TKey>
    {
        
        /// <summary>
        /// Async Remove existing Item
        /// </summary>
        /// <param name="id">value of the Id property</param>
        /// <returns></returns>
        Task<IResult<bool>> RemoveByIdAsync([NotNull] TKey id);

    }
}