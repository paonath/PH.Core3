using System;

namespace PH.Core3.Common.Models.ViewModels.Tree
{
    /// <summary>
    /// A Data Transfer Object Tree Item
    /// </summary>
    /// <typeparam name="TKey">Type of the Id Property (this must be a struct, e.g. Guid)</typeparam>
    public interface IEditTreeDto<TKey> :  ITreeNewDto<TKey>, IEditDto<TKey>
        where TKey : struct, IEquatable<TKey>
    {
    }
}