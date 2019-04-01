using System;

namespace PH.Core3.Common.CoreSystem
{
    
    /// <summary>
    /// anything that should have an identifier
    /// </summary>
    /// <typeparam name="TKey">Type of Id Property</typeparam>
    public interface IIdentifiable<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Unique Id of current class
        /// </summary>
        TKey Id { get; set; }
    }
}