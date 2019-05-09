using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace PH.Core3.Common.CoreSystem
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.AggregateException" />
    /// <seealso cref="CoreException" />
    public class CoreAggregateException : AggregateException
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public IIdentifier Identifier { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.AggregateException"></see> class with references to the inner exceptions that are the cause of this exception.</summary>
        /// <param name="identifier">The identifier</param>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="innerExceptions">innerExceptions</paramref> argument is null.</exception>
        /// <exception cref="T:System.ArgumentException">An element of <paramref name="innerExceptions">innerExceptions</paramref> is null.</exception>
        public CoreAggregateException(IIdentifier identifier, IEnumerable<Exception> innerExceptions) : base(innerExceptions)
        {
            Identifier = identifier;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.AggregateException"></see> class with references to the inner exceptions that are the cause of this exception.</summary>
        /// <param name="identifier">The identifier</param>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="innerExceptions">innerExceptions</paramref> argument is null.</exception>
        /// <exception cref="T:System.ArgumentException">An element of <paramref name="innerExceptions">innerExceptions</paramref> is null.</exception>
        public CoreAggregateException(IIdentifier identifier, [NotNull] params Exception[] innerExceptions) : base(innerExceptions)
        {
            Identifier = identifier;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.AggregateException"></see> class with a specified error message and references to the inner exceptions that are the cause of this exception.</summary>
        /// <param name="identifier">The identifier</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="innerExceptions">innerExceptions</paramref> argument is null.</exception>
        /// <exception cref="T:System.ArgumentException">An element of <paramref name="innerExceptions">innerExceptions</paramref> is null.</exception>
        public CoreAggregateException(IIdentifier identifier,string message, IEnumerable<Exception> innerExceptions) : base(message, innerExceptions)
        {
            Identifier = identifier;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.AggregateException"></see> class with a specified error message and references to the inner exceptions that are the cause of this exception.</summary>
        /// <param name="identifier">The identifier</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="innerExceptions">innerExceptions</paramref> argument is null.</exception>
        /// <exception cref="T:System.ArgumentException">An element of <paramref name="innerExceptions">innerExceptions</paramref> is null.</exception>
        public CoreAggregateException(IIdentifier identifier, string message, [NotNull] params Exception[] innerExceptions) : base(message, innerExceptions)
        {
            Identifier = identifier;
        }
    }
}