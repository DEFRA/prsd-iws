namespace EA.Iws.Domain.Tests.Unit
{
    using Xunit;

    public class AddressTests
    {
        [Fact]
        public void IsUkAddress()
        {
            var addressUk = new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "United Kingdom");

            Assert.True(addressUk.IsUkAddress);
        }

        [Fact]
        public void IsNotUkAddress()
        {
            var addressNonUk = new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Thailand");

            Assert.False(addressNonUk.IsUkAddress);
        }
    }
}
