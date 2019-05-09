using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PH.Core3.Common.Extensions;
using Xunit;

namespace PH.Core3.XUnitTest
{
    public class EnumTest
    {
        [Fact]
        public void Test01()
        {

            var d0 = MyEnum.TestWithNoName.GetDisplayName();

            var d1 = MyEnum.TestWithName.GetDisplayName();

            var d2 = MyEnum.TestWithNameAndDescription.GetDisplayName();

            var d3 = MyEnum.TestWithNameAndDescription.GetDescription();

            var d4 = MyEnum.TestWithNoName.GetDescription();
            var d5 = MyEnum.TestWithName.GetDescription();

            Assert.True(!StringExtensions.IsNullString(d0));
            Assert.True(!StringExtensions.IsNullString(d1));
            Assert.True(!StringExtensions.IsNullString(d2));
            Assert.True(!StringExtensions.IsNullString(d3));
            Assert.True(!StringExtensions.IsNullString(d4));
            Assert.True(!StringExtensions.IsNullString(d5));

        }


        public enum MyEnum
        {
            TestWithNoName = 0,

            [Display(Name = "Test with a name")]
            TestWithName = 1,

            [Display(Name = "Test with a name and description")]
            [Description("Questa è la descrizione")]
            TestWithNameAndDescription = 2

        }

    }
}