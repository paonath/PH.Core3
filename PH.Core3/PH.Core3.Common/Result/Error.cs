using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PH.Core3.Common.Result
{
    /// <summary>
    /// Error instance
    /// </summary>
    public class Error : IError
    {

        /// <summary>Initializes a new instance of the <see cref="Error"/> class.</summary>
        /// <param name="errorMessage">The error message.</param>
        public Error([NotNull] string errorMessage):this(errorMessage,null,null)
        {
            
        }

        /// <summary>
        /// Init new instance of Error
        /// </summary>
        /// <param name="errorMessage">Error message (useful for logging)</param>
        /// <param name="outputMessage">Optional Message to Service that received the error</param>
        /// <param name="errorEventId">Event Id for this error</param>
        /// <param name="innerError">Inner Error</param>
        public Error([NotNull] string errorMessage, [CanBeNull] string outputMessage, EventId? errorEventId, [CanBeNull] IError innerError = null)
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(errorMessage));
            }

            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorMessage));
            }

            ErrorMessage  = errorMessage;
            OutputMessage = outputMessage;
            ErrorEventId = errorEventId;
            InnerError    = innerError;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get;  }

        /// <summary>
        /// Event Id for this error
        /// </summary>
        public EventId? ErrorEventId { get; }

        /// <summary>
        /// Optional Message to Service that received the error
        /// </summary>
        public string OutputMessage { get;  }

        /// <summary>
        /// Inner Error
        /// </summary>
        public IError InnerError { get; }

        /// <summary>
        /// Exception related to this error
        /// </summary>
        [JsonIgnore]
        public Exception Exception { get; set; }


        /// <summary>
        /// Parse a string error message and optional event id and return new instance of <see cref="Error"/>
        /// </summary>
        /// <param name="errorMessage">Error message</param>
        /// <param name="eventId">event id</param>
        /// <returns>instance of Error</returns>
        [NotNull]
        public static Error Parse([NotNull] string errorMessage, EventId? eventId = null)
        {
            return new Error(errorMessage, "",eventId);
        }

        /// <summary>
        /// Parse a string array messages and return new array of Errors
        /// </summary>
        /// <param name="errorMessages">messages</param>
        /// <returns>Error array</returns>
        [NotNull]
        public static Error[] Parse([NotNull] string[] errorMessages)
        {
            var l = new List<Error>();
            foreach (var errorMessage in errorMessages)
            {
                l.Add(Parse(errorMessage));
            }

            return l.ToArray();
        }
    }
}