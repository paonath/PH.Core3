using System;
using JetBrains.Annotations;

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
        /// Optional Message to Service that received the error
        /// </summary>
        string OutputMessage { get; }

        IError InnerError { get; }
    }

    public class Error : IError
    {

        public Error([NotNull] string errorMessage, [CanBeNull] string outputMessage, [CanBeNull] IError innerError = null)
        {
            if (string.IsNullOrEmpty(errorMessage))
                throw new ArgumentException("Value cannot be null or empty.", nameof(errorMessage));
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorMessage));

            ErrorMessage = errorMessage;
            OutputMessage = outputMessage;
            InnerError = innerError;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get;  }

        /// <summary>
        /// Optional Message to Service that received the error
        /// </summary>
        public string OutputMessage { get;  }

        public IError InnerError { get; }
    }
}