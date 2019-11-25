using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PH.Core3.Common.Result
{
    /// <summary>
    /// Error class
    /// </summary>
    /// <seealso cref="PH.Core3.Common.Result.IError" />
    public class Error : IError
    {
        /// <summary>
        /// The Error ErrorMessage
        /// </summary>
        public string ErrorMessage { get; }


        /// <summary>
        /// Error EventId if any
        /// </summary>
        public EventId? ErrorEventId { get; set; }

        /// <summary>
        /// Optional Message to Service that received the error
        /// </summary>
        public string OutputMessage { get; set; }

        /// <summary>
        /// Inner Error if any
        /// </summary>
        public IError InnerError { get; set; }

        /// <summary>Initializes a new instance of the <see cref="Error"/> class.</summary>
        /// <param name="errorMessage">The error message.</param>
        public Error(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>Initializes a new instance of the <see cref="Error"/> class.</summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="eventId">The event identifier.</param>
        public Error(string errorMessage, EventId eventId):this(errorMessage)
        {
            ErrorEventId = eventId;
        }

        /// <summary>Initializes a new instance of the <see cref="Error"/> class.</summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="innerError">The inner error.</param>
        public Error(string errorMessage, IError innerError):this(errorMessage)
        {
            InnerError = innerError;
        }

        /// <summary>Initializes a new instance of the <see cref="Error"/> class.</summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="innerError">The inner error.</param>
        public Error(string errorMessage, EventId eventId, IError innerError) : this(errorMessage, eventId)
        {
            InnerError = innerError;
        }

        /// <summary>Initializes a new instance of the <see cref="Error"/> class from a exception.</summary>
        /// <param name="exception">The exception.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">exception if null exception given</exception>
        [NotNull]
        public static Error FromException([NotNull] Exception exception, EventId? eventId = null)
        {
            if (null == exception)
            {
                throw new ArgumentNullException(nameof(exception));
            }
            
            var e = new Error(exception.Message);
            
            if (null != eventId)
            {
                e.ErrorEventId = eventId;
            }
            if (null != exception.InnerException)
            {
                e.InnerError = FromException(exception.InnerException);
            }

            return e;
        }
        
    }

    
}