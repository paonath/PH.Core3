using System;
using System.Collections.Generic;
using System.Linq;

namespace PH.Core3.Common.Models.ViewModels.Tree
{
    
    /// <summary>
    /// Class of Tree Item Result
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TTree">Interface type result</typeparam>
    /// <typeparam name="TSelf">Class type result</typeparam>
    public abstract class TreeItemDto<TSelf,TTree, TKey> : DtoResult<TKey>, ITreeItemDto<TTree, TKey>
        where TKey : struct, IEquatable<TKey>
        where TTree : ITreeItemDto<TTree, TKey>
        where TSelf : class, TTree,   ITreeItemDto<TTree, TKey>
    {
        ///// <summary>
        ///// Unique Id of current class
        ///// </summary>
        //public TKey Id { get; set; }

        /// <summary>
        /// Reference Id for Parent
        ///
        /// Null if current item is a root
        /// </summary>
        public TKey? ParentId { get; set; }

        /// <summary>
        /// Reference Id For Root Item
        /// </summary>
        public TKey RootId { get; set; }


        /// <summary>
        /// List of Children Items
        /// </summary>
        public List<TTree> Childrens { get; set; }

        /// <summary>
        /// Deep of Tree
        /// </summary>
        public Lazy<int> Deep => GetDeep();


        /// <summary>
        /// Tree Count
        /// </summary>
        public Lazy<int> TreeCount => GetTreeCount();

        /// <summary>
        /// Position On Level Of Tree
        /// </summary>
        public int EntityLevel { get; set; }

        private Lazy<int> GetTreeCount()
        {
           
            if (null != Childrens && Childrens.Count == 0)
            {
                return new Lazy<int>(() => 1);
            }


            var l = new Lazy<int>( () => 1 + (Childrens?.Select(x => x.TreeCount.Value)?.Sum()
                                              ?? 0));

            return l;
        }


        private Lazy<int> GetDeep()
        {
            if (null != Childrens && Childrens.Count == 0)
                return new Lazy<int>(() => 1);

            var l = new Lazy<int>(() => 1 + (Childrens?.OrderByDescending(x => x.Deep.Value)?.Select(x => x.Deep.Value)?.FirstOrDefault()
                                       ?? 0));


            return l;
        }

        /// <summary>
        /// Init new Instance
        /// </summary>
        protected TreeItemDto()
        {
           Childrens = new List<TTree>();
        }
    }
}