namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Domain.TransportRoute;
    using TestHelpers.Helpers;
    using Xunit;

    public class CustomsOfficeTests
    {
        private Country country;

        public CustomsOfficeTests()
        {
            country = CountryFactory.Create(new Guid("C294A7AE-D95D-416D-A8DA-F2148EA5F916"));
        }

        [Theory]
        [InlineData("name", null)]
        [InlineData(null, "address")]
        [InlineData(null, null)]
        public void CreateExitOffice_NullArguments_Throws(string name, string address)
        {
            Assert.Throws<ArgumentNullException>(() => new ExitCustomsOffice(name, address, country));
        }

        [Fact]
        public void CreateExitOffice_NonEuropeanCountry_Throws()
        {
            country = CountryFactory.Create(new Guid("0A5C8F28-1061-4017-A0E1-BCE0CAD5B90A"), "test", false);

            Assert.Throws<InvalidOperationException>(() => new ExitCustomsOffice("name", "address", country));
        }

        [Theory]
        [InlineData("name", null)]
        [InlineData(null, "address")]
        [InlineData(null, null)]
        public void CreateEntryOffice_NullArguments_Throws(string name, string address)
        {
            Assert.Throws<ArgumentNullException>(() => new EntryCustomsOffice(name, address, country));
        }

        [Fact]
        public void CreateEntryOffice_NonEuropeanCountry_Throws()
        {
            country = CountryFactory.Create(new Guid("0A5C8F28-1061-4017-A0E1-BCE0CAD5B90A"), "test", false);

            Assert.Throws<InvalidOperationException>(() => new EntryCustomsOffice("name", "address", country));
        }
    }
}
