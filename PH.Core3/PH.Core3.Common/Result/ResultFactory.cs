using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// Begin a chain of functions that accept an incoming <see cref="IResult{TContent}"/>  as argument
        ///
        /// <para>Use <see cref="LazyEvaluator{T}.Resolve"/> to resolve chain of functions</para>
        ///
        ///<para>
        ///<example>
        /// Example:
        /// <code>
        ///var lastResult = ResultFactory.Chain(() => ResultFactory.Ok(new Identifier("some Id"), 7))
        ///                             .Next(r => ResultFactory.Ok(r.Identifier,DateTime.UtcNow.AddDays(r.Content)))
        ///                             .Next(r => ResultFactory.Ok(r.Identifier,$"Added 7 days to '{DateTime.Now:D}': '{r.Content:D}' "))
        ///                             .Resolve();
        /// if (lastResult.OnError)
        /// {
        ///     //...
        /// }
        /// else
        /// {
        ///     //...
        /// }
        /// </code>
        /// </example>
        /// </para>
        /// 
        /// <para>
        /// see <see cref="IResult{TContent}"/>
        /// </para>
        /// <para>
        /// see <see cref="LazyEvaluator{T}"/>
        /// </para>
        /// <para>
        /// see <see cref="LazyEvaluatedError"/>
        /// </para>
        /// 
        /// 
        /// </summary>
        /// <typeparam name="TContent">Type of content result</typeparam>
        /// <param name="fnc">function to lazy evaluate</param>
        /// <returns>Result</returns>
        [NotNull]
        public static LazyEvaluator<TContent> Chain<TContent>([NotNull] IIdentifier identifier ,[NotNull] Func<IResult<TContent>> fnc
                                                              ,[CanBeNull] Func<IResult<TContent>,IResult<TContent>> onErrorFunc = null)
        {
            return new LazyEvaluator<TContent>(fnc, identifier,onErrorFunc);
        }

        /// <summary>
        /// Begin a chain of async functions that accept an incoming <see cref="IResult{TContent}"/>  as argument
        /// 
        ///<para>Use <see cref="LazyEvaluator{T}.Resolve"/> to resolve chain of functions</para>
        ///
        /// <example>
        /// Example:
        ///<code>
        /// var chain = await ResultFactory.Chain(async () => await SomeMethodTestAsync(ResultFactory.Ok&lt;int&lt;(new Identifier("wer"), 1)))
        ///             .Next(async result => await SomeOtherMethodTestAsync(result))
        ///             .Next(async result => await SomeOtherMethod2TestAsync(result))
        ///             .Next(async result => await MethodTestAsync(result))
        ///             .ResolveAsync();
        ///
        /// if(chain.OnError)
        /// {
        ///     //...
        /// }
        /// 
        /// </code>
        /// </example>
        /// <para>
        /// see <see cref="IResult{TContent}"/>
        /// </para>
        /// <para>
        /// see <see cref="LazyEvaluatorAsync{T}"/>
        /// </para>
        /// <para>
        /// see <see cref="LazyEvaluatedError"/>
        /// </para>
        /// </summary>
        /// <typeparam name="TContent">Type of content result</typeparam>
        /// <param name="asyncFnc">async function to lazy evaluate</param>
        /// <returns>Result</returns>
        [NotNull]
        public static LazyEvaluatorAsync<TContent> ChainAsync<TContent>([NotNull] IIdentifier identifier ,Func<Task<IResult<TContent>>> asyncFnc 
                                                                   ,[CanBeNull] Func<IResult<TContent>, Task<IResult<TContent>>> onErrorFunc = null)
        {
            return new LazyEvaluatorAsync<TContent>(asyncFnc, identifier, onErrorFunc);
        }

        public static IResult<T> FailFromException<T>([NotNull] IIdentifier identifier, Exception ex, EventId? eventId = null, string errorMessage = null, string outputMessage = null)
        {
            var msg = string.IsNullOrEmpty(errorMessage) ? ex.Message : errorMessage;
            var err = new Error(msg, outputMessage, eventId) {Exception = ex};
            return new Result<T>(identifier, new []{ err });
           
        }



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

        
        /// <summary>
        /// Return an instance of Result with a Not Found content (NULL): this is intended as not error: a "good empty result"
        /// </summary>
        /// <typeparam name="TContent">Type of not found object</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <returns>Good result</returns>
        [NotNull]
        public static IResult NotFoundOk<TContent>([NotNull] IIdentifier identifier)
        {
            return new ResultNotFound<TContent>(identifier);
        }

        /// <summary>
        /// Return an instance of Result with a Not Found content (NULL): this is intended as error: a "bad empty result"
        /// </summary>
        /// <typeparam name="TContent">Type of not found object</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <param name="errors">errors </param>
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
        /// <param name="progrId">Int id of LazyEvaluator</param>
        /// <param name="identifier">Identifier</param>
        /// <param name="errors">errors</param>
        /// <returns>bad result</returns>
        [NotNull]
        internal static IResult<TContent> FailLazyEvaluatedFunction<TContent>(int progrId,[NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
        {
            var l = new List<LazyEvaluatedError>();
            foreach (var error in errors)
            {
                LazyEvaluatedError err = error as LazyEvaluatedError 
                                         ?? new LazyEvaluatedError(progrId, error.ErrorMessage, error.OutputMessage, error.ErrorEventId, error.InnerError);
                l.Add(err);
            }

            return new Result<TContent>(identifier, l.OrderBy(x => x.ProgrId).ToArray());
        }

        /// <summary>
        /// Bad Result
        /// </summary>
        /// <typeparam name="TContent">Type of object on error</typeparam>
        /// <param name="progrId">Int id of LazyEvaluator</param>
        /// <param name="identifier">Identifier</param>
        /// <param name="exception"></param>
        /// <returns>bad result</returns>
        [NotNull]
        internal static IResult<TContent> FailLazyEvaluatedFunctionFromException<TContent>(int progrId,[NotNull] IIdentifier identifier
                                                                                           , Exception exception, EventId? eventId = null, string errorMessage = null, string outputMessage = null)
        {
            
            var msg = string.IsNullOrEmpty(errorMessage) ? exception.Message : errorMessage;
            var err = new LazyEvaluatedError(progrId,msg, outputMessage, eventId) {Exception = exception};


            return new Result<TContent>(identifier, new[] {err});
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