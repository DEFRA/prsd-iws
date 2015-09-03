namespace EA.Iws.RequestHandlers.Tests.Unit.Mappings
{
    using RequestHandlers.Mappings;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class AddressMapTests
    {
        private const string AnyString = "test";
        private const string TestString = "Pickled peppers";

        private readonly AddressMap addressMap;
        private readonly TestableAddress testAddress = new TestableAddress
            {
                Address1 = AnyString,
                Address2 = AnyString,
                Country = TestableCountry.France.Name,
                PostalCode = AnyString,
                Region = AnyString,
                TownOrCity = AnyString
            };

        public AddressMapTests()
        {
            var context = new TestIwsContext();
            addressMap = new AddressMap(context);

            context.Countries.Add(TestableCountry.UnitedKingdom);
            context.Countries.Add(TestableCountry.Switzerland);
            context.Countries.Add(TestableCountry.France);
        }

        [Fact]
        public void MapsCorrectCountryId()
        {
            testAddress.Country = TestableCountry.UnitedKingdom.Name;

            var result = addressMap.Map(testAddress);

            Assert.Equal(TestableCountry.UnitedKingdom.Id, result.CountryId);
        }

        [Fact]
        public void MapsCorrectCountryName()
        {
            testAddress.Country = TestableCountry.UnitedKingdom.Name;

            var result = addressMap.Map(testAddress);

            Assert.Equal(TestableCountry.UnitedKingdom.Name, result.CountryName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(TestString)]
        public void MapRegion(string region)
        {
            testAddress.Region = region;

            var result = addressMap.Map(testAddress);

            Assert.Equal(region, result.Region);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(TestString)]
        public void MapPostalCode(string postalCode)
        {
            testAddress.PostalCode = postalCode;

            var result = addressMap.Map(testAddress);

            Assert.Equal(postalCode, result.PostalCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(TestString)]
        public void MapTownOrCity(string townOrCity)
        {
            testAddress.TownOrCity = townOrCity;

            var result = addressMap.Map(testAddress);

            Assert.Equal(townOrCity, result.TownOrCity);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(TestString)]
        public void MapAddress2(string address2)
        {
            testAddress.Address2 = address2;

            var result = addressMap.Map(testAddress);

            Assert.Equal(address2, result.Address2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(TestString)]
        public void MapAddress1(string address1)
        {
            testAddress.Address1 = address1;

            var result = addressMap.Map(testAddress);

            Assert.Equal(address1, result.StreetOrSuburb);
        }
    }
}
