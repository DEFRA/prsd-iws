namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class FacilityValidatorTests
    {
        private readonly FacilityValidator validator;

        public FacilityValidatorTests()
        {
            var countryRepository = A.Fake<Domain.ICountryRepository>();
            var addressValidator = new AddressValidator(countryRepository);
            var contactValidator = new ContactValidator();
            validator = new FacilityValidator(addressValidator, contactValidator);
        }

        [Fact]
        public void ValidFacility_ReturnsSuccess()
        {
            var facility = GetValidFacility();

            var result = validator.Validate(facility);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void FacilityAddress_IsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Address, typeof(AddressValidator));
        }

        [Fact]
        public void FacilityContact_IsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Contact, typeof(ContactValidator));
        }

        [Fact]
        public void FacilityAddressMissing_ReturnsFailure()
        {
            var facility = GetValidFacility();
            facility.Address = null;

            validator.ShouldHaveValidationErrorFor(x => x.Address, facility);
        }

        [Fact]
        public void FacilityContactMissing_ReturnsFailure()
        {
            var facility = GetValidFacility();
            facility.Contact = null;

            validator.ShouldHaveValidationErrorFor(x => x.Contact, facility);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BusinessNameMissing_ReturnsFailure(string businessName)
        {
            var facility = GetValidFacility();
            facility.BusinessName = businessName;

            validator.ShouldHaveValidationErrorFor(x => x.BusinessName, facility);
        }

        [Fact]
        public void BusinessTypeMissing_ReturnsFailure()
        {
            var facility = GetValidFacility();
            facility.Type = null;

            validator.ShouldHaveValidationErrorFor(x => x.Type, facility);
        }

        private Facility GetValidFacility()
        {
            return new Facility(new Guid("D39DA9E3-0E5F-4DA4-8560-3743029CE76F"))
            {
                Address = AddressTestData.GetValidTestAddress(),
                Contact = ContactTestData.GetValidTestContact(),
                BusinessName = "Woking Waste Facility",
                Id = new Guid("B86BBCD0-9196-4E54-BDB2-3E65180D83DD"),
                RegistrationNumber = "99462556AAB88",
                Type = BusinessType.Other,
                IsActualSite = true
            };
        }
    }
}