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

        internal LazyEvaluatorAsync([NotNull] Func<Task<IResult<T>>> asyncFnc )
            : this(0, asyncFnc)
        {
            
        }

        private LazyEvaluatorAsync(int progrId, Func<Task<IResult<T>>> asyncFnc)
        {
            ProgrId     = progrId;
            _lazyResult = new Lazy<Task<IResult<T>>>(async () => await asyncFnc.Invoke() );
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
        
        /// <summary>
        /// Chain current function with another that accept current function result as input
        /// </summary>
        /// <typeparam name="TOther">Type of output result</typeparam>
        /// <param name="nextFunction">Function to lazy-evaluate</param>
        /// <returns>lazy evaluator</returns>
        [NotNull]
        public LazyEvaluatorAsync<TOther> Next<TOther>([NotNull] Func<IResult<T>, Task<IResult<TOther>>> nextFunction)
        {
            var pid = ProgrId + 1;
            return new LazyEvaluatorAsync<TOther>(pid,async () =>
            {
                var r =  this.Value;
                if (r.OnError)
                    return ResultFactory.FailLazyEvaluatedFunction<TOther>(pid, r.Identifier, r.Errors);
                
                return  await nextFunction.Invoke(r);
            });
        }
        
        
        /// <summary>
        /// Async result a chain of functions returning output result
        /// </summary>
        /// <returns>final result</returns>
        public async Task<IResult<T>> ResolveAsync() =>  await _lazyResult.Value;
 
    }
}