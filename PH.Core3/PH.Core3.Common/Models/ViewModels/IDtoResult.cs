using System;

namespace PH.Core3.Common.Models.ViewModels
{
    /// <summary>
    /// Interface for DTO result
    ///
    /// <seealso cref="IEditDto{TKey}"/>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IDtoResult<TKey> : IEditDto<TKey>
        where TKey : IEquatable<TKey>
    {
    }
}