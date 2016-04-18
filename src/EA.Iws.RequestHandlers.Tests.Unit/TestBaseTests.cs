namespace EA.Iws.RequestHandlers.Tests.Unit
{
    using Xunit;

    public class TestBaseTests : TestBase
    {
        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        [InlineData(465)]
        [InlineData(-3)]
        public void DeterministicGuid_ReturnsCorrectGuid(int id)
        {
            var guid = DeterministicGuid(id);
            Assert.Equal(guid, DeterministicGuid(id));
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(3, 2)]
        [InlineData(0, 1)]
        [InlineData(int.MaxValue, 12)]
        [InlineData(4651, 463)]
        public void DeterministicGuid_ReturnsCorrectGuidWhichIsDifferent(int id, int otherId)
        {
            Assert.NotEqual(DeterministicGuid(otherId), DeterministicGuid(id));
        }
    }
}
