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
        public void ValidAddress_ReturnsSuccessResult()
        {
            var address = AddressTestData.GetValidTestAddress();

            var result = validator.Validate(address);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddressLine1Missing_ReturnsFailureResult(string address1)
        {
            var model = new Address { AddressLine1 = address1 };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(address => address.AddressLine1);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddressLine2Missing_ReturnsSuccessResult(string address2)
        {
            var model = new Address { AddressLine2 = address2 };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(address => address.AddressLine2);

            //validator.ShouldNotHaveValidationErrorFor(x => x.AddressLine2, address2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TownOrCityMissing_ReturnsFailureResult(string townOrCity)
        {
            var model = new Address { TownOrCity = townOrCity };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(address => address.TownOrCity);

            //validator.ShouldHaveValidationErrorFor(x => x.TownOrCity, townOrCity);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void PostalCodeMissing_CountryUK_ReturnsFailureResult(string postcode)
        {
            var invalidAddress = AddressTestData.GetValidTestAddress();
            invalidAddress.CountryId = unitedKingdomCountryId;
            invalidAddress.PostalCode = postcode;

            var model = new Address { PostalCode = postcode };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(address => address.PostalCode);
            //validator.ShouldHaveValidationErrorFor(x => x.PostalCode, invalidAddress);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void PostalCodeMissing_CountryNotUK_ReturnsSuccessResult(string postcode)
        {
            var model = new Address { PostalCode = postcode };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(address => address.PostalCode);

            //validator.ShouldNotHaveValidationErrorFor(x => x.PostalCode, postcode);
        }

        [Fact]
        public void CountryMissing_ReturnsFailureResult()
        {
            var model = new Address { CountryId = null as Guid? };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(address => address.CountryId);

            //validator.ShouldHaveValidationErrorFor(x => x.CountryId, null as Guid?);
        }
    }
}