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

    internal class ResultErrorWithContent<TContent> : Result<TContent>, IResult<TContent>,
                                                      IResultErrorWithContent<TContent>
    {
        
        /// <summary>Initializes a new instance of the Result on error with content class.</summary>
        public ResultErrorWithContent([NotNull] TContent content, [NotNull] List<IError> errors, [NotNull] IIdentifier identifier) : base(content, errors, identifier)
        {
        }
    }

    /// <summary>
    /// Transport object wrapping a real Result
    ///
    /// <see cref="IResult{TContent}"/>
    /// </summary>
    /// <typeparam name="TContent">Type of the Result content</typeparam>
    internal class Result<TContent> : IResult<TContent>
    {
        /// <summary>
        /// 
        /// </summary>
        private Result()
        {
            Errors = new List<IError>();
        }

        /// <summary>
        /// Init new instance of result with no error
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="content">Content</param>
        internal Result([NotNull] IIdentifier identifier,[NotNull] TContent content)
        {
            Content = content;
            Identifier = identifier;
        }

        /// <summary>
        /// Init new instance of result with errors
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="errors">errors </param>
        internal Result([NotNull] IIdentifier identifier, [CanBeNull] IEnumerable<IError> errors)
        {
            Identifier = identifier;
            Errors = errors?.ToList() ?? new List<IError>();
        }

        /// <summary>Initializes a new instance of the Result on error with content class.</summary>
        public Result([NotNull] TContent content,[NotNull] List<IError> errors, [NotNull] IIdentifier identifier)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            Errors = errors ?? throw new ArgumentNullException(nameof(errors));
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            Content = content;
        }


        /// <summary>
        /// True if On Error
        /// </summary>
        public bool OnError => Errors != null && Errors.Count > 0;

        /// <summary>
        /// List of Errors, empty id result is Ok
        /// </summary>
        public List<IError> Errors { get; }
        public IIdentifier Identifier { get; }

        /// <summary>
        /// Result Content
        /// </summary>
        public TContent Content { get; protected set; }
    }


    /*
    internal class PagedResult<TContent[]> : Result<TContent[]>
    {
        /// <summary>
        /// Init new instance of result with no error
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="content">Content</param>
        internal PagedResult([NotNull] IIdentifier identifier, [NotNull] TContent[] content) : base(identifier, content)
        {
        }

        /// <summary>
        /// Init new instance of result with errors
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="errors">errors </param>
        internal PagedResult([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors) : base(identifier, errors)
        {
        }
    }*/

}
