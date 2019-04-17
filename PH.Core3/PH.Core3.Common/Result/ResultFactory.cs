using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace PH.Core3.Common.Result
{
    /// <summary>
    /// Factory methods class for <see cref="IResult"/>
    /// </summary>
    public static class ResultFactory
    {
        /// <summary>
        /// True Result
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <returns>Ok Result with True on Content</returns>
        [NotNull]
        public static IResult True([NotNull] IIdentifier identifier)
        {
            return new ResultEmpty(identifier);
        }

        /// <summary>
        /// True Result
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <returns>Ok Result with True on Content</returns>
        [NotNull]
        public static IResult Ok([NotNull] IIdentifier identifier)
        {
            return new ResultEmpty(identifier);
        }

        /// <summary>
        /// Ok Result
        /// </summary>
        /// <typeparam name="TContent">Type of Content</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <param name="content">result of operation</param>
        /// <returns>Good result</returns>
        [NotNull]
        public static IResult<TContent> Ok<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content)
        {
            return new Result<TContent>(identifier, content);
        }

        //[NotNull]
        //public static IResult<TContent> SwitchResult<TInputContent,TContent>([NotNull] IIdentifier identifier, [NotNull] IResult<TInputContent> content, Func<TInputContent, TContent> transformFunc)
        //{
        //    return new Result<TContent>(identifier, content);
        //}

        /// <summary>
        /// Return an instance of Result with a Not Found content (NULL): this is intended as not error: a "good empty result"
        /// </summary>
        /// <typeparam name="TContent">Type of not found object</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <returns>Good result</returns>
        [NotNull]
        public static IResult NotFoundOK<TContent>([NotNull] IIdentifier identifier)
        {
            return new ResultNotFound<TContent>(identifier);
        }

        /// <summary>
        /// Return an instance of Result with a Not Found content (NULL): this is intended as error: a "bad empty result"
        /// </summary>
        /// <typeparam name="TContent">Type of not found object</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <returns>Bad Result</returns>
        [NotNull]
        public static IResult NotFound<TContent>([NotNull] IIdentifier identifier,[NotNull] IEnumerable<IError> errors)
        {
            return new ResultNotFound<TContent>(identifier,errors);
        }



        /// <summary>
        /// Bad Result
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="errors">errors</param>
        /// <returns>bad result</returns>
        [NotNull]
        public static IResult Fail([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
        {
            return new ResultEmpty(identifier, errors);
        }

        /// <summary>
        /// Bad Result
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="error">error</param>
        /// <returns>bad result</returns>
        [NotNull]
        public static IResult Fail([NotNull] IIdentifier identifier, [NotNull] IError error)
        {
            return new ResultEmpty(identifier, new []{error});
        }

        /// <summary>
        /// Bad Result
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="eventId">Event Id</param>
        /// <param name="outputMessage">Output message</param>
        /// <returns>bad result</returns>
        [NotNull]
        public static IResult Fail([NotNull] IIdentifier identifier, [NotNull] string errorMessage,EventId? eventId = null,
                                   string outputMessage = "")
        {
            return new ResultEmpty(identifier, new[] {new Error(errorMessage,outputMessage,eventId)});
        }

        /// <summary>
        /// Bad Result
        /// </summary>
        /// <typeparam name="TContent">Type of object on error</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <param name="errors">errors</param>
        /// <returns>bad result</returns>
        [NotNull]
        public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
        {
            return new Result<TContent>(identifier, errors);
        }
        
        /// <summary>
        /// Bad Result
        /// </summary>
        /// <typeparam name="TContent">Type of object on error</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <param name="error">error</param>
        /// <returns>bad result</returns>
        [NotNull]
        public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] IError error)
        {
            return new Result<TContent>(identifier, new []{error});
        }


        /// <summary>
        /// Bad Result
        /// </summary>
        /// <typeparam name="TContent">Type of object on error</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="eventId">Event Id</param>
        /// <param name="outputMessage">Output message</param>
        /// <returns>bad result</returns>
        [NotNull]
        public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] string errorMessage,
                                                       string outputMessage = "",EventId? eventId = null)
        {
            return new Result<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) }); 
        }

        /// <summary>
        /// Bad Result
        /// </summary>
        /// <typeparam name="TContent">Type of object on error</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="eventId">Event Id</param>
        /// <param name="outputMessage">Output message</param>
        /// <returns>bad result</returns>
        [NotNull]
        public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, EventId eventId, [NotNull] string errorMessage,
                                                       string outputMessage = "")
        {
            return new Result<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) }); 
        }


    }
}