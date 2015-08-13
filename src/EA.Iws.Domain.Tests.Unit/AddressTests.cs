namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Xunit;

    public class AddressTests
    {
        [Fact]
        public void IsUkAddress()
        {
            var addressUk = new Address("address1", string.Empty, "town", string.Empty, "postcode", "United Kingdom");

            Assert.True(addressUk.IsUkAddress);
        }

        [Fact]
        public void IsNotUkAddress()
        {
            var addressNonUk = new Address("address1", string.Empty, "town", string.Empty, string.Empty, "Thailand");

            Assert.False(addressNonUk.IsUkAddress);
        }

        [Fact]
        public void TownOrCityCantBeNull()
        {
            Action createAddress =
                () => new Address("address1", "address2", null, "region", "postcode", "country");

            Assert.Throws<ArgumentNullException>(createAddress);
        }

        [Fact]
        public void CountryCantBeNull()
        {
            Action createAddress =
                () => new Address("address1", "address2", "town", "region", "postcode", null);

            Assert.Throws<ArgumentNullException>(createAddress);
        }

        [Fact]
        public void Address1CantBeNull()
        {
            Action createAddress =
                () => new Address(null, "address2", "town", "region", "postcode", "country");

            Assert.Throws<ArgumentNullException>(createAddress);
        }

        [Fact]
        public void TownOrCityCantBeEmpty()
        {
            Action createAddress =
                () => new Address("address1", "address2", string.Empty, "region", "postcode", "country");

            Assert.Throws<ArgumentException>(createAddress);
        }

        [Fact]
        public void CountryCantBeEmpty()
        {
            Action createAddress =
                () => new Address("address1", "address2", "town", "region", "postcode", string.Empty);

            Assert.Throws<ArgumentException>(createAddress);
        }

        [Fact]
        public void Address1CantBeEmpty()
        {
            Action createAddress =
                () => new Address(string.Empty, "address2", "town", "region", "postcode", "country");

            Assert.Throws<ArgumentException>(createAddress);
        }

        [Fact]
        public void Address2CanBeNull()
        {
            var address = new Address("address1", null, "town", "region", "postcode", "country");

            Assert.NotNull(address);
        }

        [Fact]
        public void RegionCanBeNull()
        {
            var address = new Address("address1", "town", "town", null, "postcode", "country");

            Assert.NotNull(address);
        }

        [Fact]
        public void PostcodeCanBeNullForNonUkAddress()
        {
            var address = new Address("address1", "address2", "town", "region", null, "Germany");

            Assert.NotNull(address);
        }

        [Fact]
        public void PostcodeCantBeNullForUkAddress()
        {
            Action createAddress =
                () => new Address("address1", "address2", "town", "region", null, "United Kingdom");

            Assert.Throws<InvalidOperationException>(createAddress);
        }
    }
}
