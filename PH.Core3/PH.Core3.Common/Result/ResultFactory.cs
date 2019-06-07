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

        


        /*
        ///// <summary>
        ///// Begin a chain of functions that accept an incoming <see cref="IResult{TContent}"/>  as argument
        /////
        ///// <para>Use <see cref="LazyEvaluator{T}.Resolve"/> to resolve chain of functions</para>
        /////
        /////<para>
        /////<example>
        ///// Example:
        ///// <code>
        /////var lastResult = ResultFactory.Chain(() => ResultFactory.Ok(new Identifier("some Id"), 7))
        /////                             .Next(r => ResultFactory.Ok(r.Identifier,DateTime.UtcNow.AddDays(r.Content)))
        /////                             .Next(r => ResultFactory.Ok(r.Identifier,$"Added 7 days to '{DateTime.Now:D}': '{r.Content:D}' "))
        /////                             .Resolve();
        ///// if (lastResult.OnError)
        ///// {
        /////     //...
        ///// }
        ///// else
        ///// {
        /////     //...
        ///// }
        ///// </code>
        ///// </example>
        ///// </para>
        ///// 
        ///// <para>
        ///// see <see cref="IResult{TContent}"/>
        ///// </para>
        ///// <para>
        ///// see <see cref="LazyEvaluator{T}"/>
        ///// </para>
        ///// <para>
        ///// see <see cref="LazyEvaluatedError"/>
        ///// </para>
        ///// 
        ///// 
        ///// </summary>
        ///// <typeparam name="TContent">Type of content result</typeparam>
        ///// <param name="fnc">function to lazy evaluate</param>
        ///// <returns>Result</returns>
        //[NotNull]
        //public static LazyEvaluator<TContent> Chain<TContent>([NotNull] IIdentifier identifier ,[NotNull] Func<IResult<TContent>> fnc
        //                                                      ,[CanBeNull] Func<IResult<TContent>,IResult<TContent>> onErrorFunc = null)
        //{
        //    return new LazyEvaluator<TContent>(fnc, identifier,onErrorFunc);
        //}


        /// <summary>
        /// Begin a chain of async functions that accept an incoming <see cref="IResult{TContent}"/>  as argument
        /// <para>Use <see cref="LazyEvaluatorAsync{T}.ResolveAsync"/> to resolve chain of functions</para><div class="LW_CollapsibleArea_Container"><div class="LW_CollapsibleArea_TitleDiv"><h2 class="LW_CollapsibleArea_Title">Examples<a class="IconAddEncoded" href="add:code"></a><a class="IconDeleteEncoded" href="remove:example"></a></h2><div class="LW_CollapsibleArea_HrDiv"><hr class="LW_CollapsibleArea_Hr"></div></div><a id="exampleSection"><!----></a><div class="summary contenteditable" id="div_example" contentEditable="true">
        ///  Example:
        /// </div><div class="code"><span id="cbc_1" codeLanguage="CSharp" x-lang="CSharp"><div class="highlight-title"><span tabIndex="0" class="highlight-copycode"></span>C# <a title="Edit <c> tag" class="IconEditEncoded" href="edit:code:div_example_e11f4a8fe4594c9aa61c02ca78f7c5ac"></a><a title="Delete <c> tag" class="IconDeleteEncoded" href="remove:code:div_example_e11f4a8fe4594c9aa61c02ca78f7c5ac"></a></div><div id="div_example_e11f4a8fe4594c9aa61c02ca78f7c5ac" name="div_example"><pre xml:space="preserve"><span class="highlight-keyword">var</span> id = <span class="highlight-keyword">new</span> Identifier(...);
        /// <span class="highlight-keyword">var</span> chain = <span class="highlight-keyword">await</span> ResultFactory.ChainAsync(id, <span class="highlight-keyword">async</span> () =&gt; <span class="highlight-keyword">await</span> SomeMethodTestAsync(ResultFactory.Ok&lt;<span class="highlight-keyword">int</span>&lt;(<span class="highlight-keyword">new</span> Identifier("wer"), <span class="highlight-number">1</span>)))
        ///             .Next(<span class="highlight-keyword">async</span> (evaluator,result) =&gt; <span class="highlight-keyword">await</span> SomeOtherMethodTestAsync(result))
        ///             .Next(<span class="highlight-keyword">async</span> (evaluator,result) =&gt; <span class="highlight-keyword">await</span> SomeOtherMethod2TestAsync(result))
        ///             .Next(<span class="highlight-keyword">async</span> (evaluator,result) =&gt; <span class="highlight-keyword">await</span> MethodTestAsync(result))
        ///             .ResolveAsync();
        /// <span class="highlight-keyword">if</span>(chain.OnError)
        /// {
        /// }</pre></div></span></div></div><para>
        ///  see <see cref="IResult{TContent}"/>
        ///  </para><para>
        ///  see <see cref="LazyEvaluatorAsync{T}"/>
        ///  </para><para>
        ///  see <see cref="LazyEvaluatedError"/>
        ///  </para>
        /// </summary>
        /// <typeparam name="TContent">Type of content result</typeparam>
        /// <param name="identifier"></param>
        /// <param name="asyncFnc">async function to lazy evaluate</param>
        /// <param name="onErrorFunc"></param>
        /// <returns>Result</returns>
        /// <example>
        /// Example:
        /// <code>var id = new Identifier(...);
        ///  var chain = await ResultFactory.ChainAsync(id, async () =&gt; await SomeMethodTestAsync(ResultFactory.Ok&lt;int&lt;(new Identifier("wer"), 1)))
        ///              .Next(async (evaluator,result) =&gt; await SomeOtherMethodTestAsync(result))
        ///              .Next(async (evaluator,result) =&gt; await SomeOtherMethod2TestAsync(result))
        ///              .Next(async (evaluator,result) =&gt; await MethodTestAsync(result))
        ///              .ResolveAsync();
        ///
        ///  if(chain.OnError)
        ///  {
        ///      //...
        ///  }</code>
        /// </example>
        */

            /*
        public static async Task<IResult<TOutput>> ApplyAsync<TInput, TOutput>(this IResult<TInput> r, Func<TInput, Task<TOutput>> transform)
        {
            if (r.OnError)
            {
                return ResultFactory.Fail<TOutput>(r.Identifier, r.Errors);
            }
            else
            {
                var internalResult = await transform.Invoke(r.Content);
                return ResultFactory.Ok(r.Identifier, internalResult);
            }
        }
        */


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
            var msg = string.IsNullOrEmpty(errorMessage) ? ex.Message : errorMessage;
            var err = new Error(msg, outputMessage, eventId) {Exception = ex};
            return new Result<T>(identifier, new []{ err });
           
        }

        /// <summary>
        /// Evaluate a content and wrap into <see cref="IResult{TContent}"/>
        /// </summary>
        /// <typeparam name="T">Type of content</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <param name="content">content instance or null</param>
        /// <returns>IResult</returns>
        [NotNull]
        public static IResult<T> Eval<T>([NotNull] IIdentifier identifier, [CanBeNull] T content)
        {
            if (null == content)
            {
                return Fail<T>(identifier, "null object");
            }

            return Ok(identifier, content);
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
            //if (content is Array)
            //    return PagedOk<TContent>(identifier, content, -1, -1, -1);

            return new Result<TContent>(identifier, content);
        }


        /// <summary>Ok Json Result</summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static IJsonResult<TContent> Ok<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content,
                                                         [NotNull] string json)
        {
            return new JsonResult<TContent>(identifier, content, json);
        }


        /// <summary>Ok result for paged content </summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="count">The count.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [NotNull]
        public static IPagedResult<TContent> PagedOk<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent[] content, long count, int pageNumber, int pageSize)
        {
            return new PagedResult<TContent>(identifier, content, count, pageNumber, pageSize);
        }

        /// <summary>Empty result for paged content. The result is not on error</summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        [NotNull]
        public static IPagedResult<TContent> PagedEmpty<TContent>([NotNull] IIdentifier identifier)
        {
            return new PagedResult<TContent>(identifier, new TContent[0], 0, -1, -1);
        }

        /// <summary>Fail result for paged content</summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="errors">The errors.</param>
        /// <returns></returns>
        [NotNull]
        public static IPagedResult<TContent> PagedFail<TContent>([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
        {
            return new PagedResult<TContent>(identifier, errors);
        }




        /// <summary>
        /// Return an instance of Result with a Not Found content (NULL): this is intended as not error: a "good empty result"
        /// </summary>
        /// <typeparam name="TContent">Type of not found object</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <returns>Good result</returns>
        [NotNull]
        public static IResult<TContent> NotFoundOk<TContent>([NotNull] IIdentifier identifier)
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
        public static IResult<TContent> NotFound<TContent>([NotNull] IIdentifier identifier,[NotNull] IEnumerable<IError> errors)
        {
            return new ResultNotFound<TContent>(identifier,errors);
        }

        /// <summary>Return an instance of Result with a Not Found content (NULL): this is intended as error: a "bad empty result"</summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="error">The error.</param>
        /// <returns>Bad Result</returns>
        [NotNull]
        public static IResult<TContent> NotFound<TContent>([NotNull] IIdentifier identifier,[NotNull] IError error)
        {
            return new ResultNotFound<TContent>(identifier,new IError[]{error});
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

            return Fail<TContent>(identifier, l.OrderBy(x => x.ProgrId).ToArray());
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
            var err = new LazyEvaluatedError(progrId,msg, outputMessage, eventId) {Exception = exception};


            //return new Result<TContent>(identifier, new[] {err});
            return Fail<TContent>(identifier, new[] {err});
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
        /// Bad Json Result
        /// </summary>
        /// <typeparam name="TContent">Type of object on error</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <param name="errors">errors</param>
        /// <returns>bad result</returns>
        [NotNull]
        public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
        {
            return new JsonResult<TContent>(identifier, errors);
        }

        /// <summary>Return Bad Result with content.</summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="errors">The errors.</param>
        /// <returns>bad result with content</returns>
        [NotNull]
        public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content,
                                                       [NotNull] IEnumerable<IError> errors)
        {
            return new Result<TContent>(content,errors.ToList(), identifier);
        }


        /// <summary>Return Bad Result with content.</summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="json">The json content</param>
        /// <param name="errors">The errors.</param>
        /// <returns>bad result with content</returns>
        [NotNull]
        public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content,string json,
                                                       [NotNull] IEnumerable<IError> errors)
        {
            return new JsonResult<TContent>(content,errors.ToList(), identifier,json);
        }


        /// <summary>Return Bad Result with content.</summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        [NotNull]
        public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content, [NotNull] IError error)
        {
            //return new Result<TContent>(identifier, new []{error});
            return Fail<TContent>(identifier, content,new[] {error} );
        }

        /// <summary>Return Bad Result with content.</summary>
        /// <typeparam name="TContent">The type of the content.</typeparam>
        /// <param name="identifier">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <param name="json">The json content</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        [NotNull]
        public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content, string json, [NotNull] IError error)
        {
            //return new Result<TContent>(identifier, new []{error});
            return FailJson<TContent>(identifier, content, json,new[] {error} );
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
            //return new Result<TContent>(identifier, new []{error});
            return Fail<TContent>(identifier, new[] {error});
        }


        /// <summary>
        /// Bad Json Result
        /// </summary>
        /// <typeparam name="TContent">Type of object on error</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <param name="error">error</param>
        /// <returns>bad result</returns>
        [NotNull]
        public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, [NotNull] IError error)
        {
            //return new Result<TContent>(identifier, new []{error});
            return FailJson<TContent>(identifier, new[] {error});
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
            //return new Result<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) });
            return Fail<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) });
        }
        
        /// <summary>
        /// Bad Json Result
        /// </summary>
        /// <typeparam name="TContent">Type of object on error</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="eventId">Event Id</param>
        /// <param name="outputMessage">Output message</param>
        /// <returns>bad result</returns>
        [NotNull]
        public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, [NotNull] string errorMessage,
                                                       string outputMessage = "",EventId? eventId = null)
        {
            return FailJson<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) });
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
            return Fail<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) });
        }

        /// <summary>
        /// Bad Json Result
        /// </summary>
        /// <typeparam name="TContent">Type of object on error</typeparam>
        /// <param name="identifier">Identifier</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="eventId">Event Id</param>
        /// <param name="outputMessage">Output message</param>
        /// <returns>bad result</returns>
        [NotNull]
        public static IJsonResult<TContent> FailJson<TContent>([NotNull] IIdentifier identifier, EventId eventId, [NotNull] string errorMessage,
                                                       string outputMessage = "")
        {
            return FailJson<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) });
        }

    }
}