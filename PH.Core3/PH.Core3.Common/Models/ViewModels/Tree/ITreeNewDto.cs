using System;

namespace PH.Core3.Common.Models.ViewModels.Tree
{
    /// <summary>
    /// DTO for Insert/Create
    /// </summary>
    public interface ITreeNewDto<TKey> : ITreeDto, INewDto
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Reference Id for Parent
        ///
        /// Null if current item is a root
        /// </summary>
        Nullable<TKey> ParentId { get; set; }
    }
}