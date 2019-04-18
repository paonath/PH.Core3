using System;
using System.Runtime.Serialization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace PH.Core3.Common.Result
{
    /// <summary>
    /// Error
    /// </summary>
    public interface IError
    {
        /// <summary>
        /// Error Message
        /// </summary>
        string ErrorMessage { get; }

        /// <summary>
        /// Event Id
        /// </summary>
        EventId? ErrorEventId { get; }

        /// <summary>
        /// Optional Message to Service that received the error
        /// </summary>
        string OutputMessage { get; }

        /// <summary>
        /// Inner Error
        /// </summary>
        IError InnerError { get; }

        /// <summary>
        /// Error Exception
        /// </summary>
        [JsonIgnore]
        Exception Exception { get; }
    }
}