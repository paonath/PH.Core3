using Microsoft.Extensions.Logging;

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

        EventId? ErrorEventId { get; }

        /// <summary>
        /// Optional Message to Service that received the error
        /// </summary>
        string OutputMessage { get; }

        IError InnerError { get; }
    }
}