using System.Collections.Generic;
using System.Text;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Result;

namespace PH.Core3.Common.Extensions
{
    public static class LoggingExtensions
    {
        #region IResult

        [NotNull]
        public static IResult ErrorAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,string outputMessage = "")
        {
            l.LogError(errorMessage);
            return ResultFactory.Fail(i, errorMessage, null, outputMessage);
        }

        [NotNull]
        public static IResult ErrorAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId? eventId = null, string outputMessage = "")
        {
            l.LogError(errorMessage);
            return ResultFactory.Fail(i, errorMessage, eventId, outputMessage);
        }

        [NotNull]
        public static IResult ErrorAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId eventId, string outputMessage = "")
        {
            l.LogError(errorMessage);
            return ResultFactory.Fail(i, errorMessage, eventId, outputMessage);
        }

        [NotNull]
        public static IResult ErrorAndReturnFail(this ILogger l, [NotNull] IIdentifier i,[NotNull] ValidationResult fluentValidationResult)
        {
            StringBuilder sb = PrepareValidationFailures(fluentValidationResult, null, out var errors);
            l.LogError(sb.ToString());
            return ResultFactory.Fail(i, errors);
        }

        [NotNull]
        public static IResult CriticalAndReturnFail(this ILogger l, [NotNull] IIdentifier i,[NotNull] ValidationResult fluentValidationResult,EventId? eventId = null)
        {
            StringBuilder sb = PrepareValidationFailures(fluentValidationResult, eventId, out var errors);
            l.LogCritical(sb.ToString());
            return ResultFactory.Fail(i, errors);
        }

        [NotNull]
        public static IResult CriticalAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,string outputMessage = "")
        {
            l.LogCritical(errorMessage);
            return ResultFactory.Fail(i, errorMessage, null, outputMessage);
        }

        [NotNull]
        public static IResult CriticalAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId? eventId = null, string outputMessage = "")
        {
            l.LogCritical(errorMessage);
            return ResultFactory.Fail(i, errorMessage, eventId, outputMessage);
        }

        [NotNull]
        public static IResult CriticalAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId eventId, string outputMessage = "")
        {
            l.LogCritical(errorMessage);
            return ResultFactory.Fail(i, errorMessage, eventId, outputMessage);
        }

        #endregion

        #region IResult<T>

        [NotNull]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,[NotNull] ValidationResult fluentValidationResult,EventId? eventId = null)
        {
            StringBuilder sb = PrepareValidationFailures(fluentValidationResult, eventId, out var errors);
            l.LogError(sb.ToString());
            return ResultFactory.Fail<T>(i, errors);
        }

        [NotNull]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId? eventId = null, string outputMessage = "")
        {
            l.LogError(errorMessage);
            return ResultFactory.Fail<T>(i, errorMessage, outputMessage, eventId);
        }

        [NotNull]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId eventId , string outputMessage = "")
        {
            l.LogError(errorMessage);
            return ResultFactory.Fail<T>(i, errorMessage, outputMessage, eventId);
        }

        [NotNull]
        public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,[NotNull] ValidationResult fluentValidationResult,EventId? eventId = null)
        {
            StringBuilder sb = PrepareValidationFailures(fluentValidationResult, eventId, out var errors);
            l.LogCritical(sb.ToString());
            return ResultFactory.Fail<T>(i, errors);
        }


        [NotNull]
        public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId? eventId = null, string outputMessage = "")
        {
            l.LogCritical(errorMessage);
            return ResultFactory.Fail<T>(i, errorMessage, outputMessage, eventId);
        }
        
        [NotNull]
        public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId eventId, string outputMessage = "")
        {
            l.LogCritical(errorMessage);
            return ResultFactory.Fail<T>(i, errorMessage, outputMessage, eventId);
        }

        #endregion


        #region private methods

        [NotNull]
        private static StringBuilder PrepareValidationFailures([NotNull] ValidationResult fluentValidationResult,
                                                               EventId? eventId, [NotNull] out Error[] errors)
        {
            StringBuilder sb   = new StringBuilder();
            var           errs = new List<Error>();
            foreach (var failure in fluentValidationResult.Errors)
            {
                string msg = $"{failure.ErrorMessage}; ";
                if (!StringExtensions.IsNullString(failure.PropertyName) && failure.PropertyName != "*")
                    msg = $"PropertyName '{failure.PropertyName}' - ErrorMessage '{failure.ErrorMessage}'; ";

                sb.Append(msg);
                errs.Add(Error.Parse(msg, eventId));
            }

            errors = errs.ToArray();
            return sb;
        }

        #endregion
    }
}