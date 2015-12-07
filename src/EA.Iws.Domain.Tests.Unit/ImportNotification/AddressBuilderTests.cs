namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Domain.ImportNotification;
    using FakeItEasy;
    using TestHelpers.Helpers;
    using Xunit;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "It's a unit test")]
    public class AddressBuilderTests
    {
        private readonly AddressBuilder addressBuilder;
        private readonly ICountryRepository countryRepository;
        private readonly Guid ukId = new Guid("E2809744-F543-40B0-8F11-64EB0AD9365F");
        private readonly Guid nonUkId = new Guid("D3DED9D6-8422-4FE7-969B-EC61A66712E0");

        public AddressBuilderTests()
        {
            countryRepository = A.Fake<ICountryRepository>();

            var uk = CountryFactory.Create(ukId, "United Kingdom");
            var france = CountryFactory.Create(nonUkId, "France");

            A.CallTo(() => countryRepository.GetById(ukId)).Returns(uk);
            A.CallTo(() => countryRepository.GetById(nonUkId)).Returns(france);

            addressBuilder = new AddressBuilder(countryRepository);
        }

        [Fact]
        public void CanCreateAddress()
        {
            var address = addressBuilder
                .Create("address 1", "town", nonUkId)
                .ToAddress();

            Assert.IsAssignableFrom<Address>(address);
        }

        [Fact]
        public void CanCreateAddressWithAddressLine2()
        {
            var address = addressBuilder
                .Create("address 1", "town", nonUkId)
                .WithAddressLine2("address 2")
                .ToAddress();

            Assert.IsAssignableFrom<Address>(address);
        }

        [Fact]
        public void CanCreateAddressWithPostalCode()
        {
            var address = addressBuilder
                .Create("address 1", "town", nonUkId)
                .WithPostalCode("AA11AA")
                .ToAddress();

            Assert.IsAssignableFrom<Address>(address);
        }

        [Fact]
        public void PostalCodeRequiredWhenCountryIsUk()
        {
            var address = addressBuilder
                .Create("address 1", "town", ukId);

            Action toAddress = () => address.ToAddress();

            Assert.Throws<InvalidOperationException>(toAddress);
        }

        [Fact]
        public void CanCreateUkAddressWithPostalCode()
        {
            var address = addressBuilder
                .Create("address 1", "town", ukId)
                .WithPostalCode("AA11AA")
                .ToAddress();

            Assert.IsAssignableFrom<Address>(address);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void AddressLine1CantBeNullOrEmpty(string input, Type expectedException)
        {
            Action createAddress = () => addressBuilder.Create(input, "town", nonUkId);

            Assert.Throws(expectedException, createAddress);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void TownCantBeNullOrEmpty(string input, Type expectedException)
        {
            Action createAddress = () => addressBuilder.Create("line 1", input, nonUkId);

            Assert.Throws(expectedException, createAddress);
        }

        [Fact]
        public void CountryIdCantBeDefault()
        {
            Action createAddress = () => addressBuilder.Create("line 1", "town", Guid.Empty);

            Assert.Throws<ArgumentException>(createAddress);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void AddressLine2CantBeNullOrEmpty(string input, Type expectedException)
        {
            Action createAddress = () => addressBuilder.Create("line 1", "town", nonUkId).WithAddressLine2(input);

            Assert.Throws(expectedException, createAddress);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void PostalCodeCantBeNullOrEmpty(string input, Type expectedException)
        {
            Action createAddress = () => addressBuilder.Create("line 1", "town", nonUkId).WithPostalCode(input);

            Assert.Throws(expectedException, createAddress);
        }
    }
}
