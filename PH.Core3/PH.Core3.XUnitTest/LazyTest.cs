using System;
using PH.Core3.Common.Identifiers;
using PH.Core3.Common.Result;
using Xunit;
using Xunit.Abstractions;

namespace PH.Core3.XUnitTest
{
    public class LazyTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public LazyTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private IResult<int> MethodTest(IResult<int> input)
        {
            _testOutputHelper.WriteLine($"{input.Content}");
            
            var i = new Identifier("abc");
            if(input.Content == 4)
                return ResultFactory.Fail<int>(i, "some error");

            return ResultFactory.Ok<int>(i, input.Content +1);
        }

        [Fact]
        public void Test2()
        {
            var chain = new LazyEvaluator<int>(() => MethodTest(ResultFactory.Ok<int>(new Identifier("wer")
                    , 1)))
                .Next(result => MethodTest(result))
                .Next(result => MethodTest(result))
                .Next(result => MethodTest(result))
                .Next(result => MethodTest(result))
                .Next(result => MethodTest(result))
                .Next(result => MethodTest(result))
                .Next(result => MethodTest(result))
                .Resolve();

            Assert.True(chain.OnError);
        }

        [Fact]
        public void Test1()
        {
            var id = new Identifier("c");
            var c = new LazyEvaluator<int>(() => ResultFactory.Ok(id,3))
                .Next(result => ResultFactory.Ok(result.Identifier, DateTime.Now.AddDays(result.Content) ))
                .Next(result => ResultFactory.Ok(result.Identifier, $"Step 3 => {result.Content}"));


            var x = c.Resolve();
            
            _testOutputHelper.WriteLine(x.ToString());
            
        }
        
        [Fact]
        public void Test()
        {
            var id = new Identifier("c");
            var c = new LazyEvaluator<string>(() => ResultFactory.Ok(id,"step 1"))
                .Next(result => ResultFactory.Ok(result.Identifier, $"Step 2 => {result.Content}"))
                .Next(result => ResultFactory.Ok(result.Identifier, $"Step 3 => {result.Content}"));


            var x = c.Resolve();
            
            Console.WriteLine(x);
            
        }

        }
    
    
    public class LazyEvaluator<T>
    {
        
        private Lazy<IResult<T>> _lazyResult;
        public LazyEvaluator(Func<IResult<T>> fnc)
        {
            _lazyResult = new Lazy<IResult<T>>( () => fnc.Invoke());
        }

//        public IResult<T> Evaluate()
//        {
//            var r = _lazyResult.Value;
//            
//        }
        private IResult<T> Value => _lazyResult.Value;

        public LazyEvaluator<TOther> Next<TOther>(Func<IResult<T>, IResult<TOther>> nextFunction)
        {
            return new LazyEvaluator<TOther>(() =>
            {
                var r = this.Value;
                return r.OnError ? ResultFactory.Fail<TOther>(r.Identifier, r.Errors) : nextFunction.Invoke(r);
            });
        }

        public IResult<T> Resolve() => Value;

    }

    /*
    public static class LazyEvaluatorExtensions
    {
        public static LazyEvaluator<TOutput> Next<TInput, TOutput>
            (this LazyEvaluator<TInput> evaluator, Func<IResult<TInput>, IResult<TOutput>> nextFunction)
        {
            
            return new LazyEvaluator<TOutput>(() =>
            {
                var r = evaluator.Value;
                return r.OnError ? ResultFactory.Fail<TOutput>(r.Identifier, r.Errors) : nextFunction.Invoke(r);
            });
        }
    }
    */
}