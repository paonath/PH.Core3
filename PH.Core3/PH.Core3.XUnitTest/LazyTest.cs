using System;
using System.Linq;
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
        public void TExample()
        {
            var lastResult = ResultFactory.Chain(() => ResultFactory.Ok(new Identifier("some Id"), 7))
                                          .Next(r => ResultFactory.Ok(r.Identifier,
                                                                      DateTime.UtcNow.AddDays(r.Content)))
                                          .Next(r => ResultFactory.Ok(r.Identifier,
                                                                      $"Added 7 days to '{DateTime.Now:D}': '{r.Content:D}' "))
                                          .Resolve();
            if (lastResult.OnError)
            {
                //...
            }
            else
            {
                //...
            }
        }


        [Fact]
        public async void TestAsync()
        {
            _testOutputHelper.WriteLine($"TestAsync start {Thread.CurrentThread.ManagedThreadId}");

            var chain = await ResultFactory.Chain(
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

            var chain = await ResultFactory.Chain(
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
            var initData = ResultFactory.Ok<int>(new Identifier("wer")
                                                 , 1);

            var chain = ResultFactory
                        .Chain(() => MethodTest(initData), onError =>
                        {
                            _testOutputHelper
                                .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                            return onError;
                        })
                        .Next(result => MethodTest(result), onError =>
                        {
                            _testOutputHelper
                                .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                            return onError;
                        })
                        .Next(result => MethodTest(result), onError =>
                        {
                            _testOutputHelper
                                .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                            return onError;
                        })
                        .Next(result => MethodTest(result), onError =>
                        {
                            _testOutputHelper
                                .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                            return onError;
                        })
                        .Next(result => MethodTest(result), onError =>
                        {
                            _testOutputHelper
                                .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                            return onError;
                        })
                        .Next(result => MethodTest(result), onError =>
                        {
                            _testOutputHelper
                                .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                            return onError;
                        })
                        .Next(MethodTest, onError =>
                        {
                            _testOutputHelper
                                .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                            return onError;
                        })
                        .Next(MethodTest, onError =>
                        {
                            _testOutputHelper
                                .WriteLine($"On Error {onError.Errors.FirstOrDefault()}");
                            return onError;
                        })
                        .Resolve();


            Assert.True(chain.OnError);
        }


        [Fact]
        public void Test1()
        {
            var id = new Identifier("c");
            var c = ResultFactory.Chain(() => ResultFactory.Ok(id,3))
                .Next(result => ResultFactory.Ok(result.Identifier, DateTime.Now.AddDays(result.Content) ))
                .Next(result => ResultFactory.Ok(result.Identifier, $"Step 3 => {result.Content}"));


            var x = c.Resolve();
            
            _testOutputHelper.WriteLine(x.ToString());
            
        }
        
        [Fact]
        public void Test()
        {
            var id = new Identifier("c");
            var c = ResultFactory.Chain(() => ResultFactory.Ok(id,"step 1"))
                .Next(result => ResultFactory.Ok(result.Identifier, $"Step 2 => {result.Content}"))
                .Next(result => ResultFactory.Ok(result.Identifier, $"Step 3 => {result.Content}"));


            var x = c.Resolve();
            
            Console.WriteLine(x);
            
        }

        }


   
}