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
    /// Service for Update exsisting items.
    /// </summary>
    /// <typeparam name="TDto">Type of the Result</typeparam>
    /// <typeparam name="TEditDto">Type of the Update Item</typeparam>
    /// <typeparam name="TKey">Type of the Result Id Property</typeparam>
    public interface IUpdateService<TDto, in TEditDto, in TKey> 
        : IReadOnlyService<TDto, TKey> 

          , ICrudAbstractionBase
        where TEditDto : IEditDto<TKey>, IDto<TKey>, IIdentifiable<TKey>
        where TDto : IDtoResult<TKey>, TEditDto, IDto<TKey>, IIdentifiable<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Async update a Dto
        /// </summary>
        /// <param name="entity">Content to Update</param>
        /// <returns><see cref="Result{TContent}"/> result</returns>
        Task<IResult<TDto>> UpdateAsync([NotNull] TEditDto entity);
    }
}