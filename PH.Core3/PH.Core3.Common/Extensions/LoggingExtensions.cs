using System.Collections.Generic;
using System.Text;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Result;

namespace PH.Core3.Common.Extensions
{
    /// <summary>
    /// Logging useful extensions
    /// </summary>
    public static class LoggingExtensions
    {

        
        /// <summary>
        /// Log error
        /// </summary>
        /// <param name="l">logger</param>
        /// <param name="error">error</param>
        public static void LogOnlyError(this ILogger l, [NotNull] IError error)
        {
            if (error.ErrorEventId.HasValue)
            {
                var evId = error.ErrorEventId.Value;
                l.Log(LogLevel.Error,evId, error.ErrorMessage);
            }
            else
            {
                l.LogError(error.ErrorMessage);
            }
        }

        /// <summary>
        /// log critical
        /// </summary>
        /// <param name="l">logger</param>
        /// <param name="error">error</param>
        public static void LogOnlyCritical(this ILogger l, [NotNull] IError error)
        {
            if (error.ErrorEventId.HasValue)
            {
                var evId = error.ErrorEventId.Value;
                l.Log(LogLevel.Critical,evId, error.ErrorMessage);
            }
            else
            {
                l.LogCritical(error.ErrorMessage);
            }

            
        }

        #region IResult

        /// <summary>
        /// Log error and return IResult fail
        /// </summary>
        /// <param name="l">logger</param>
        /// <param name="i">identifier</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="outputMessage">output message</param>
        /// <returns>Iresult on fail</returns>
        [NotNull]
        public static IResult ErrorAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,string outputMessage = "")
        {
            l.LogError(errorMessage);
            return ResultFactory.Fail(i, errorMessage, null, outputMessage);
        }

