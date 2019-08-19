using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Models.ViewModels;
using PH.Core3.Common.Result;

namespace PH.Core3.Common.Services.Crud
{
    
    /// <summary>
    /// Simple Remove Service
    /// </summary>
    /// <typeparam name="T">The Entity Type</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface ISimpleRemoveService<T, in TKey> : ISimpleReadService<T, TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Async Remove existing Item
        /// </summary>
        /// <param name="id">value of the Id property</param>
        /// <returns></returns>
        Task<IResult<bool>> EntityRemoveByIdAsync([NotNull] TKey id);

    }

    /// <summary>
    /// Simple Crud Service
    /// </summary>
    /// <typeparam name="T">The Entity Type</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface ICrudSimpleService<T, in TKey> : ISimpleReadService<T,TKey>
    ,  ISimpleAddService<T, TKey>, ISimpleUpdateService<T,TKey>, ISimpleRemoveService<T,TKey>
        where TKey : IEquatable<TKey>
    {
        

        



    }

    /// <summary>
    /// Service for CRUD (Create/Read/Update/Delete) Operations.
    ///
    /// 
    ///     <see cref="IReadOnlyService{TDto,TKey}"/>
    ///     <see cref="IAddService{TDto,TNewDto,TKey}"/>
    ///     <see cref="IUpdateService{TDto,TEditDto,TKey}"/>
    ///     <see cref="IRemoveService{TDto,TKey}"/>
    /// </summary>
    /// <typeparam name="TDto">Type of the Result</typeparam>
    /// <typeparam name="TNewDto">Type of the Add Class</typeparam>
    /// <typeparam name="TEditDto">Type of the Edit Class</typeparam>
    /// <typeparam name="TKey">Type of the Result and Edit Id Property</typeparam>
    public interface ICrudService<TDto, in TNewDto, in TEditDto, in TKey> :
        IReadOnlyService<TDto, TKey>
        , IAddService<TDto, TNewDto, TKey>
        , IUpdateService<TDto, TEditDto, TKey>
        , IRemoveService<TDto, TKey>
        , ICrudAbstractionBase
        where TNewDto : IDto, INewDto 
        where TEditDto :  TNewDto, IEditDto<TKey>, IDto<TKey>, IIdentifiable<TKey>
        where TDto : IDtoResult<TKey>, TEditDto, IDto<TKey>, IIdentifiable<TKey>
        where TKey : IEquatable<TKey>
    {
    }
}