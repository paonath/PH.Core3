using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using PH.Core3.Common;
using PH.Core3.Common.Identifiers;
using PH.Core3.Common.Result;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace PH.Core3.XUnitTest
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class TestBeforeAfter : BeforeAfterTestAttribute
    {
        private Stopwatch _stopwatch;
        
        public TestBeforeAfter()
        {
            _stopwatch = new Stopwatch();
        }

        public override void Before(MethodInfo methodUnderTest)
        {
           // Debug.WriteLine(methodUnderTest.Name);
           _stopwatch.Start();
        }

        public override void After(MethodInfo methodUnderTest)
        {
            _stopwatch.Stop();
            
            Debug.WriteLine(methodUnderTest.Name);
            Debug.WriteLine("Time elapsed: {0}", _stopwatch.Elapsed);
            Debug.WriteLine("------");
            Debug.WriteLine("");
            _stopwatch.Reset();
        }
    }

    public class AdvLazyTest
    {
        public async void Test01()
        {
            var id = new Identifier("abc");
            var chain = await ResultFactory
                              .ChainAsync(id, async () => await Fnc01())
                              .ResolveAsync();
        }

        private Task<IResult<int>> Fnc01()
        {
            var t = Task.FromResult(ResultFactory.Ok(new Identifier("abc"), 1));
            return t;
        }


    }

   

    public class LazyTest
    {
        #region init


        private readonly ITestOutputHelper _testOutputHelper;

        public LazyTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        
        private Task<IResult<int>> MethodTestAsync(IResult<int> input, int max = 180)
        {
            var m = $"MethodTestAsync {input.Content} {Thread.CurrentThread.ManagedThreadId}";
            Debug.WriteLine(m);
            _testOutputHelper.WriteLine(m);
            
            var i = new Identifier("abc");
            if(input.Content == max)
                return Task.FromResult(ResultFactory.Fail<int>(i, "some error"));


            return Task.FromResult(ResultFactory.Ok<int>(i, input.Content + 1));
            /*
            return Task.Run( ()=>
            {
                
               
            }) ;*/
        }

        private Task<IResult<int>> MethodTestAsync2(IResult<int> input)
        {
            var m = $"MethodTestAsync2 {input.Content} {Thread.CurrentThread.ManagedThreadId}";
            Debug.WriteLine(m);
            _testOutputHelper.WriteLine(m);
            var i = new Identifier("abc");


            return Task.FromResult(ResultFactory.Ok<int>(i, input.Content + 1));


        }


        private Task<IResult<object>> MethodTestAsync3(IResult<object> input)
        {
            var m = $"MethodTestAsync3  {Thread.CurrentThread.ManagedThreadId}";
            Debug.WriteLine(m);
            _testOutputHelper.WriteLine(m);
            var i = new Identifier("abc");


            return Task.FromResult(ResultFactory.Ok<object>(i, input.Content));


        }


        private IResult<int> MethodTest(IResult<int> input)
        {
            _testOutputHelper.WriteLine($"{input.Content}");
            
            var i = new Identifier("abc");
            if(input.Content == 4)
                return ResultFactory.Fail<int>(i, "some error");

            return ResultFactory.Ok<int>(i, input.Content +1);
        }
        

            #endregion

        [Fact]
        [TestBeforeAfter]
        public void TExample()
        {
            var id = new Identifier("some id");

            var lastResult = ResultFactory.Chain(id, () => ResultFactory.Ok(id, 7))
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

            Assert.True(lastResult.OnError == false);
        }


        [Fact]
        [TestBeforeAfter]
        public async void TestLazyEvaluatorAsync2()
        {
            _testOutputHelper.WriteLine($"TestAsync start {Thread.CurrentThread.ManagedThreadId}");
            var id = new Identifier("wer");

            Stopwatch counter = Stopwatch.StartNew();

            var chain = await ResultFactory.ChainAsync(id,
                    async () => await MethodTestAsync(ResultFactory.Ok<int>(id, 1)), error =>
                    {
                        _testOutputHelper.WriteLine(error.Errors.First().ErrorMessage);
                        return Task.FromResult(error);
                    })

                               #region body

                               .Next(async (v, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))                
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))                
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))                
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))                
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))                
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))                
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))                
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))                
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))                
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                               .Next(async (eval, result) => await MethodTestAsync(result))
                
                               .Next(async (eval, result) => await MethodTestAsync(result))

                #endregion

                .ResolveAsync();

            counter.Stop();

            _testOutputHelper
                .WriteLine($"TestAsync end {Thread.CurrentThread.ManagedThreadId}:  Time elapsed: {counter.Elapsed}");

            Assert.True(chain.OnError);
        }


        [Fact]
        [TestBeforeAfter]
        public async void TestAsync()
        {
            _testOutputHelper.WriteLine($"TestAsync start {Thread.CurrentThread.ManagedThreadId}");
            var id = new Identifier("wer");

            var chain = await ResultFactory.ChainAsync(id,
                    async () => await MethodTestAsync(ResultFactory.Ok<int>(id, 1)), error =>
                    {
                        _testOutputHelper.WriteLine(error.Errors.First().ErrorMessage);
                        return Task.FromResult(error);
                    })
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))                
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))                
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))                
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))
                .Next(async (eval, result) => await MethodTestAsync(result))

                .ResolveAsync();

            _testOutputHelper.WriteLine($"TestAsync end {Thread.CurrentThread.ManagedThreadId}");

            Assert.True(chain.OnError == false);
        }

        [Fact]
        [TestBeforeAfter]
        public async void TestAsync2()
        {
            _testOutputHelper.WriteLine($"TestAsync start {Thread.CurrentThread.ManagedThreadId}");
            var id = new Identifier("wer");
            Stopwatch counter = Stopwatch.StartNew();

            var chain = await ResultFactory.ChainAsync(id,
                    async () => await MethodTestAsync(ResultFactory.Ok<int>(id, 1)))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))
                .Next(async (eval, result) => await MethodTestAsync2(result))

                .ResolveAsync();

            counter.Stop();
            _testOutputHelper
                .WriteLine($"TestAsync end {Thread.CurrentThread.ManagedThreadId}:  Time elapsed: {counter.Elapsed}");


            Assert.True(chain.OnError == false);
        }

        [Fact]
        [TestBeforeAfter]
        public void Test2()
        {
            var id = new Identifier("wer");
            var initData = ResultFactory.Ok<int>(id
                                                 , 1);

            var chain = ResultFactory
                        .Chain(id, () => MethodTest(initData), onError =>
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
        [TestBeforeAfter]
        public void Test1()
        {
            var id = new Identifier("c");
            var c = ResultFactory.Chain(id, () => ResultFactory.Ok(id, 3))
                                 .Next(result => ResultFactory.Ok(result.Identifier,
                                                                  DateTime.Now.AddDays(result.Content)))
                                 .Next(result => ResultFactory.Ok(result.Identifier, $"Step 3 => {result.Content}"))
                                 .Next(result => MethodTest(ResultFactory.Ok(result.Identifier, 78)), onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 }).Next(MethodTest, onError =>
                                 {
                                     _testOutputHelper
                                         .WriteLine($"On Error {onError.Errors.FirstOrDefault()};");
                                     return onError;
                                 });

            var x = c.Resolve();
            
            _testOutputHelper.WriteLine(x.ToString());
            
        }
        

        [Fact]
        [TestBeforeAfter]
        public async void TestWithExit()
        {
            _testOutputHelper.WriteLine($"TestAsync start {Thread.CurrentThread.ManagedThreadId}");
            var       id      = new Identifier("wer");
            Stopwatch counter = Stopwatch.StartNew();

            var chain = await ResultFactory.ChainAsync(id,
                                                       async () => await MethodTestAsync(ResultFactory.Ok<int>(id, 1)))
                                           .Next(async (eval, result) =>
                                           {
                                               var r = await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                               return r;
                                           }).Next(async (eval, result) =>
                                           {
                                               var r = await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                               return r;
                                           })
                                           .Next(async (eval, result) =>
                                           {
                                               var r =  await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                               await eval.RaiseExitAsync();


                                               return r;
                                           } )
                                          .Next(async (eval, result) =>
                                                 {
                                                     var r = await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                                     return r;
                                                 })
                                           .Next(async (eval, result) =>
                                           {
                                               var r = await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                               return r;
                                           })
                                           .Next(async (eval, result) =>
                                           {
                                               var r = await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                               return r;
                                           })
                                           .Next(async (eval, result) =>
                                           {
                                               var r = await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                               return r;
                                           })
                                           .Next(async (eval, result) =>
                                           {
                                               var r = await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                               return r;
                                           })
                                           .Next(async (eval, result) =>
                                           {
                                               var r = await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                               return r;
                                           })
                                           .Next(async (eval, result) =>
                                           {
                                               var r = await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                               return r;
                                           })
                                           .Next(async (eval, result) =>
                                           {
                                               var r = await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                               return r;
                                           })
                                           //.Next(async (eval, result) =>
                                           //{
                                           //    var r =  await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                           //    return r;
                                           //} )
                                           //.Next(async (eval, result) =>
                                           //{
                                           //    var r =  await MethodTestAsync3(ResultFactory.Ok(id, new object()));
                                           //    return r;
                                           //} )
                                           /*
                                            .Next(async v =>
                                            {
                                                var xy = await MethodTestAsync3(ResultFactory.Ok(id, new object()));


                                            } )
                                            .Next(async v => await MethodTestAsync3(ResultFactory.Ok(id, new object()) ))
                                           */


                                           //.Next(async (eval, result) => await MethodTestAsync2(result))
                                           //.Next(async (eval, result) => await MethodTestAsync2(result))
                                           //.Next(async (eval, result) => await MethodTestAsync2(result))


                                           .ResolveAsync();

            counter.Stop();
            _testOutputHelper
                .WriteLine($"TestAsync end {Thread.CurrentThread.ManagedThreadId}:  Time elapsed: {counter.Elapsed}");


            Assert.True(chain.OnError == false);
            
        }

        [Fact]
        [TestBeforeAfter]
        public void Test()
        {
            var id = new Identifier("c");
            var c = ResultFactory.Chain(id,() => ResultFactory.Ok(id,"step 1"))
                .Next(result => ResultFactory.Ok(result.Identifier, $"Step 2 => {result.Content}"))
                .Next(result => ResultFactory.Ok(result.Identifier, $"Step 3 => {result.Content}"));


            var x = c.Resolve();
            
            Console.WriteLine(x);
            
        }

        }


   
}