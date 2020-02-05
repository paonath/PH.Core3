using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Models.ViewModels;
using PH.Results;
using PH.Results.Internals;


namespace PH.Core3.Common.Services.Crud
{
    

    /// <summary>
    /// Service for Adding new Items
    /// </summary>
    /// <typeparam name="TDto">Type of the Result</typeparam>
    /// <typeparam name="TNewDto">Type of the Add Item</typeparam>
    /// <typeparam name="TKey">Type of the Result Id Property</typeparam>
    public interface IAddService<TDto, in TNewDto, in TKey> 
        : IReadOnlyService<TDto, TKey>,  ICrudAbstractionBase
        where TNewDto : INewDto
        where TDto : IDtoResult<TKey>, TNewDto, IDto<TKey>, IIdentifiable<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Async Add new Item
        /// </summary>
        /// <param name="entity">Item to Add</param>
        /// <returns><see cref="Result{TContent}"/> containing added Item or error</returns>
        Task<IResult<TDto>> AddAsync([NotNull] TNewDto entity);
    }
}