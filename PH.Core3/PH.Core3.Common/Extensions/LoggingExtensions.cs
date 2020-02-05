using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

using PH.Results;
using PH.Results.Internals;

namespace PH.Core3.Common.Extensions
{
    /// <summary>
    /// Logging useful extensions
    /// </summary>
    public static class LoggingExtensions
    {

        
       
        #region IResult<T>


        /// <summary>Log Error and return fail with content</summary>
        /// <typeparam name="T">Type of the content</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        [NotNull]
        [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
        public static IResult<T> ErrorWithContentAndReturnFail<T>(this ILogger logger, [NotNull] IIdentifier identifier,
                                                                                  [NotNull] T content, [NotNull] string errorMessage)
         => ErrorWithContentAndReturnFail(logger, identifier, content, Error.Instance(errorMessage));
        

        /// <summary>Log Error and return fail with content</summary>
        /// <typeparam name="T">Type of the content</typeparam>
        /// <param name="logger">The logger.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        [NotNull]
        [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
        public static IResult<T> ErrorWithContentAndReturnFail<T>(this ILogger logger, [NotNull] IIdentifier identifier,
                                                                                  [NotNull] T content, [NotNull] IError error)
        {

            logger.LogError(error);
            return ResultFactory.Fail<T>(identifier, content, error);
        }


        /// <summary>Errors the and return fail.</summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="l">The logger</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <returns>Result fail</returns>
        [NotNull]
        [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier identifier, [NotNull] Exception exception, [CanBeNull] string message = "")
        {
            var msg = string.IsNullOrEmpty(message) ? exception.Message : message;
            l.LogError(exception, msg);
            return ResultFactory.Fail<T>(identifier, Error.FromException(exception));
        }


        /// <summary>
        /// Log Error and return fail
        /// </summary>
        /// <param name="l">the logger</param>
        /// <param name="i">the identifier</param>
        /// <param name="error">error</param>
        /// <typeparam name="T">type of result</typeparam>
        /// <returns>Result fail</returns>
        [NotNull]
        [Obsolete("Remove usage and use ResultFactory from PH.Results")]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,[NotNull] IError error)
        {
           l.LogError(error);
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
        [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,[NotNull] ValidationResult fluentValidationResult,EventId? eventId = null)
        {
            StringBuilder sb = PrepareValidationFailures(fluentValidationResult, eventId, out var errors);
            var err = Error.Instance(sb.ToString(), null, eventId);
            

            return ErrorAndReturnFail<T>(l, i, err);
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
        [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,
                                                       [NotNull] string errorMessage, EventId? eventId = null,
                                                       string outputMessage = "")
        {
            var r =(Error)Error.Instance(errorMessage, null, eventId);
            r.OutputMessage = outputMessage;
            return ErrorAndReturnFail<T>(l, i, r);

        }


        /// <summary>Errors the and return fail.</summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="l">The logger.</param>
        /// <param name="i">The identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <returns></returns>
        [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
        public static IResult<T> ErrorAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,
                                                       [NotNull] string errorMessage, EventId eventId,
                                                       string outputMessage = "")
        {
            var r =(Error)Error.Instance(errorMessage, null, eventId);
            r.OutputMessage = outputMessage;
            return ErrorAndReturnFail<T>(l, i, r);
        }
           // => ErrorAndReturnFail<T>(l, i,
            //                         new Error(errorMessage, eventId) {OutputMessage = outputMessage});


        /// <summary>Criticals the and return fail.</summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="l">The logger.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
        public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier identifier, [NotNull] Exception exception, [CanBeNull] string message = "")
        {
            var msg = string.IsNullOrEmpty(message) ? exception.Message : message;
            l.LogCritical(exception, msg);
            return ResultFactory.Fail<T>(identifier, Error.FromException(exception));
        }

        /// <summary>Criticals the and return fail.</summary>
        /// <typeparam name="T">type of result</typeparam>
        /// <param name="l">The logger.</param>
        /// <param name="i">The identifier.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        [NotNull]
        [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
        public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,[NotNull] IError error)
        {
            l.LogCritical(error);
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
        [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
        public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,[NotNull] ValidationResult fluentValidationResult,EventId? eventId = null)
        {
            StringBuilder sb = PrepareValidationFailures(fluentValidationResult, eventId, out var errors);
            var err =  Error.Instance(sb.ToString(), null, eventId) ;
            return CriticalAndReturnFail<T>(l, i, err);
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
        [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
        public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,
                                                          [NotNull] string errorMessage, EventId? eventId = null,
                                                          string outputMessage = "")
        {
            var r =(Error)Error.Instance(errorMessage, null, eventId);
            r.OutputMessage = outputMessage;
            return CriticalAndReturnFail<T>(l, i, r);
        }
          //  => CriticalAndReturnFail<T>(l, i,
           //                             new Error(errorMessage)
             //                               {ErrorEventId = eventId, OutputMessage = outputMessage});

             /// <summary>Criticals the and return fail.</summary>
             /// <typeparam name="T">type of result</typeparam>
             /// <param name="l">The logger.</param>
             /// <param name="i">The identifier.</param>
             /// <param name="errorMessage">The error message.</param>
             /// <param name="eventId">The event identifier.</param>
             /// <param name="outputMessage">The output message.</param>
             /// <returns></returns>
             [NotNull]
             [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
             public static IResult<T> CriticalAndReturnFail<T>(this ILogger l, [NotNull] IIdentifier i,
                                                               [NotNull] string errorMessage, EventId eventId,
                                                               string outputMessage = "")
             {
                 var r =(Error)Error.Instance(errorMessage, null, eventId);
                 r.OutputMessage = outputMessage;
                 return CriticalAndReturnFail<T>(l, i, r);
             }
            //=> CriticalAndReturnFail<T>(l, i,
            //                            new Error(errorMessage,eventId)
            //                                {OutputMessage = outputMessage});

        #endregion


        #region private methods

        [NotNull]
        [Obsolete("Remove usage and use ResultFactory from PH.Results", true)]
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
                //errs.Add(Error.Parse(msg, eventId));
                errs.Add(Error.Instance(msg, null, eventId));
            }

            errors = errs.ToArray();
            return sb;
        }

        #endregion

      
    }
}