//using System;
//using JetBrains.Annotations;
//// ReSharper disable IdentifierTypo

//namespace PH.Core3.Common.Result
//{

//    /// <summary>
//    /// Wrapper for function that require to be chained and lazy-evaluated on on <see cref="Resolve"/> call
//    /// 
//    /// </summary>
//    /// <typeparam name="T">Type of result</typeparam>
//    public class LazyEvaluator<T>
//    {
        
//        private readonly Lazy<IResult<T>> _lazyResult;
//        private readonly IIdentifier _identifier;
//        internal readonly int ProgrId;

//        internal LazyEvaluator([NotNull] Func<IResult<T>> fnc, IIdentifier identifier, Func<IResult<T>,IResult<T>> onErrorFunc = null)
//            :this(0, fnc, identifier)
//        {
            
//        }

//        private LazyEvaluator(int progrId,Func<IResult<T>> fnc, IIdentifier identifier, Func<IResult<T>,IResult<T>> onErrorFunc = null)
//        {
//            ProgrId     = progrId;
//            _identifier = identifier;
//            _lazyResult = new Lazy<IResult<T>>(() => FunctionCombiner(fnc, onErrorFunc));
//        }

//        private IResult<T> FunctionCombiner(Func<IResult<T>> fnc, Func<IResult<T>,IResult<T>> onErrorFunc = null)
//        {
//            if (null == onErrorFunc)
//            {
//                return fnc.Invoke();
//            }
//            else
//            {
//                var r = fnc.Invoke();
//                if (r.OnError)
//                    return onErrorFunc.Invoke(r);
//                else
//                    return r;
//            }
//        }

//        private IResult<TOther> FunctionCombinerWithArg<TOther>(IResult<T> result, int progrIdentifier,[NotNull] Func<IResult<T>, IResult<TOther>> nextFunction, Func<IResult<TOther>,IResult<TOther>> onErrorFunc = null)
//        {
//            if(result.OnError)
//                return ResultFactory.FailLazyEvaluatedFunction<TOther>(progrIdentifier, result.Identifier,
//                                                                       result.Errors);

//            if (null == onErrorFunc)
//            {
//                var r = nextFunction.Invoke(result);
//                if (r.OnError)
//                {
//                    return ResultFactory.FailLazyEvaluatedFunction<TOther>(progrIdentifier, r.Identifier, r.Errors);
//                }
//                else
//                {
//                    return r;
//                }
//            }
//            else
//            {
//                var r = nextFunction.Invoke(result);
//                if (r.OnError)
//                {
//                    var parsedError = onErrorFunc.Invoke(r);
//                    return ResultFactory.FailLazyEvaluatedFunction<TOther>(progrIdentifier, parsedError.Identifier,
//                                                                           parsedError.Errors);
//                }
//                else
//                {
//                    return r;
//                }

                
                
                    
//            }
//        }



       

//        private IResult<T> Value => _lazyResult.Value;

//        private LazyEvaluator<TOther> NextCombiner<TOther>(int progrIdentifier,[NotNull] Func<IResult<T>, IResult<TOther>> nextFunction,
//                                                           Func<IResult<TOther>, IResult<TOther>> onErrorFunc = null)
//        {
//            return new LazyEvaluator<TOther>(progrIdentifier
//                                             , () => FunctionCombinerWithArg(Value, progrIdentifier, nextFunction, onErrorFunc),
//                                             _identifier);
//        }

//        /// <summary>
//        /// Chain current function with another that accept current function result as input
//        /// </summary>
//        /// <typeparam name="TOther">Type of output result</typeparam>
//        /// <param name="nextFunction">Function to lazy-evaluate</param>
//        /// <param name="onErrorFunc"></param>
//        /// <returns>lazy evaluator</returns>
//        [NotNull]
//        public LazyEvaluator<TOther> Next<TOther>([NotNull] Func<IResult<T>, IResult<TOther>> nextFunction,Func<IResult<TOther>, IResult<TOther>> onErrorFunc = null)
//        {
//            var pid = ProgrId + 1;

//            return NextCombiner(pid, nextFunction, onErrorFunc);

//            //return new LazyEvaluator<TOther>(pid,() =>
//            //{
//            //    var r = this.Value;
//            //    return r.OnError ? ResultFactory.FailLazyEvaluatedFunction<TOther>(pid,r.Identifier, r.Errors) : nextFunction.Invoke(r);
//            //});
//        }

//        /// <summary>
//        /// Resolve a chain of functions returning output result
//        /// </summary>
//        /// <returns>final result</returns>
//        public IResult<T> Resolve() => Value;

        

//        public bool TryResolve(out IResult<T> result)
//        {
//            try
//            {
//                result = Resolve();
//                return true;
//            }
//            catch (Exception e)
//            {
//                result = ResultFactory.FailLazyEvaluatedFunctionFromException<T>(ProgrId, _identifier, e);
//                return false;
//            }
//        }

//    }
//}