using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace PH.Core3.Common.Result
{
    public class Error : IError
    {

        public Error([NotNull] string errorMessage, [CanBeNull] string outputMessage, [CanBeNull] IError innerError = null)
        {
            if (string.IsNullOrEmpty(errorMessage))
                throw new ArgumentException("Value cannot be null or empty.", nameof(errorMessage));
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(errorMessage));

            ErrorMessage  = errorMessage;
            OutputMessage = outputMessage;
            InnerError    = innerError;
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

        [NotNull]
        public static Error Parse([NotNull] string errorMessage)
        {
            return new Error(errorMessage, "");
        }

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