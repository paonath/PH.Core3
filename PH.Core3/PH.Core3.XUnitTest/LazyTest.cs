using System;
using System.Threading;
using System.Threading.Tasks;
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

        
        private Task<IResult<int>> MethodTestAsync(IResult<int> input)
        {
            _testOutputHelper.WriteLine($"MethodTestAsync {input.Content} {Thread.CurrentThread.ManagedThreadId}");

            return Task.Run( ()=>
            {
                
                var i = new Identifier("abc");
                if(input.Content == 7)
                    return ResultFactory.Fail<int>(i, "some error");


                return ResultFactory.Ok<int>(i, input.Content + 1);
            }) ;
        }

        private Task<IResult<int>> MethodTestAsync2(IResult<int> input)
        {
            
            _testOutputHelper.WriteLine($"MethodTestAsync {input.Content} {Thread.CurrentThread.ManagedThreadId}");

            return Task.Run( ()=>
            {
               
                var i = new Identifier("abc");
                
                return ResultFactory.Ok<int>(i, input.Content + 1);
            }) ;
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
        public async void TestAsync()
        {
            _testOutputHelper.WriteLine($"TestAsync start {Thread.CurrentThread.ManagedThreadId}");

            var chain = await new LazyEvaluatorAsync<int>(
                    async () => await MethodTestAsync(ResultFactory.Ok<int>(new Identifier("wer")
                    , 1)))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))
                .Next(async result => await MethodTestAsync(result))

                .ResolveAsync();

            _testOutputHelper.WriteLine($"TestAsync end {Thread.CurrentThread.ManagedThreadId}");

            Assert.True(chain.OnError);
        }

        [Fact]
        public async void TestAsync2()
        {
            _testOutputHelper.WriteLine($"TestAsync start {Thread.CurrentThread.ManagedThreadId}");

            var chain = await new LazyEvaluatorAsync<int>(
                    async () => await MethodTestAsync(ResultFactory.Ok<int>(new Identifier("wer")
                        , 1)))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))
                .Next(async result => await MethodTestAsync2(result))

                .ResolveAsync();

            _testOutputHelper.WriteLine($"TestAsync end {Thread.CurrentThread.ManagedThreadId}");

            Assert.True(chain.OnError == false);
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


    public class LazyEvaluatorAsync<T>
    {
        private Lazy<Task<IResult<T>>> _lazyResult;

        public LazyEvaluatorAsync(Func<Task<IResult<T>>> asyncFnc )
        {
            _lazyResult = new Lazy<Task<IResult<T>>>(async () => await asyncFnc.Invoke() );
        }

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
        
        public LazyEvaluatorAsync<TOther> Next<TOther>(Func<IResult<T>, Task<IResult<TOther>>> nextFunction)
        {
            return new LazyEvaluatorAsync<TOther>(async () =>
            {
                var r =  this.Value;
                if (r.OnError)
                    return ResultFactory.Fail<TOther>(r.Identifier, r.Errors);
                
                return  await nextFunction.Invoke(r);
            });
        }
        
        
        public async Task<IResult<T>> ResolveAsync() =>  await _lazyResult.Value;
 
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