namespace EA.Iws.Web.Tests.Unit.Infrastructure
{
    using System.Linq;
    using Web.Infrastructure;
    using Xunit;

    public class RangeExtensionsTests
    {
        [Theory]
        [InlineData("1", "1")]
        [InlineData("1,3,4,5", "1, 3-5")]
        [InlineData("1,2,3,4,5", "1-5")]
        [InlineData("1,2,3,5,6,7,8,12", "1-3, 5-8, 12")]
        [InlineData("4,5,6,8,10,14,15", "4-6, 8, 10, 14-15")]
        public void CanCreateRangeString(string input, string expected)
        {
            var result = input.Split(',').Select(int.Parse).ToRangeString();

            Assert.Equal(expected, result);
        }
    }
}