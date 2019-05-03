using System;
using System.Threading.Tasks;
using PH.Core3.Common.Models.Entities;
using PH.Core3.Common.Models.ViewModels;
using PH.Core3.Common.Result;

namespace PH.Core3.Common.Services.Crud
{
    /// <summary>
    /// Service for CRUD (Create/Read/Update/Delete) Operations on <see cref="ITreeItem">Tree Items</see>.
    ///     
    ///     <see cref="ICrudService{TDto,TNewDto,TEditDto,TKey}"/>
    /// </summary>
    /// <typeparam name="TDto">Type of the Result</typeparam>
    /// <typeparam name="TNewDto">Type of the Add Class</typeparam>
    /// <typeparam name="TEditDto">Type of the Edit Class</typeparam>
    /// <typeparam name="TKey">Type of the Result and Edit Id Property (this must be a struct, e.g. a Guid)</typeparam>
    public interface ITreeCrudService<TDto, in TNewDto, in TEditDto, in TKey> : ICrudService<TDto, TNewDto, TEditDto, TKey>
        
        where TKey : struct, IEquatable<TKey>
        where TNewDto : INewDto
        where TEditDto :  TNewDto, IEditDto<TKey>
        where TDto :   TEditDto, IDtoResult<TKey>
    {
        /// <summary>
        /// Load All Items as Tree
        /// </summary>
        /// <returns><see cref="Result{TContent}"/> instance</returns>
        Task<IResult<TDto[]>> LoadAllAsTreeAsync();
    }
}