using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Extensions;

namespace PH.Core3.Common.Result
{
    /// <summary>
    /// Static Factory class for init Result 
    /// </summary>
    public static class ResultFactory
    {
        /// <summary>Ok result</summary>
        /// <typeparam name="T">Type of Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns>Result OK</returns>
        public static IResult<T> Ok<T>([NotNull] IIdentifier identifier, [NotNull] T content)
        {
            return new Result<T>(identifier, content);
        }

        /// <summary>Boolean Ok result</summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>Result OK</returns>
        public static IResult<bool> TrueResult([NotNull] IIdentifier identifier)
            => Ok(identifier, true);

        /// <summary>Boolean Fail result</summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="error">The error.</param>
        /// <returns>Result FAIL</returns>
        public static IResult<bool> FalseResult([NotNull] IIdentifier identifier, IError error)
            => Fail<bool>(identifier, false, error);


        /// <summary>Fail Result</summary>
        /// <typeparam name="T">Type of the Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="error">The error.</param>
        /// <returns>Result FAIL</returns>
        public static IResult<T> Fail<T>([NotNull] IIdentifier identifier, [NotNull] IError error)
        {
            return new Result<T>(identifier,error);
        }

        /// <summary>Fail result</summary>
        /// <typeparam name="T">Type of the Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <returns>Result FAIL</returns>
        public static IResult<T> Fail<T>([NotNull] IIdentifier identifier, [NotNull] Exception exception
            ,[CanBeNull] EventId? eventId = null)
            => Fail<T>(identifier, Error.FromException(exception, eventId));


        /// <summary>Fail result</summary>
        ///  <typeparam name="T">Type of the Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="message">The message.</param>
        /// <returns>Result FAIL</returns>
        public static IResult<T> Fail<T>([NotNull] IIdentifier identifier, string message)
            => Fail<T>(identifier, new Error(message));

        /// <summary>Fail result</summary>
        ///  <typeparam name="T">Type of the Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        public static IResult<T> Fail<T>([NotNull] IIdentifier identifier, string message, EventId eventId)
            => Fail<T>(identifier, new Error(message, eventId));


        /// <summary>Fails result</summary>
        ///  <typeparam name="T">Type of the Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <returns></returns>
        public static IResult<T> Fail<T>([NotNull] IIdentifier identifier, string message, IError inner)
            => Fail<T>(identifier, new Error(message, inner));

        /// <summary>Fail result</summary>
        ///  <typeparam name="T">Type of the Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="inner">The inner.</param>
        /// <returns></returns>
        public static IResult<T> Fail<T>([NotNull] IIdentifier identifier, string message, EventId eventId, IError inner)
            => Fail<T>(identifier, new Error(message, eventId, inner));



        /// <summary>Fail result</summary>
        ///  <typeparam name="T">Type of the Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public static IResult<T> Fail<T>([NotNull] IIdentifier identifier, [CanBeNull] T content
            , [NotNull] IError error)
        {
            return new Result<T>(identifier, content,error);
        }

        /// <summary>Fail result</summary>
        ///  <typeparam name="T">Type of the Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static IResult<T> Fail<T>([NotNull] IIdentifier identifier, [CanBeNull] T content, string message)
            => Fail<T>(identifier, content, new Error(message));

        /// <summary>Fail result</summary>
        ///  <typeparam name="T">Type of the Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="message">The message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        public static IResult<T> Fail<T>([NotNull] IIdentifier identifier, [CanBeNull] T content, string message, EventId eventId)
            => Fail<T>(identifier, content, new Error(message, eventId));

        /// <summary>Fail result</summary>
        ///  <typeparam name="T">Type of the Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <returns></returns>
        public static IResult<T> Fail<T>([NotNull] IIdentifier identifier, [CanBeNull] T content, string message, IError inner)
            => Fail<T>(identifier, content, new Error(message, inner));

