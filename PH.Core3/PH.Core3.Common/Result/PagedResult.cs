using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using JetBrains.Annotations;

namespace PH.Core3.Common.Result
{
    internal class PagedResult<TContent> : Result<TContent[]>, IPagedResult<TContent> , IResult<TContent[]>
    {
        public bool PaginationDisabled => Count == -1 && PageSize == -1 && PageNumber == -1;

        /// <summary>
        /// Init new instance of result with no error
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="content">Content</param>
        /// <param name="count">Number of Total Items</param>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="pageSize">Items per Page</param>
        internal PagedResult([NotNull] IIdentifier identifier, [NotNull] TContent[] content, long count, int pageNumber, int pageSize) : base(identifier, content)
        {
            Count      = count;
            PageNumber = pageNumber;
            PageSize   = pageSize;
        }

        /// <summary>
        /// Init new instance of result with errors
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="errors">errors </param>
        internal PagedResult([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors) : base(identifier, errors)
        {
            Count      = -1;
            PageNumber = -1;
            PageSize   = -1;
        }

        /// <summary>
        /// Count of All Items
        /// </summary>
        public long Count { get; }

        /// <summary>
        /// Page Number
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Number Of items per Page
        /// </summary>
        public int PageSize { get; }

        public Lazy<long> PageCount => new Lazy<long>(CountPages);
      

        private long CountPages()
        {
            if (PaginationDisabled)
                return 0;

            var m = Count / PageSize;
            if (Count % PageSize != 0)
                m++;

            return m;

        }
    }


    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="TContent"></typeparam>
    //internal class FilterResult<TContent> : PagedResult<TContent>
    //{
        

    //    /// <summary>
    //    /// Init new instance of result with no error
    //    /// </summary>
    //    /// <param name="identifier">Identifier</param>
    //    /// <param name="content">Content</param>
    //    /// <param name="count">Number of Total Items</param>
    //    /// <param name="pageNumber">Current page number</param>
    //    /// <param name="pageSize">Items per Page</param>
    //    internal FilterResult([NotNull] IIdentifier identifier,[NotNull] Expression filterExpression, [NotNull] TContent[] content, long count, int pageNumber, int pageSize) : base(identifier, content, count, pageNumber, pageSize)
    //    {
    //        FilterExpression = filterExpression;
           
    //    }

    //    /// <summary>
    //    /// Init new instance of result with errors
    //    /// </summary>
    //    /// <param name="identifier">Identifier</param>
    //    /// <param name="errors">errors </param>
    //    internal FilterResult([NotNull] IIdentifier identifier,[NotNull] Expression filterExpression, [NotNull] IEnumerable<IError> errors) : base(identifier, errors)
    //    {
    //        FilterExpression = filterExpression;
    //    }


    //    public Expression FilterExpression { get; }
    //}
}