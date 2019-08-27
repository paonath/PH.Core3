//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Reflection;
//using JetBrains.Annotations;
//using Org.BouncyCastle.Crypto.Macs;
//using PH.Core3.Common.Services.Components.Crud.Entities;
//using PH.Core3.TestContext;
//using Xunit;

//namespace PH.Core3.XUnitTest
//{
//    public class ValidationTest
//    {
//        [Fact]
//        public void TestDataAnnotation()
//        {
//            var c = new Category();


//           bool b = DataAnnotationsValidator.TryValidate(c, out var result);
//           var t = result;

//           var y = new Test();

           

//           y.EValue = (TestValues) 44;



//           y.EValue = TestValues.Due;

//           var yy = TestValues.Due;

//           var yu = yy.GetType();
//           var yi = yy.GetTypeCode();


//           y.Name = "Pluto";

//           bool q = DataAnnotationsValidator.TryValidate(y, out var errorResults);



//        }
//    }

//    public class Test
//    {
//        [Required]
//        public string Name { get; set; }
        
//        public DateTime? Date { get; set; }

//        public TestValues EValue { get; set; }
//    }

//    public enum TestValues
//    {
//        Uno = 1, Due = 2, Tre = 3
//    }


    
//}