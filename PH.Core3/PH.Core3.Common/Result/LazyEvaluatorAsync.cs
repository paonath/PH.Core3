using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using FluentValidation;
using JetBrains.Annotations;
// ReSharper disable IdentifierTypo

namespace PH.Core3.Common.Result
{

    //internal class LazyEvaluatorResolver<T> 
    //{
    //    private readonly LazyEvaluatorAsync<T> _evaluatorAsync;


    //    public async Task<IResult<T>> ResolveAsync()
    //    {
    //        return await _evaluatorAsync.ResolveAsync();
    //    }
    //}

    //public class ChainHelper
    //{
    //    private LazyResult<T> _lazyResult;

    //    public IResult<T> Result => _lazyResult;

    //    public ChainHelper(LazyResult<T> lazyResult)
    //    {
    //        _lazyResult = lazyResult;
    //    }


    //    public void RaiseExit<TExit>() => _lazyResult.RaiseExit<TExit>();


    //}

    internal class LazyResult<T> : Result<T> , IResult<T>
    {
        private bool _raisedExit;
        private int _progrId;
        internal int ProgrId => _progrId;

        public bool RaisedExit => _raisedExit;

        /// <summary>
        /// Init new instance of result with no error
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="progrId">progr id</param>
        /// <param name="content">Content</param>
        internal LazyResult([NotNull] IIdentifier identifier, int progrId, [NotNull] T content)
            : this(identifier, progrId, content, false)
        {
        }

        internal LazyResult([NotNull] IIdentifier identifier, int progrId, [NotNull] T content, bool raiseExit) : base(identifier, content)
        {
            _raisedExit = raiseExit;
            _progrId = progrId;
        }

        private LazyResult([NotNull] IIdentifier identifier, int progrId) : base(identifier, null)
        {
            _raisedExit = true;
            _progrId = progrId;
        }


        [NotNull]
        public LazyResult<T> RaiseExit(int pid)
        {
            _raisedExit = true;
            _progrId = pid;

            return this;
        }

        [NotNull]
        public static LazyResult<TEnd> Parse<TEnd, TSource>([NotNull] LazyResult<TSource> resultExited)
        {
            return new LazyResult<TEnd>(resultExited.Identifier,resultExited.ProgrId);
        }

        /// <summary>
        /// Init new instance of result with errors
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="progrId">progr id</param>
        /// <param name="error">error </param>
        internal LazyResult([NotNull] IIdentifier identifier, int progrId, [NotNull] IError error) 
            : base(identifier, error)
        {
            _progrId = progrId;
            _raisedExit = false;
        }

        //internal static LazyResult<TResult> Parse<TResult>(IResult<TResult> result)
        //{
        //    return new LazyResult<TResult>(result.Identifier, result.Content);
        //}
        

    }

