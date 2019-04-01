using System;

namespace PH.Core3.Common.Models.ViewModels
{
    /// <summary>
    /// DTO for Edit/Update
    ///
    /// <seealso cref="INewDto"/>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEditDto<TKey> : INewDto, IDto<TKey>
        where TKey : IEquatable<TKey>
    {
    }
}