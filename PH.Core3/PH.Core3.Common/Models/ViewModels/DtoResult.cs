using System;

namespace PH.Core3.Common.Models.ViewModels
{
    public abstract class DtoResult<TKey> : IDtoResult<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Unique Id of current class
        /// </summary>
        public TKey Id { get; set; }
    }
}