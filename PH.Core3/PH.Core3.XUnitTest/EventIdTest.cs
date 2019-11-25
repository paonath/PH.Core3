using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Identifiers.Services;
using Xunit;

namespace PH.Core3.XUnitTest
{


    public class EventIdTest
    {
        [Fact]
        public void T01()
        {

            var test0 = new EventId(1);
            var test1 = new EventId(2);

            //var d = test0 + test1;



            var svc0 = new ServiceIdentifier(1000, "ciao");
            var svc1 = new ServiceIdentifier(2000, "ciao caro");

            var t0 = svc0 - svc1;

            var t1 = svc0 + test0;


        }
    }
}