    /// <summary>
    /// Lazy evaluator for functions
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyEvaluatorAsync<T>
    {
        internal readonly int ProgrId;
        private bool _evaluated;
        private LazyResult<T> _lazyResultInternal;
        private bool _raisedExit;


        private readonly IIdentifier _identifier;
        private readonly Func<Task<IResult<T>>> _asyncFnc;
        private readonly Func<IResult<T>, Task<IResult<T>>> _onErrorFunc;


        internal LazyEvaluatorAsync(IIdentifier identifier, Func<Task<IResult<T>>> asyncFnc, [CanBeNull] Func<IResult<T>, Task<IResult<T>>> onErrorFunc = null)
            : this(0, identifier, asyncFnc, onErrorFunc)
        {
            
            
        }

        private LazyEvaluatorAsync(int progrId, IIdentifier identifier, Func<Task<IResult<T>>> asyncFnc,[CanBeNull] Func<IResult<T>, Task<IResult<T>>> onErrorFunc = null)
        {
            ProgrId      = progrId;
            _identifier  = identifier;
            _asyncFnc    = asyncFnc;
            _onErrorFunc = onErrorFunc;
            _evaluated = false;
            _raisedExit = false;
        }

        

        [ItemNotNull]
        private async Task<LazyResult<T>> EvaluateResultAsync()
        {
            var c = await _asyncFnc.Invoke();
            _evaluated = true;

            if (_raisedExit || ((c as LazyResult<T>)?.RaisedExit ?? false))
            {
                return new LazyResult<T>(c.Identifier, ProgrId, c.Content).RaiseExit(ProgrId);
            }


            if (c.OnError)
            {
                if (null != _onErrorFunc)
                {
                    var err = await _onErrorFunc.Invoke(c);
                    return new LazyResult<T>(err.Identifier, ProgrId, err.Error);
                }
                else
                {
                    return new LazyResult<T>(c.Identifier, ProgrId, c.Error);
                }
            }

            
            return new LazyResult<T>(c.Identifier, ProgrId, c.Content); //LazyResult<T>.Parse(c);


            

        }


        /// <summary>Raises the exit asynchronous.</summary>
        /// <returns>Result</returns>
        [ItemNotNull]
        public async Task<IResult<T>> RaiseExitAsync()
        {
            if (!_evaluated)
            {
                _lazyResultInternal = await EvaluateResultAsync();
            }

            _raisedExit = true;

            return _lazyResultInternal.RaiseExit(ProgrId);

        }

        /// <summary>Add the specified next function.</summary>
        /// <typeparam name="TOther">The type of the next exit result.</typeparam>
        /// <param name="nextFunction">The next function.</param>
        /// <param name="onErrorFunc">The on error function.</param>
        /// <returns></returns>
        [NotNull]
        public LazyEvaluatorAsync<TOther> Next<TOther>([NotNull] Func<LazyEvaluatorAsync<T>, IResult<T>,  Task<IResult<TOther>>> nextFunction,[CanBeNull] Func<IResult<TOther>,Task<IResult<TOther>>> onErrorFunc = null)
        {
           
            var pid = ProgrId + 1;

            return new LazyEvaluatorAsync<TOther>(pid, _identifier,
                                                  async () => await FunctionCombinerAsync(nextFunction, onErrorFunc));


        }

        private async Task<IResult<TOther>> FunctionCombinerAsync<TOther>( [NotNull] Func<LazyEvaluatorAsync<T>, IResult<T>,  Task<IResult<TOther>>> nextFunction, Func<IResult<TOther>, Task<IResult<TOther>>> onErrorFunc)
        {
            _lazyResultInternal = await EvaluateResultAsync();
            if (_lazyResultInternal.OnError)
            {
                return ResultFactory.FailLazyEvaluatedFunction<TOther>(ProgrId, _identifier, _lazyResultInternal.Error);
            }

            if (_raisedExit)
            {
                return LazyResult<TOther>.Parse<TOther, T>(_lazyResultInternal);
            }


            var c = await nextFunction.Invoke(this,_lazyResultInternal);

            if (_raisedExit || _lazyResultInternal.RaisedExit)
            {
                return LazyResult<TOther>.Parse<TOther, T>(_lazyResultInternal);
            }

            if (!c.OnError || null == onErrorFunc)
            {
                return c;
            }

            return await onErrorFunc.Invoke(c);
           
        }

        #region old


        //[NotNull]
        //public LazyEvaluatorAsync<TOther> Next<TOther>([NotNull] Func<IResult<T>,  Task<IResult<TOther>>> nextFunction,Func<IResult<TOther>,Task<IResult<TOther>>> onErrorFunc = null)
        //{
           
        //    var pid = ProgrId + 1;
        //    return new LazyEvaluatorAsync<TOther>(pid, _identifier, async () => await FunctionCombinerAsync(nextFunction, onErrorFunc ));

        //}

        //private async Task<IResult<TOther>> FunctionCombinerAsync<TOther>( Func<LazyResult<T>,  Task<IResult<TOther>>> nextFunction, Func<IResult<TOther>, Task<IResult<TOther>>> onErrorFunc)
        //{
        //    var r = await EvaluateResultAsync();
        //    if (r.OnError)
        //        return ResultFactory.FailLazyEvaluatedFunction<TOther>(ProgrId, _identifier, r.Errors);

        //    if (r.RaisedExit)
        //        return LazyResult<TOther>.Parse<TOther, T>(r);


        //    var c = await nextFunction.Invoke(r);
        //    if (c.OnError)
        //    {
        //        if(null != onErrorFunc)
        //            return await onErrorFunc.Invoke(c);
        //    }

            
        //    return c;

            
           
        //}

        

        #endregion
        /// <summary>
        /// Async result a chain of functions returning output result
        /// </summary>
        /// <returns>final result</returns>
        public async Task<IResult<T>> ResolveAsync()
        {
            return await Task.Run(async () => await EvaluateResultAsync());
            
        }

        /// <summary>Tries the resolve asynchronous.</summary>
        /// <returns></returns>
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

    ///// <summary>
    ///// Wrapper for function that require to be chained and async lazy-evaluated on on <see cref="ResolveAsync"/> call
    ///// 
    ///// </summary>
    ///// <typeparam name="T">Type of result</typeparam>
    //public class LazyEvaluatorAsync<T>
    //{
    //    private readonly Lazy<Task<IResult<T>>> _lazyResult;
    //    internal readonly int ProgrId;
    //    private readonly IIdentifier _identifier;


    //    internal LazyEvaluatorAsync([NotNull] Func<Task<IResult<T>>> asyncFnc ,IIdentifier identifier, Func<IResult<T>,Task<IResult<T>>> onErrorFunc = null)
    //        : this(0, asyncFnc, identifier, onErrorFunc)
    //    {
            
    //    }

    //    private LazyEvaluatorAsync(int progrId, Func<Task<IResult<T>>> asyncFnc,IIdentifier identifier, Func<IResult<T>,Task<IResult<T>>> onErrorFunc = null)
    //    {
    //        ProgrId     = progrId;
    //        _identifier = identifier;
    //        //_lazyResult = new Lazy<Task<IResult<T>>>(async () => await asyncFnc.Invoke() );
    //        _lazyResult = new Lazy<Task<IResult<T>>>(async () => await FunctionCombinerAsync(asyncFnc, onErrorFunc));
    //    }

    //    private async Task<IResult<T>> FunctionCombinerAsync(Func<Task<IResult<T>>> asyncFnc, Func<IResult<T>, Task<IResult<T>>> onErrorFunc)
    //    {
    //        var c = await asyncFnc.Invoke();
    //        if (!c.OnError || null == onErrorFunc)
    //            return c;

    //        return await onErrorFunc.Invoke(c);
           
    //    }


    //    [NotNull]
    //    private Task<IResult<T>> GetValue()
    //    {

    //        var t = Task.Run(() =>
    //        {
    //            var r = _lazyResult.Value;
    //            return r;
    //        });

    //        return t;
    //    }

    //    private IResult<T> Value =>  GetValue().Result;
        

        
    //      private async  Task<IResult<TOther>> FunctionCombinerWithArgAsync<TOther>(IResult<T> result, int progrIdentifier
    //                                                                          ,[NotNull] Func<IResult<T>, Task<IResult<TOther>>> nextFunction, Func<IResult<TOther>,Task<IResult<TOther>>> onErrorFunc = null)
    //    {
    //        if(result.OnError)
    //            return ResultFactory.FailLazyEvaluatedFunction<TOther>(progrIdentifier, result.Identifier,
    //                                                                   result.Errors);

    //        var r = await nextFunction.Invoke(result);
    //        if (!r.OnError || null == onErrorFunc)
    //            return r;


    //        return ResultFactory.FailLazyEvaluatedFunction<TOther>(progrIdentifier, r.Identifier, r.Errors);

            
    //    }



    //    private LazyEvaluatorAsync<TOther> NextCombiner<TOther>(int progrIdentifier,[NotNull] Func<IResult<T>, Task<IResult<TOther>>> nextFunction,
    //                                                            Func<IResult<TOther>,Task<IResult<TOther>>> onErrorFunc = null)
    //    {
    //        return new LazyEvaluatorAsync<TOther>(progrIdentifier
    //                                         , async () => await FunctionCombinerWithArgAsync(Value, progrIdentifier, nextFunction, onErrorFunc),
    //                                         _identifier);
    //    }


    //    /// <summary>
    //    /// Chain current function with another that accept current function result as input
    //    /// </summary>
    //    /// <typeparam name="TOther">Type of output result</typeparam>
    //    /// <param name="nextFunction">Function to lazy-evaluate</param>
    //    /// <returns>lazy evaluator</returns>
    //    [NotNull]
    //    //public LazyEvaluatorAsync<TOther> Next<TOther>([NotNull] Func<IResult<T>, Task<IResult<TOther>>> nextFunction)
    //    public LazyEvaluatorAsync<TOther> Next<TOther>([NotNull] Func<IResult<T>,  Task<IResult<TOther>>> nextFunction,Func<IResult<TOther>,Task<IResult<TOther>>> onErrorFunc = null)
    //    {
           
    //        var pid = ProgrId + 1;

    //        return NextCombiner(pid, nextFunction, onErrorFunc);

    //        /*
    //        return new LazyEvaluatorAsync<TOther>(pid,async () =>
    //        {
    //            var r =  this.Value;
    //            if (r.OnError)
    //                return ResultFactory.FailLazyEvaluatedFunction<TOther>(pid, r.Identifier, r.Errors);
                
    //            return  await nextFunction.Invoke(r);
    //        });*/
    //    }
        
        
    //    /// <summary>
    //    /// Async result a chain of functions returning output result
    //    /// </summary>
    //    /// <returns>final result</returns>
    //    public async Task<IResult<T>> ResolveAsync() =>  await _lazyResult.Value;

    //    public async Task<(bool Ok, IResult<T> result)> TryResolveAsync()
    //    {
    //        try
    //        {
    //            var result = await ResolveAsync();
    //            return (true, result);
    //        }
    //        catch (Exception e)
    //        {
    //            var result = ResultFactory.FailLazyEvaluatedFunctionFromException<T>(ProgrId, _identifier, e);
    //            return (false, result);
    //        }
    //    }

    //}


}