        /// <summary>Fail result</summary>
        ///  <typeparam name="T">Type of the Content</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="message">The message.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="inner">The inner.</param>
        /// <returns></returns>
        public static IResult<T> Fail<T>([NotNull] IIdentifier identifier,[CanBeNull] T content, string message, EventId eventId, IError inner)
            => Fail<T>(identifier, content, new Error(message, eventId, inner));


        #region OLD

        
        /// <summary>Chains the asynchronous.</summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="asyncFnc">The asynchronous FNC.</param>
        /// <param name="onErrorFunc">The on error function.</param>
        /// <returns></returns>
        [NotNull]
        public static LazyEvaluatorAsync<TContent> ChainAsync<TContent>([NotNull] IIdentifier identifier ,Func<Task<IResult<TContent>>> asyncFnc 
                                                                        ,[CanBeNull] Func<IResult<TContent>, Task<IResult<TContent>>> onErrorFunc = null)
        {
            return new LazyEvaluatorAsync<TContent>(identifier, asyncFnc, onErrorFunc);
        }

        

        /// <summary>Fails from exception.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <returns></returns>
        [NotNull]
        public static IResult<T> FailFromException<T>([NotNull] IIdentifier identifier, Exception ex, EventId? eventId = null, [CanBeNull] string errorMessage = null, [CanBeNull] string outputMessage = null)
        {
            //var msg = string.IsNullOrEmpty(errorMessage) ? ex.Message : errorMessage;
            //var err = new Error(msg, outputMessage, eventId) {Exception = ex};
            //return new Result<T>(identifier, new []{ err });
            var err = Error.FromException(ex, eventId);

            err.OutputMessage = outputMessage;

            return new Result<T>(identifier, err);


        }

        /// <summary>
        /// Bad Result
        /// </summary>
        /// <typeparam name="TContent">Type of object on error</typeparam>
        /// <param name="progrId">Int id of LazyEvaluator</param>
        /// <param name="identifier">Identifier</param>
        /// <param name="error">errors</param>
        /// <returns>bad result</returns>
        [NotNull]
        internal static IResult<TContent> FailLazyEvaluatedFunction<TContent>(int progrId,[NotNull] IIdentifier identifier, [NotNull] IError error)
        {
            //var l = new List<LazyEvaluatedError>();
            //foreach (var error in errors)
            //{
            //    LazyEvaluatedError err = error as LazyEvaluatedError 
            //                             ?? new LazyEvaluatedError(progrId, error.ErrorMessage, error.OutputMessage, error.ErrorEventId, error.InnerError);
            //    l.Add(err);
            //}

            //return Fail<TContent>(identifier, l.OrderBy(x => x.ProgrId).ToArray());
            LazyEvaluatedError err = error as LazyEvaluatedError 
                                     ?? new LazyEvaluatedError(progrId, error.ErrorMessage, error.OutputMessage, error.ErrorEventId, error.InnerError);
            return Fail<TContent>(identifier, err);
        }

        /// <summary>Fails the lazy evaluated function from exception.</summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="progrId">The progr identifier.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <returns></returns>
        [NotNull]
        internal static IResult<TContent> FailLazyEvaluatedFunctionFromException<TContent>(int progrId,[NotNull] IIdentifier identifier
                                                                                           , Exception exception, EventId? eventId = null, [CanBeNull] string errorMessage = null, [CanBeNull] string outputMessage = null)
        {
            
            var msg = string.IsNullOrEmpty(errorMessage) ? exception.Message : errorMessage;
            var err0 = Error.FromException(exception, eventId);
            var err = new LazyEvaluatedError(progrId,msg, outputMessage, eventId, err0);

            return Fail<TContent>(identifier, err);
        }


