//using System;
//using System.Security.Claims;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using PH.Core3.Common.Identifiers;
//using PH.Results;
//using Xunit;

//namespace PH.Core3.XUnitTest
//{
//    public class SerializeTest
//    {
//        [Fact]
//        public void SerializeAnError()
//        {

//            var r = ResultFactory.FailFromException<DateTime>(new ClaimsPrincipalIdentifier("TESTId", ClaimsPrincipal.Current),
//                                                              new ArgumentException("è solo un test"),
//                                                              new EventId(1, "qualcosa"), "sto provando a serializzare",
//                                                              "messaggio in uscita");
//            var t = JsonConvert.SerializeObject(r);

//            Assert.NotNull(t);


//        }

        
//    }
//}