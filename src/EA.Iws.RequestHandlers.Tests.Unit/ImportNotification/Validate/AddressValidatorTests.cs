namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Domain;
    using FakeItEasy;
    using FluentValidation.TestHelper;
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
            validator.ShouldHaveValidationErrorFor(x => x.AddressLine1, address1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddressLine2Missing_ReturnsSuccessResult(string address2)
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.AddressLine2, address2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TownOrCityMissing_ReturnsFailureResult(string townOrCity)
        {
            validator.ShouldHaveValidationErrorFor(x => x.TownOrCity, townOrCity);
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

            validator.ShouldHaveValidationErrorFor(x => x.PostalCode, invalidAddress);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void PostalCodeMissing_CountryNotUK_ReturnsSuccessResult(string postcode)
        {
            validator.ShouldNotHaveValidationErrorFor(x => x.PostalCode, postcode);
        }

        [Fact]
        public void CountryMissing_ReturnsFailureResult()
        {
            validator.ShouldHaveValidationErrorFor(x => x.CountryId, null as Guid?);
        }
    }
}