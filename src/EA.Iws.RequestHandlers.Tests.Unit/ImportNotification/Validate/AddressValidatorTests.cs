namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Domain;
    using FakeItEasy;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;
    using Address = Core.ImportNotification.Draft.Address;

    public class AddressValidatorTests
    {
        private readonly AddressValidator validator;
        private readonly Guid unitedKingdomCountryId = new Guid("73973C74-5981-4895-8785-9FD72B29F528");
        private readonly Guid anyCountryId = new Guid("8BB51831-AB8B-4951-8DEC-962E77E59CEC");
        private readonly ICountryRepository countryRepository;

        public AddressValidatorTests()
        {
            countryRepository = A.Fake<ICountryRepository>();
            A.CallTo(() => countryRepository.GetUnitedKingdomId()).Returns(unitedKingdomCountryId);
            validator = new AddressValidator(countryRepository);
        }

        [Fact]
        public void ValidAddress_ReturnsSuccessResult()
        {
            var address = GetValidAddress();

            var result = validator.Validate(address);

            Assert.True(result.IsValid);
        }

        private Address GetValidAddress()
        {
            return new Address
            {
                AddressLine1 = "Eliot House",
                AddressLine2 = "Eliot Lane",
                CountryId = anyCountryId,
                PostalCode = "EL10TJ",
                TownOrCity = "Eliotsville"
            };
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddressLine1Missing_ReturnsFailureResult(string address1)
        {
            var invalidAddress = GetValidAddress();
            invalidAddress.AddressLine1 = address1;

            var result = validator.Validate(invalidAddress);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddressLine2Missing_ReturnsSuccessResult(string address2)
        {
            var validAddress = GetValidAddress();
            validAddress.AddressLine2 = address2;

            var result = validator.Validate(validAddress);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TownOrCityMissing_ReturnsFailureResult(string townOrCity)
        {
            var invalidAddress = GetValidAddress();
            invalidAddress.TownOrCity = townOrCity;

            var result = validator.Validate(invalidAddress);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void PostalCodeMissing_CountryUK_ReturnsFailureResult(string postcode)
        {
            var invalidAddress = GetValidAddress();
            invalidAddress.CountryId = unitedKingdomCountryId;
            invalidAddress.PostalCode = postcode;

            var result = validator.Validate(invalidAddress);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void PostalCodeMissing_CountryNotUK_ReturnsSuccessResult(string postcode)
        {
            var invalidAddress = GetValidAddress();
            invalidAddress.PostalCode = postcode;

            var result = validator.Validate(invalidAddress);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void CountryMissing_ReturnsFailureResult()
        {
            var invalidAddress = GetValidAddress();
            invalidAddress.CountryId = null;

            var result = validator.Validate(invalidAddress);

            Assert.False(result.IsValid);
        }
    }
}