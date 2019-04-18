using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
// ReSharper disable IdentifierTypo

namespace PH.Core3.Common.Result
{
    /// <summary>
    /// Wrapper for function that require to be chained and async lazy-evaluated on on <see cref="ResolveAsync"/> call
    /// 
    /// </summary>
    /// <typeparam name="T">Type of result</typeparam>
    public class LazyEvaluatorAsync<T>
    {
        private readonly Lazy<Task<IResult<T>>> _lazyResult;
        internal readonly int ProgrId;
        private readonly IIdentifier _identifier;


        internal LazyEvaluatorAsync([NotNull] Func<Task<IResult<T>>> asyncFnc ,IIdentifier identifier, Func<IResult<T>,Task<IResult<T>>> onErrorFunc = null)
            : this(0, asyncFnc, identifier, onErrorFunc)
        {
            
        }

        private LazyEvaluatorAsync(int progrId, Func<Task<IResult<T>>> asyncFnc,IIdentifier identifier, Func<IResult<T>,Task<IResult<T>>> onErrorFunc = null)
        {
            ProgrId     = progrId;
            _identifier = identifier;
            //_lazyResult = new Lazy<Task<IResult<T>>>(async () => await asyncFnc.Invoke() );
            _lazyResult = new Lazy<Task<IResult<T>>>(async () => await FunctionCombinerAsync(asyncFnc, onErrorFunc));
        }

        private async Task<IResult<T>> FunctionCombinerAsync(Func<Task<IResult<T>>> asyncFnc, Func<IResult<T>, Task<IResult<T>>> onErrorFunc)
        {
            if (null == onErrorFunc)
            {
                return await asyncFnc.Invoke();
            }
            else
            {
                var r = await asyncFnc.Invoke();
                if (r.OnError)
                    return await onErrorFunc.Invoke(r);
                else
                    return r;
            }
        }


        [NotNull]
        private Task<IResult<T>> GetValue()
        {
            var t = Task.Run(() =>
            {
                var r = _lazyResult.Value;
                return r;
            });

            return t;
        }

        private IResult<T> Value =>  GetValue().Result;
        

        
          private async  Task<IResult<TOther>> FunctionCombinerWithArgAsync<TOther>(IResult<T> result, int progrIdentifier
                                                                              ,[NotNull] Func<IResult<T>, Task<IResult<TOther>>> nextFunction, Func<IResult<TOther>,Task<IResult<TOther>>> onErrorFunc = null)
        {
            if(result.OnError)
                return ResultFactory.FailLazyEvaluatedFunction<TOther>(progrIdentifier, result.Identifier,
                                                                       result.Errors);

            if (null == onErrorFunc)
            {
                var r = await nextFunction.Invoke(result);
                if (r.OnError)
                {
                    return ResultFactory.FailLazyEvaluatedFunction<TOther>(progrIdentifier, r.Identifier, r.Errors);
                }
                else
                {
                    return r;
                }
            }
            else
            {
                var r = await nextFunction.Invoke(result);
                if (r.OnError)
                {
                    var parsedError = await onErrorFunc.Invoke(r);
                    return ResultFactory.FailLazyEvaluatedFunction<TOther>(progrIdentifier, parsedError.Identifier,
                                                                           parsedError.Errors);
                }
                else
                {
                    return r;
                }

                
                
                    
            }
        }



        private LazyEvaluatorAsync<TOther> NextCombiner<TOther>(int progrIdentifier,[NotNull] Func<IResult<T>, Task<IResult<TOther>>> nextFunction,
                                                                Func<IResult<TOther>,Task<IResult<TOther>>> onErrorFunc = null)
        {
            return new LazyEvaluatorAsync<TOther>(progrIdentifier
                                             , async () => await FunctionCombinerWithArgAsync(Value, progrIdentifier, nextFunction, onErrorFunc),
                                             _identifier);
        }


        /// <summary>
        /// Chain current function with another that accept current function result as input
        /// </summary>
        /// <typeparam name="TOther">Type of output result</typeparam>
        /// <param name="nextFunction">Function to lazy-evaluate</param>
        /// <returns>lazy evaluator</returns>
        [NotNull]
        //public LazyEvaluatorAsync<TOther> Next<TOther>([NotNull] Func<IResult<T>, Task<IResult<TOther>>> nextFunction)
        public LazyEvaluatorAsync<TOther> Next<TOther>([NotNull] Func<IResult<T>,  Task<IResult<TOther>>> nextFunction,Func<IResult<TOther>,Task<IResult<TOther>>> onErrorFunc = null)
        {
           
            var pid = ProgrId + 1;

            return NextCombiner(pid, nextFunction, onErrorFunc);

            /*
            return new LazyEvaluatorAsync<TOther>(pid,async () =>
            {
                var r =  this.Value;
                if (r.OnError)
                    return ResultFactory.FailLazyEvaluatedFunction<TOther>(pid, r.Identifier, r.Errors);
                
                return  await nextFunction.Invoke(r);
            });*/
        }
        
        
        /// <summary>
        /// Async result a chain of functions returning output result
        /// </summary>
        /// <returns>final result</returns>
        public async Task<IResult<T>> ResolveAsync() =>  await _lazyResult.Value;

        public async Task<(bool Ok, IResult<T> result)> TryResolveAsync()
        {
            try
            {
                var result = await ResolveAsync();
                return (true, result);
            }
            catch (Exception e)
            {
                var result = ResultFactory.FailLazyEvaluatedFunctionFromException<T>(ProgrId, _identifier, e);
                return (false, result);
            }
        }

    }
}