        /// <summary>
        /// Log error and return IResult fail
        /// </summary>
        /// <param name="l">logger</param>
        /// <param name="i">identifier</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="eventId">event id</param>
        /// <param name="outputMessage">output message</param>
        /// <returns>Iresult on fail</returns>
        [NotNull]
        public static IResult ErrorAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId? eventId = null, string outputMessage = "")
        {
            l.LogError(errorMessage);
            return ResultFactory.Fail(i, errorMessage, eventId, outputMessage);
        }

        /// <summary>
        /// Log error and return IResult fail
        /// </summary>
        /// <param name="l">logger</param>
        /// <param name="i">identifier</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="eventId">event id</param>
        /// <param name="outputMessage">output message</param>
        /// <returns>Iresult on fail</returns>
        [NotNull]
        public static IResult ErrorAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId eventId, string outputMessage = "")
        {
            l.LogError(errorMessage);
            return ResultFactory.Fail(i, errorMessage, eventId, outputMessage);
        }

        /// <summary>
        /// Log error and return IResult fail from fluent validation result
        /// </summary>
        /// <param name="l">logger</param>
        /// <param name="i">identifier</param>

        /// <param name="fluentValidationResult">fluent validation result</param>
        /// <returns>Iresult on fail</returns>
        [NotNull]
        public static IResult ErrorAndReturnFail(this ILogger l, [NotNull] IIdentifier i,[NotNull] ValidationResult fluentValidationResult)
        {
            StringBuilder sb = PrepareValidationFailures(fluentValidationResult, null, out var errors);
            l.LogError(sb.ToString());
            return ResultFactory.Fail(i, errors);
        }

        /// <summary>
        /// Log critical and return IResult fail from fluent validation result
        /// </summary>
        /// <param name="l">logger</param>
        /// <param name="i">identifier</param>
        /// <param name="fluentValidationResult">fluent validation result</param>
        /// <param name="eventId">event id</param>
        /// <returns>Iresult on fail</returns>
        [NotNull]
        public static IResult CriticalAndReturnFail(this ILogger l, [NotNull] IIdentifier i,[NotNull] ValidationResult fluentValidationResult,EventId? eventId = null)
        {
            StringBuilder sb = PrepareValidationFailures(fluentValidationResult, eventId, out var errors);
            l.LogCritical(sb.ToString());
            return ResultFactory.Fail(i, errors);
        }

        /// <summary>Criticals the and return fail.</summary>
        /// <param name="l">The logger.</param>
        /// <param name="i">The identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <returns>IResult fail</returns>
        [NotNull]
        public static IResult CriticalAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,string outputMessage = "")
        {
            l.LogCritical(errorMessage);
            return ResultFactory.Fail(i, errorMessage, null, outputMessage);
        }

        /// <summary>Criticals the and return fail.</summary>
        /// <param name="l">The logger.</param>
        /// <param name="i">The identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <returns>IResult</returns>
        [NotNull]
        public static IResult CriticalAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId? eventId = null, string outputMessage = "")
        {
            l.LogCritical(errorMessage);
            return ResultFactory.Fail(i, errorMessage, eventId, outputMessage);
        }

        /// <summary>Criticals the and return fail.</summary>
        /// <param name="l">The logger.</param>
        /// <param name="i">The identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <returns></returns>
        [NotNull]
        public static IResult CriticalAndReturnFail(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId eventId, string outputMessage = "")
        {
            l.LogCritical(errorMessage);
            return ResultFactory.Fail(i, errorMessage, eventId, outputMessage);
        }

        #endregion

        #region IResult<T>

        /// <summary>
        /// Log Error and return fail
        /// </summary>
        /// <param name="l">the logger</param>
        /// <param name="i">the identifier</param>
        /// <param name="error">error</param>
        /// <typeparam name="T">type of result</typeparam>
        /// <returns>Result fail</returns>
        [NotNull]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,[NotNull] IError error)
        {
           LogOnlyError(l, error);
            return ResultFactory.Fail<T>(i, error);
        }

        /// <summary>Errors the and return fail.</summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="l">The logger.</param>
        /// <param name="i">The i.</param>
        /// <param name="fluentValidationResult">The fluent validation result.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        [NotNull]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,[NotNull] ValidationResult fluentValidationResult,EventId? eventId = null)
        {
            StringBuilder sb = PrepareValidationFailures(fluentValidationResult, eventId, out var errors);
            l.LogError(sb.ToString());
            return ResultFactory.Fail<T>(i, errors);
        }

        /// <summary>Errors the and return fail.</summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="l">The logger.</param>
        /// <param name="i">The identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <returns></returns>
        [NotNull]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId? eventId = null, string outputMessage = "")
        {
            l.LogError(errorMessage);
            return ResultFactory.Fail<T>(i, errorMessage, outputMessage, eventId);
        }

        /// <summary>Errors the and return fail.</summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="l">The logger.</param>
        /// <param name="i">The identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <returns></returns>
        [NotNull]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId eventId , string outputMessage = "")
        {
            l.LogError(errorMessage);
            return ResultFactory.Fail<T>(i, errorMessage, outputMessage, eventId);
        }

        /// <summary>Criticals the and return fail.</summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="l">The logger.</param>
        /// <param name="i">The identifier.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        [NotNull]
        public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,[NotNull] IError error)
        {
            l.LogOnlyCritical(error);
            return ResultFactory.Fail<T>(i, error);
        }

        /// <summary>Criticals the and return fail.</summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="l">The logger.</param>
        /// <param name="i">The identifier.</param>
        /// <param name="fluentValidationResult">The fluent validation result.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        [NotNull]
        public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,[NotNull] ValidationResult fluentValidationResult,EventId? eventId = null)
        {
            StringBuilder sb = PrepareValidationFailures(fluentValidationResult, eventId, out var errors);
            l.LogCritical(sb.ToString());
            return ResultFactory.Fail<T>(i, errors);
        }

        /// <summary>Criticals the and return fail.</summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="l">The logger.</param>
        /// <param name="i">The identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <returns></returns>
        [NotNull]
        public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId? eventId = null, string outputMessage = "")
        {
            l.LogCritical(errorMessage);
            return ResultFactory.Fail<T>(i, errorMessage, outputMessage, eventId);
        }

        /// <summary>Criticals the and return fail.</summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="l">The logger.</param>
        /// <param name="i">The identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <returns></returns>
        [NotNull]
        public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i, [NotNull] string errorMessage,EventId eventId, string outputMessage = "")
        {
            l.LogCritical(errorMessage);
            return ResultFactory.Fail<T>(i, errorMessage, outputMessage, eventId);
        }

        #endregion


        #region private methods

        [NotNull]
        internal static StringBuilder PrepareValidationFailures([NotNull] ValidationResult fluentValidationResult,
                                                               EventId? eventId, [NotNull] out Error[] errors)
        {
            StringBuilder sb   = new StringBuilder();
            var           errs = new List<Error>();
            foreach (var failure in fluentValidationResult.Errors)
            {
                string msg = $"{failure.ErrorMessage}; ";
                if (!StringExtensions.IsNullString(failure.PropertyName) && failure.PropertyName != "*")
                {
                    msg = $"PropertyName '{failure.PropertyName}' - ErrorMessage '{failure.ErrorMessage}'; ";
                }

                sb.Append(msg);
                errs.Add(Error.Parse(msg, eventId));
            }

            errors = errs.ToArray();
            return sb;
        }

        #endregion
    }
}