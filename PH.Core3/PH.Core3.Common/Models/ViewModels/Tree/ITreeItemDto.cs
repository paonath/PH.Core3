using System;
using System.Collections.Generic;

namespace PH.Core3.Common.Models.ViewModels.Tree
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface ITreeItemDto<TKey> : IEditTreeDto<TKey> , IDtoResult<TKey>
        where TKey : struct, IEquatable<TKey>
    {
        /// <summary>
        /// Reference Id For Root Item
        /// </summary>
        TKey RootId { get; set; }
    }

    /// <summary>
    /// Interface of Tree Item Result
    /// </summary>
    /// <typeparam name="TTree"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface ITreeItemDto<TTree, TKey> : ITreeItemDto<TKey>
        where TKey : struct, IEquatable<TKey>
        where TTree : ITreeItemDto<TTree, TKey>
    {
        /// <summary>
        /// List of Children Items
        /// </summary>
        List<TTree> Childrens { get; set; }

        /// <summary>
        /// Deep of Tree
        /// </summary>
        Lazy<int> Deep { get; }

        /// <summary>
        /// Tree Count
        /// </summary>
        Lazy<int> TreeCount { get; }

        /// <summary>
        /// Position On Level Of Tree
        /// </summary>
        int EntityLevel { get; set; }

    }

}