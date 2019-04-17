using System;
using System.Threading.Tasks;
using PH.Core3.Common.Identifiers;
using PH.Core3.Common.Result;
using Xunit;

namespace PH.Core3.XUnitTest
{


    public class UnitTest1
    {
        private IResult<int> TryTre(bool b)
        {
            var id = new Identifier("tre");
            if (b)
            {
                var r = ResultFactory.Ok(id, 3);
                return r;
            }
            else
            {
                var bad = ResultFactory.Fail<int>(id, new Error("Sbaglio apposta", "", null, null));
                return bad;
            }

        }



        public async Task<IResult<int>> TreOk()
        {
            bool param = true;

            return TryTre(param);


        }

        [Fact]
        public async void Test1()
        {
            var c = new Couple<int, string>(async () => await TreOk(),
                                            result =>
                                            {
                                                var i  = result.Identifier;
                                                var kc = ResultFactory.Ok<string>(i, $"Tre � '{result.Content}'");
                                                return Task.FromResult(kc);
                                            });

            var c2 = new Couple<string, string>(async () =>
            {
                var res = await c.GetLazyOutput();
                return res.Value;
            }, result =>
            {
                var i  = result.Identifier;
                var kc = ResultFactory.Ok<string>(i, $"Ho scritto nella Couple precedente: '{result.Content}'");
                return Task.FromResult(kc);
            });


           // var cc = await c.GetLazyOutput();

            //var x = cc.Value;

            var finalResult = (await c2.GetLazyOutput()).Value;


        }
    }

   

    public class Combiner
    {
        public Combiner()
        {
            
        }
    }

    public class ChainedCouple<TInput, TOuput> : Couple<TInput, TOuput>
    {
        public ChainedCouple(Func<Task<IResult<TInput>>> inputFunc, Func<IResult<TInput>, Task<IResult<TOuput>>> outputFunc) : base(inputFunc, outputFunc)
        {
        }
    }

    public abstract class BaseCouple
    {
        public abstract Task<Lazy<IResult<TOuput>>> GetLazyOutput<TOuput>();

    }

    public class Couple<TInput, TOuput> //: BaseCouple
    {
   

        private Func<Task<IResult<TInput>>> _inputFunc;
        private Func<IResult<TInput>, Task<IResult<TOuput>>> _outputFunc;

        public Couple(Func<Task<IResult<TInput>>> inputFunc, Func<IResult<TInput>, Task<IResult<TOuput>>> outputFunc)
        {
            _inputFunc = inputFunc;
            _outputFunc = outputFunc;
        }


        public IResult<TOuput> Error<T>(IResult<T> inpurResult)
        {
            if (inpurResult is IResult<TOuput> r)
                return r;

            return ResultFactory.Fail<TOuput>(inpurResult.Identifier, inpurResult.Errors);
        }

        /*
        public override Task<Lazy<IResult<TOuput>>> GetLazyOutput()
        {
            return Task.FromResult(new Lazy<IResult<TOuput>>( () => GetOutput().Result));
        }
        */

        private async Task<IResult<TOuput>> GetOutput()
        {
            var r1 = await _inputFunc.Invoke();
            if (r1.OnError)
                return Error(r1);

            return await _outputFunc.Invoke(r1);
        }


        public Task<Lazy<IResult<TOuput>>> GetLazyOutput()
        {
            var lazy = new Lazy<IResult<TOuput>>(() =>
            {
                var r = GetOutput();
                r.Wait();
                return r.Result;
            });

            return Task.FromResult(lazy);

            //var lazy = new Lazy<TOuput>(() =>
            //{
            //    var r = GetOutput();
            //    r.Wait();
            //    return r.Result;
            //});



            //return Task.FromResult(new Lazy<IResult<TOuput>>( () => GetOutput().Result));
        }
    }

    
}
