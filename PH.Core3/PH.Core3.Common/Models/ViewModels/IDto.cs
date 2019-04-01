using System;
using PH.Core3.Common.CoreSystem;

namespace PH.Core3.Common.Models.ViewModels
{
    /// <summary>
    /// A Data Transfer Object
    /// </summary>
    public interface IDto
    {
    }


    /// <summary>
    /// A Data Transfer Object
    /// </summary>
    /// <typeparam name="TKey">Type of the Id Property</typeparam>
    public interface IDto<TKey> : IDto, IIdentifiable<TKey>
        where TKey : IEquatable<TKey>
    {

    }
}