        #endregion
        
    }

   
    ///// <summary>
    ///// Factory methods class for <see cref="IResult"/>
    ///// </summary>
    //public static class ResultFactory
    //{

        




    //    ///// <summary>Chains the asynchronous.</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="asyncFnc">The asynchronous FNC.</param>
    //    ///// <param name="onErrorFunc">The on error function.</param>
    //    ///// <returns></returns>
    //    //[NotNull]
    //    //public static LazyEvaluatorAsync<TContent> ChainAsync<TContent>([NotNull] IIdentifier identifier ,Func<Task<IResult<TContent>>> asyncFnc 
    //    //                                                                ,[CanBeNull] Func<IResult<TContent>, Task<IResult<TContent>>> onErrorFunc = null)
    //    //{
    //    //    return new LazyEvaluatorAsync<TContent>(identifier, asyncFnc, onErrorFunc);
    //    //}

        

    //    ///// <summary>Fails from exception.</summary>
    //    ///// <typeparam name="T"></typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="ex">The ex.</param>
    //    ///// <param name="eventId">The event identifier.</param>
    //    ///// <param name="errorMessage">The error message.</param>
    //    ///// <param name="outputMessage">The output message.</param>
    //    ///// <returns></returns>
    //    //[NotNull]
    //    //public static IResult<T> FailFromException<T>([NotNull] IIdentifier identifier, Exception ex, EventId? eventId = null, [CanBeNull] string errorMessage = null, [CanBeNull] string outputMessage = null)
    //    //{
    //    //    var msg = string.IsNullOrEmpty(errorMessage) ? ex.Message : errorMessage;
    //    //    var err = new Error(msg, outputMessage, eventId) {Exception = ex};
    //    //    return new Result<T>(identifier, new []{ err });
           
    //    //}

    //    ///// <summary>
    //    ///// Evaluate a content and wrap into <see cref="IResult{TContent}"/>
    //    ///// </summary>
    //    ///// <typeparam name="T">Type of content</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="content">content instance or null</param>
    //    ///// <returns>IResult</returns>
    //    //[NotNull]
    //    //public static IResult<T> Eval<T>([NotNull] IIdentifier identifier, [CanBeNull] T content)
    //    //{
    //    //    if (null == content)
    //    //    {
    //    //        return Fail<T>(identifier, "null object");
    //    //    }

    //    //    return Ok(identifier, content);
    //    //}


    //    ///// <summary>
    //    ///// True Result
    //    ///// </summary>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <returns>Ok Result with True on Content</returns>
    //    //[NotNull]
    //    //public static IResult True([NotNull] IIdentifier identifier)
    //    //{
    //    //    return new ResultEmpty(identifier);
    //    //}

    //    ///// <summary>
    //    ///// True Result
    //    ///// </summary>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <returns>Ok Result with True on Content</returns>
    //    //[NotNull]
    //    //public static IResult Ok([NotNull] IIdentifier identifier)
    //    //{
    //    //    return new ResultEmpty(identifier);
    //    //}

    //    ///// <summary>
    //    ///// Ok Result
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of Content</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="content">result of operation</param>
    //    ///// <returns>Good result</returns>
    //    //[NotNull]
    //    //public static IResult<TContent> Ok<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content)
    //    //{
    //    //    //if (content is Array)
    //    //    //    return PagedOk<TContent>(identifier, content, -1, -1, -1);

    //    //    return new Result<TContent>(identifier, content);
    //    //}


    //    ///// <summary>Ok Json Result</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="content">The content.</param>
    //    ///// <param name="json">The json.</param>
    //    ///// <returns></returns>
    //    //public static IJsonResult<TContent> Ok<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content,
    //    //                                                 [NotNull] string json)
    //    //{
    //    //    return new JsonResult<TContent>(identifier, content, json);
    //    //}


    //    ///// <summary>Ok result for paged content </summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="content">The content.</param>
    //    ///// <param name="count">The count.</param>
    //    ///// <param name="pageNumber">The page number.</param>
    //    ///// <param name="pageSize">Size of the page.</param>
    //    ///// <returns></returns>
    //    //[NotNull]
    //    //public static IPagedResult<TContent> PagedOk<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent[] content, long count, int pageNumber, int pageSize)
    //    //{
    //    //    return new PagedResult<TContent>(identifier, content, count, pageNumber, pageSize);
    //    //}

    //    ///// <summary>Empty result for paged content. The result is not on error</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <returns></returns>
    //    //[NotNull]
    //    //public static IPagedResult<TContent> PagedEmpty<TContent>([NotNull] IIdentifier identifier)
    //    //{
    //    //    return new PagedResult<TContent>(identifier, new TContent[0], 0, -1, -1);
    //    //}

    //    ///// <summary>Fail result for paged content</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="errors">The errors.</param>
    //    ///// <returns></returns>
    //    //[NotNull]
    //    //public static IPagedResult<TContent> PagedFail<TContent>([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
    //    //{
    //    //    return new PagedResult<TContent>(identifier, errors);
    //    //}




    //    ///// <summary>
    //    ///// Return an instance of Result with a Not Found content (NULL): this is intended as not error: a "good empty result"
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of not found object</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <returns>Good result</returns>
    //    //[NotNull]
    //    //public static IResult<TContent> NotFoundOk<TContent>([NotNull] IIdentifier identifier)
    //    //{
    //    //    return new ResultNotFound<TContent>(identifier);
    //    //}

    //    ///// <summary>
    //    ///// Return an instance of Result with a Not Found content (NULL): this is intended as error: a "bad empty result"
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of not found object</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="errors">errors </param>
    //    ///// <returns>Bad Result</returns>
    //    //[NotNull]
    //    //public static IResult<TContent> NotFound<TContent>([NotNull] IIdentifier identifier,[NotNull] IEnumerable<IError> errors)
    //    //{
    //    //    return new ResultNotFound<TContent>(identifier,errors);
    //    //}

    //    ///// <summary>Return an instance of Result with a Not Found content (NULL): this is intended as error: a "bad empty result"</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="error">The error.</param>
    //    ///// <returns>Bad Result</returns>
    //    //[NotNull]
    //    //public static IResult<TContent> NotFound<TContent>([NotNull] IIdentifier identifier,[NotNull] IError error)
    //    //{
    //    //    return new ResultNotFound<TContent>(identifier,new IError[]{error});
    //    //}

    //    ///// <summary>
    //    ///// Bad Result
    //    ///// </summary>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="errors">errors</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //public static IResult Fail([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
    //    //{
    //    //    return new ResultEmpty(identifier, errors);
    //    //}

    //    ///// <summary>
    //    ///// Bad Result
    //    ///// </summary>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="error">error</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //public static IResult Fail([NotNull] IIdentifier identifier, [NotNull] IError error)
    //    //{
    //    //    return new ResultEmpty(identifier, new []{error});
    //    //}

    //    ///// <summary>
    //    ///// Bad Result
    //    ///// </summary>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="errorMessage">error message</param>
    //    ///// <param name="eventId">Event Id</param>
    //    ///// <param name="outputMessage">Output message</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //public static IResult Fail([NotNull] IIdentifier identifier, [NotNull] string errorMessage,EventId? eventId = null,
    //    //                           string outputMessage = "")
    //    //{
    //    //    return new ResultEmpty(identifier, new[] {new Error(errorMessage,outputMessage,eventId)});
    //    //}

    //    ///// <summary>
    //    ///// Bad Result
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of object on error</typeparam>
    //    ///// <param name="progrId">Int id of LazyEvaluator</param>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="error">errors</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //internal static IResult<TContent> FailLazyEvaluatedFunction<TContent>(int progrId,[NotNull] IIdentifier identifier, [NotNull] IError error)
    //    //{
    //    //    //var l = new List<LazyEvaluatedError>();
    //    //    //foreach (var error in errors)
    //    //    //{
    //    //    //    LazyEvaluatedError err = error as LazyEvaluatedError 
    //    //    //                             ?? new LazyEvaluatedError(progrId, error.ErrorMessage, error.OutputMessage, error.ErrorEventId, error.InnerError);
    //    //    //    l.Add(err);
    //    //    //}

    //    //    //return Fail<TContent>(identifier, l.OrderBy(x => x.ProgrId).ToArray());
    //    //    LazyEvaluatedError err = error as LazyEvaluatedError 
    //    //                             ?? new LazyEvaluatedError(progrId, error.ErrorMessage, error.OutputMessage, error.ErrorEventId, error.InnerError);
    //    //    return Fail<TContent>(identifier, err);
    //    //}

    //    ///// <summary>Fails the lazy evaluated function from exception.</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="progrId">The progr identifier.</param>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="exception">The exception.</param>
    //    ///// <param name="eventId">The event identifier.</param>
    //    ///// <param name="errorMessage">The error message.</param>
    //    ///// <param name="outputMessage">The output message.</param>
    //    ///// <returns></returns>
    //    //[NotNull]
    //    //internal static IResult<TContent> FailLazyEvaluatedFunctionFromException<TContent>(int progrId,[NotNull] IIdentifier identifier
    //    //                                                                                   , Exception exception, EventId? eventId = null, [CanBeNull] string errorMessage = null, [CanBeNull] string outputMessage = null)
    //    //{
            
    //    //    var msg = string.IsNullOrEmpty(errorMessage) ? exception.Message : errorMessage;
    //    //    var err = new LazyEvaluatedError(progrId,msg, outputMessage, eventId) {Exception = exception};


    //    //    //return new Result<TContent>(identifier, new[] {err});
    //    //    return Fail<TContent>(identifier, new[] {err});
    //    //}


    //    ///// <summary>Bad Result with Content</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="content">The content.</param>
    //    ///// <param name="errors">The errors.</param>
    //    ///// <returns>Bad Result with Content</returns>
    //    //[NotNull]
    //    //public static IResultErrorWithContent<TContent> FailWithContent<TContent>([NotNull] IIdentifier identifier,[NotNull] TContent content, [NotNull] IEnumerable<IError> errors)
    //    //{
    //    //    return new ResultErrorWithContent<TContent>(content, errors.ToList(), identifier);
    //    //}

    //    ///// <summary>Bad Result with Content</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="content">The content.</param>
    //    ///// <param name="error">The error.</param>
    //    ///// <returns>Bad Result with Content</returns>
    //    //[NotNull]
    //    //public static IResultErrorWithContent<TContent> FailWithContent<TContent>([NotNull] IIdentifier identifier,[NotNull] TContent content, [NotNull] IError error)
    //    //{
    //    //    return FailWithContent(identifier, content, new List<IError>() {error});
    //    //}

    //    ///// <summary>Fails the content of the with.</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="content">The content.</param>
    //    ///// <param name="errorMessage">The error message.</param>
    //    ///// <returns>Bad Result with Content</returns>
    //    //[NotNull]
    //    //public static IResultErrorWithContent<TContent> FailWithContent<TContent>([NotNull] IIdentifier identifier,[NotNull] TContent content, [NotNull] string errorMessage)
    //    //{
    //    //    return FailWithContent(identifier, content, new List<IError>() {new Error(errorMessage)});
    //    //}




    //    ///// <summary>
    //    ///// Bad Result
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of object on error</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="errors">errors</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
    //    //{
    //    //    return new Result<TContent>(identifier, errors);
    //    //}

    //    ///// <summary>
    //    ///// Bad Json Result
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of object on error</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="errors">errors</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
    //    //{
    //    //    return new JsonResult<TContent>(identifier, errors);
    //    //}

    //    ///// <summary>Return Bad Result with content.</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="content">The content.</param>
    //    ///// <param name="errors">The errors.</param>
    //    ///// <returns>bad result with content</returns>
    //    //[NotNull]
    //    //public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content,
    //    //                                               [NotNull] IEnumerable<IError> errors)
    //    //{
    //    //    return new Result<TContent>(content,errors.ToList(), identifier);
    //    //}


    //    ///// <summary>Return Bad Result with content.</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="content">The content.</param>
    //    ///// <param name="json">The json content</param>
    //    ///// <param name="errors">The errors.</param>
    //    ///// <returns>bad result with content</returns>
    //    //[NotNull]
    //    //public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content,string json,
    //    //                                               [NotNull] IEnumerable<IError> errors)
    //    //{
    //    //    return new JsonResult<TContent>(content,errors.ToList(), identifier,json);
    //    //}


    //    ///// <summary>Return Bad Result with content.</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="content">The content.</param>
    //    ///// <param name="error">The error.</param>
    //    ///// <returns></returns>
    //    //[NotNull]
    //    //public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content, [NotNull] IError error)
    //    //{
    //    //    //return new Result<TContent>(identifier, new []{error});
    //    //    return Fail<TContent>(identifier, content,new[] {error} );
    //    //}

    //    ///// <summary>Return Bad Result with content.</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="content">The content.</param>
    //    ///// <param name="json">The json content</param>
    //    ///// <param name="error">The error.</param>
    //    ///// <returns></returns>
    //    //[NotNull]
    //    //public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content, string json, [NotNull] IError error)
    //    //{
    //    //    //return new Result<TContent>(identifier, new []{error});
    //    //    return FailJson<TContent>(identifier, content, json,new[] {error} );
    //    //}


    //    ///// <summary>Return Bad Result.</summary>
    //    ///// <typeparam name="TContent">The type of the content.</typeparam>
    //    ///// <param name="identifier">The identifier.</param>
    //    ///// <param name="fluentValidationResult">The fluent validation result.</param>
    //    ///// <param name="eventId">The event identifier.</param>
    //    ///// <returns></returns>
    //    //public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] ValidationResult fluentValidationResult,EventId? eventId = null)
    //    //{
    //    //    StringBuilder sb = LoggingExtensions.PrepareValidationFailures(fluentValidationResult, eventId, out var errors);
    //    //    return ResultFactory.Fail<TContent>(identifier, errors);
    //    //}

    //    ///// <summary>
    //    ///// Bad Result
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of object on error</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="error">error</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] IError error)
    //    //{
    //    //    //return new Result<TContent>(identifier, new []{error});
    //    //    return Fail<TContent>(identifier, new[] {error});
    //    //}


    //    ///// <summary>
    //    ///// Bad Json Result
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of object on error</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="error">error</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, [NotNull] IError error)
    //    //{
    //    //    //return new Result<TContent>(identifier, new []{error});
    //    //    return FailJson<TContent>(identifier, new[] {error});
    //    //}


    //    ///// <summary>
    //    ///// Bad Result
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of object on error</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="errorMessage">error message</param>
    //    ///// <param name="eventId">Event Id</param>
    //    ///// <param name="outputMessage">Output message</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] string errorMessage,
    //    //                                               string outputMessage = "",EventId? eventId = null)
    //    //{
    //    //    //return new Result<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) });
    //    //    return Fail<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) });
    //    //}
        
    //    ///// <summary>
    //    ///// Bad Json Result
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of object on error</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="errorMessage">error message</param>
    //    ///// <param name="eventId">Event Id</param>
    //    ///// <param name="outputMessage">Output message</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, [NotNull] string errorMessage,
    //    //                                               string outputMessage = "",EventId? eventId = null)
    //    //{
    //    //    return FailJson<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) });
    //    //}

    //    ///// <summary>
    //    ///// Bad Result
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of object on error</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="errorMessage">error message</param>
    //    ///// <param name="eventId">Event Id</param>
    //    ///// <param name="outputMessage">Output message</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, EventId eventId, [NotNull] string errorMessage,
    //    //                                               string outputMessage = "")
    //    //{
    //    //    return Fail<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) });
    //    //}

    //    ///// <summary>
    //    ///// Bad Json Result
    //    ///// </summary>
    //    ///// <typeparam name="TContent">Type of object on error</typeparam>
    //    ///// <param name="identifier">Identifier</param>
    //    ///// <param name="errorMessage">error message</param>
    //    ///// <param name="eventId">Event Id</param>
    //    ///// <param name="outputMessage">Output message</param>
    //    ///// <returns>bad result</returns>
    //    //[NotNull]
    //    //public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, EventId eventId, [NotNull] string errorMessage,
    //    //                                               string outputMessage = "")
    //    //{
    //    //    return FailJson<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) });
    //    //}

    //}


}