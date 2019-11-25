using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Schema;
using JetBrains.Annotations;
using Newtonsoft.Json;

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
        [JsonConverter(typeof(IdentifierJsonSerializer))]
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


}
