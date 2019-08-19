using System;

namespace PH.Core3.Common.Result
{
    

    ///// <summary>
    ///// Transport object wrapping a Result array paged
    ///// </summary>
    ///// <typeparam name="TContent">Type of the Result content</typeparam>
    //public interface IPagedResult<out TContent> : IResult<TContent[]>
    //{
    //    /// <summary>
    //    /// Count of All Items
    //    /// </summary>
    //    long Count { get; }

    //    /// <summary>
    //    /// Page Number
    //    /// </summary>
    //    int PageNumber { get; }

    //    /// <summary>
    //    /// Number Of items per Page
    //    /// </summary>
    //    int PageSize { get; }

    //    /// <summary>
    //    /// Get Number of Total Pages if Pagination not disabled
    //    ///
    //    /// If Result is OnError this value is always -1.
    //    /// </summary>
    //    /// <returns></returns>
    //    Lazy<long> PageCount { get; }


    //    /// <summary>
    //    /// Result Content
    //    /// </summary>
    //    new TContent[] Content { get; }

    //}
}