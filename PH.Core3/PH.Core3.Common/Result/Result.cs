using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Schema;
using JetBrains.Annotations;

namespace PH.Core3.Common.Result
{

    /// <summary>
    /// Transport object result wrapping a contents
    /// </summary>
    /// <typeparam name="T">Type of the Content</typeparam>
    /// <seealso cref="PH.Core3.Common.Result.IResult{T}" />
    public class Result<T> :  IResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="error">The error.</param>
        internal Result([NotNull] IIdentifier identifier, [CanBeNull] T content, [CanBeNull] IError error = null)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            
            Content = content;
            Error   = error;
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{T}"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="error">The error.</param>
        /// <exception cref="ArgumentNullException">
        /// error
        /// or
        /// identifier
        /// </exception>
        internal Result([NotNull] IIdentifier identifier, [NotNull] IError error)
        {
            Error      = error ?? throw new ArgumentNullException(nameof(error));
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        /// <summary>Gets the identifier.</summary>
        /// <value>The identifier.</value>
        public IIdentifier Identifier { get; }

        /// <summary>Result Content</summary>
        public T Content { get; }

        /// <summary>
        /// Gets a value indicating whether on error.
        /// </summary>
        /// <value><c>true</c> if on error; otherwise, <c>false</c>.</value>
        public bool OnError => Error != null;

        /// <summary>Gets the error, if any.</summary>
        /// <value>The error.</value>
        public IError Error { get; }
    }


    //public class PagedResult<TContent> : Result<TContent[]>, IResult<TContent[]>, IPagedResult<TContent>
    //{
    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    //    /// </summary>
    //    /// <param name="identifier">The identifier.</param>
    //    /// <param name="content">The content.</param>
    //    /// <param name="error">The error.</param>
    //    /// <exception cref="ArgumentNullException">
    //    /// identifier
    //    /// or
    //    /// content
    //    /// </exception>
    //    internal PagedResult([NotNull] IIdentifier identifier, [CanBeNull] TContent[] content, [CanBeNull] IError error = null) : base(identifier, content, error)
    //    {
    //    }

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    //    /// </summary>
    //    /// <param name="identifier">The identifier.</param>
    //    /// <param name="error">The error.</param>
    //    /// <exception cref="ArgumentNullException">
    //    /// error
    //    /// or
    //    /// identifier
    //    /// </exception>
    //    internal PagedResult([NotNull] IIdentifier identifier, [NotNull] IError error) : base(identifier, error)
    //    {
    //    }

    //    /// <summary>
    //    /// Count of All Items
    //    /// </summary>
    //    public long Count { get; }

    //    /// <summary>
    //    /// Page Number
    //    /// </summary>
    //    public int PageNumber { get; }

    //    /// <summary>
    //    /// Number Of items per Page
    //    /// </summary>
    //    public int PageSize { get; }

    //    /// <summary>
    //    /// Get Number of Total Pages if Pagination not disabled
    //    ///
    //    /// If Result is OnError this value is always -1.
    //    /// </summary>
    //    /// <returns></returns>
    //    public Lazy<long> PageCount { get; }
    //}

    ////internal class ResultErrorWithContent<TContent> : Result<TContent>, IResult<TContent>,
    ////                                                  IResultErrorWithContent<TContent>
    ////{
        
    ////    /// <summary>Initializes a new instance of the Result on error with content class.</summary>
    ////    public ResultErrorWithContent([NotNull] TContent content, [NotNull] List<IError> errors, [NotNull] IIdentifier identifier) : base(content, errors, identifier)
    ////    {
    ////    }
    ////}

    /////// <summary>
    /////// Transport object wrapping a real Result
    ///////
    /////// <see cref="IResult{TContent}"/>
    /////// </summary>
    /////// <typeparam name="TContent">Type of the Result content</typeparam>
    ////internal class Result<TContent> : IResult<TContent>
    ////{
    ////    /// <summary>
    ////    /// 
    ////    /// </summary>
    ////    private Result()
    ////    {
    ////        Errors = new List<IError>();
    ////    }

    ////    /// <summary>
    ////    /// Init new instance of result with no error
    ////    /// </summary>
    ////    /// <param name="identifier">Identifier</param>
    ////    /// <param name="content">Content</param>
    ////    internal Result([NotNull] IIdentifier identifier,[NotNull] TContent content)
    ////    {
    ////        Content = content;
    ////        Identifier = identifier;
    ////    }

    ////    /// <summary>
    ////    /// Init new instance of result with errors
    ////    /// </summary>
    ////    /// <param name="identifier">Identifier</param>
    ////    /// <param name="errors">errors </param>
    ////    internal Result([NotNull] IIdentifier identifier, [CanBeNull] IEnumerable<IError> errors)
    ////    {
    ////        Identifier = identifier;
    ////        Errors = errors?.ToList() ?? new List<IError>();
    ////    }

    ////    /// <summary>Initializes a new instance of the Result on error with content class.</summary>
    ////    public Result([NotNull] TContent content,[NotNull] List<IError> errors, [NotNull] IIdentifier identifier)
    ////    {
    ////        if (content == null)
    ////        {
    ////            throw new ArgumentNullException(nameof(content));
    ////        }

    ////        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    ////        Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
    ////        Content = content;
    ////    }


    ////    /// <summary>
    ////    /// True if On Error
    ////    /// </summary>
    ////    public bool OnError => Errors != null && Errors.Count > 0;

    ////    /// <summary>
    ////    /// List of Errors, empty id result is Ok
    ////    /// </summary>
    ////    public List<IError> Errors { get; }
    ////    public IIdentifier Identifier { get; }

    ////    /// <summary>
    ////    /// Result Content
    ////    /// </summary>
    ////    public TContent Content { get; protected set; }
    ////}


    ///*
    //internal class PagedResult<TContent[]> : Result<TContent[]>
    //{
    //    /// <summary>
    //    /// Init new instance of result with no error
    //    /// </summary>
    //    /// <param name="identifier">Identifier</param>
    //    /// <param name="content">Content</param>
    //    internal PagedResult([NotNull] IIdentifier identifier, [NotNull] TContent[] content) : base(identifier, content)
    //    {
    //    }

    //    /// <summary>
    //    /// Init new instance of result with errors
    //    /// </summary>
    //    /// <param name="identifier">Identifier</param>
    //    /// <param name="errors">errors </param>
    //    internal PagedResult([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors) : base(identifier, errors)
    //    {
    //    }
    //}*/

}
