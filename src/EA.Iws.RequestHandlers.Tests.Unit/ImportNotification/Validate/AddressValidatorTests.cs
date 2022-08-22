namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Domain;
    using EA.Iws.Core.ImportNotification.Draft;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class AddressValidatorTests
    {
        private readonly AddressValidator validator;
        private readonly Guid unitedKingdomCountryId = new Guid("73973C74-5981-4895-8785-9FD72B29F528");
        private readonly ICountryRepository countryRepository;

        public AddressValidatorTests()
        {
            countryRepository = A.Fake<ICountryRepository>();
            A.CallTo(() => countryRepository.GetUnitedKingdomId()).Returns(unitedKingdomCountryId);
            validator = new AddressValidator(countryRepository);
        }

        [Fact]
        public async void ValidAddress_ReturnsSuccessResult()
        {
            var address = AddressTestData.GetValidTestAddress();

            var result = await validator.ValidateAsync(address);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void AddressLine1Missing_ReturnsFailureResult(string address1)
        {
            var model = new Address { AddressLine1 = address1 };
            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(address => address.AddressLine1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void AddressLine2Missing_ReturnsSuccessResult(string address2)
        {
            var invalidAddress = AddressTestData.GetValidTestAddress();
            invalidAddress.CountryId = unitedKingdomCountryId;
            invalidAddress.AddressLine2 = address2;

            var result = await validator.TestValidateAsync(invalidAddress);

            result.ShouldNotHaveValidationErrorFor(address => address.AddressLine2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void TownOrCityMissing_ReturnsFailureResult(string townOrCity)
        {
            var invalidAddress = AddressTestData.GetValidTestAddress();
            invalidAddress.CountryId = unitedKingdomCountryId;
            invalidAddress.TownOrCity = townOrCity;

            var result = await validator.TestValidateAsync(invalidAddress);

            result.ShouldHaveValidationErrorFor(address => address.TownOrCity);            
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void PostalCodeMissing_CountryUK_ReturnsFailureResult(string postcode)
        {
            var invalidAddress = AddressTestData.GetValidTestAddress();
            invalidAddress.CountryId = unitedKingdomCountryId;
            invalidAddress.PostalCode = postcode;
            
            var result = await validator.TestValidateAsync(invalidAddress);

            result.ShouldHaveValidationErrorFor(address => address.PostalCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void PostalCodeMissing_CountryNotUK_ReturnsSuccessResult(string postcode)
        {
            var model = new Address { PostalCode = postcode };
            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(address => address.PostalCode);
        }

        [Fact]
        public async void CountryMissing_ReturnsFailureResult()
        {
            var model = new Address { CountryId = null as Guid? };
            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(address => address.CountryId);
        }
    }
}