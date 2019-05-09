using System;

namespace PH.Core3.Common.CoreSystem
{

    /// <summary>
    /// Core Exception class
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CoreException : Exception 
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public IIdentifier Identifier { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public CoreException(IIdentifier identifier)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="message">The message.</param>
        public CoreException(IIdentifier identifier, string message):base(message)
        {
            Identifier = identifier;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CoreException(IIdentifier identifier,string message, Exception innerException):base(message, innerException)
        {
            Identifier = identifier;
        }
        
    }
}