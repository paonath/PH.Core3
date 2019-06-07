using System;
using System.Collections.Generic;

namespace PH.Core3.Common.Result
{
    /// <summary>
    /// Result
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// Gets a value indicating whether on error.
        /// </summary>
        /// <value><c>true</c> if on error; otherwise, <c>false</c>.</value>
        bool OnError { get; }

        /// <summary>
        /// List of Errors, empty id result is Ok
        /// </summary>
        List<IError> Errors { get; }

        /// <summary>Gets the identifier.</summary>
        /// <value>The identifier.</value>
        IIdentifier Identifier { get; }


    }

    


    /// <summary>
    /// Transport object wrapping a Result
    /// </summary>
    /// <typeparam name="TContent">Type of the Result content</typeparam>
    public interface IResult<out TContent> : IResult
    {
        /// <summary>
        /// Result Content
        /// </summary>
        TContent Content { get; }
    }

    /// <summary>
    /// Transport object wrapping a Result array paged
    /// </summary>
    /// <typeparam name="TContent">Type of the Result content</typeparam>
    public interface IPagedResult<out TContent> : IResult<TContent[]>, IResult
    {
        /// <summary>
        /// Count of All Items
        /// </summary>
        long Count { get;  }

        /// <summary>
        /// Page Number
        /// </summary>
        int PageNumber { get;  }

        /// <summary>
        /// Number Of items per Page
        /// </summary>
        int PageSize { get;  }

        /// <summary>
        /// Get Number of Total Pages if Pagination not disabled
        ///
        /// If Result is OnError this value is always -1.
        /// </summary>
        /// <returns></returns>
        Lazy<long> PageCount{ get;  }


        /// <summary>
        /// Result Content
        /// </summary>
        new TContent[] Content { get; }

    }
}