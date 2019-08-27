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

    ///// <summary>
    ///// Error instance
    ///// </summary>
    //public class Error : IError
    //{

    //    /// <summary>Initializes a new instance of the <see cref="Error"/> class.</summary>
    //    /// <param name="errorMessage">The error errorMessage.</param>
    //    public Error([NotNull] string errorMessage):this(errorMessage,null,null)
    //    {
            
    //    }

    //    /// <summary>
    //    /// Init new instance of Error
    //    /// </summary>
    //    /// <param name="errorMessage">Error errorMessage (useful for logging)</param>
    //    /// <param name="outputMessage">Optional ErrorMessage to Service that received the error</param>
    //    /// <param name="errorEventId">Event Id for this error</param>
    //    /// <param name="innerError">Inner Error</param>
    //    public Error([NotNull] string errorMessage, [CanBeNull] string outputMessage, EventId? errorEventId, [CanBeNull] IError innerError = null)
    //    {
    //        if (string.IsNullOrEmpty(errorMessage))
    //        {
    //            throw new ArgumentException("Value cannot be null or empty.", nameof(errorMessage));
    //        }

    //        if (string.IsNullOrWhiteSpace(errorMessage))
    //        {
    //            throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorMessage));
    //        }

    //        ErrorMessage  = errorMessage;
    //        OutputMessage = outputMessage;
    //        ErrorEventId = errorEventId;
    //        InnerError    = innerError;
    //    }

    //    /// <summary>
    //    /// Error ErrorMessage
    //    /// </summary>
    //    public string ErrorMessage { get;  }

    //    /// <summary>
    //    /// Event Id for this error
    //    /// </summary>
    //    public EventId? ErrorEventId { get; }

    //    /// <summary>
    //    /// Optional ErrorMessage to Service that received the error
    //    /// </summary>
    //    public string OutputMessage { get;  }

    //    /// <summary>
    //    /// Inner Error
    //    /// </summary>
    //    public IError InnerError { get; }

    //    /// <summary>
    //    /// Exception related to this error
    //    /// </summary>
    //    [JsonIgnore]
    //    public Exception Exception { get; set; }


    //    /// <summary>
    //    /// Parse a string error errorMessage and optional event id and return new instance of <see cref="Error"/>
    //    /// </summary>
    //    /// <param name="errorMessage">Error errorMessage</param>
    //    /// <param name="eventId">event id</param>
    //    /// <returns>instance of Error</returns>
    //    [NotNull]
    //    public static Error Parse([NotNull] string errorMessage, EventId? eventId = null)
    //    {
    //        return new Error(errorMessage, "",eventId);
    //    }

    //    /// <summary>
    //    /// Parse a string array messages and return new array of Errors
    //    /// </summary>
    //    /// <param name="errorMessages">messages</param>
    //    /// <returns>Error array</returns>
    //    [NotNull]
    //    public static Error[] Parse([NotNull] string[] errorMessages)
    //    {
    //        var l = new List<Error>();
    //        foreach (var errorMessage in errorMessages)
    //        {
    //            l.Add(Parse(errorMessage));
    //        }

    //        return l.ToArray();
    //    }
    //}
}