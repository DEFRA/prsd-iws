namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class FacilityValidatorTests
    {
        private readonly FacilityValidator validator;

        [Obsolete]
        public FacilityValidatorTests()
        {
            var countryRepository = A.Fake<Domain.ICountryRepository>();
            var addressValidator = new AddressValidator(countryRepository);
            var contactValidator = new ContactValidator();
            validator = new FacilityValidator(addressValidator, contactValidator);
        }

        [Fact]
        public async void ValidFacility_ReturnsSuccess()
        {
            var facility = GetValidFacility();

            var result = await validator.ValidateAsync(facility);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task FacilityAddressNotEmpty_IsValidated()
        {
            var facility = GetValidFacility();
            facility.Address.AddressLine1 = null;

            var result = await validator.ValidateAsync(facility);

            Assert.Equal("Address.AddressLine1", result.Errors.Single().PropertyName);
        }

        [Fact]
        public async Task FacilityContactNotEmpty_IsValidated()
        {
            var facility = GetValidFacility();
            facility.Contact.ContactName = null;

            var result = await validator.ValidateAsync(facility);

            Assert.Equal("Contact.ContactName", result.Errors.Single().PropertyName);
        }

        [Fact]
        public void FacilityAddressMissing_ReturnsFailure()
        {
            var facility = GetValidFacility();
            facility.Address = null;

            var result = validator.TestValidate(facility);
            result.ShouldHaveValidationErrorFor(f => f.Address);
        }

        [Fact]
        public async Task FacilityAddressMising_OnlyValidatesOnce()
        {
            var facility = GetValidFacility();
            facility.Address = null;

            var result = await validator.ValidateAsync(facility);

            Assert.Equal(1, result.Errors.Count(e => e.PropertyName == "Address"));
        }

        [Fact]
        public void FacilityAddressEmpty_ReturnsFailure()
        {
            var facility = GetValidFacility();
            facility.Address = new Address();

            var result = validator.TestValidate(facility);
            result.ShouldHaveValidationErrorFor(f => f.Address);
        }

        [Fact]
        public async Task FacilityAddressEmpty_DoesNotValidateAddress()
        {
            var facility = GetValidFacility();
            facility.Address = new Address();

            var result = await validator.ValidateAsync(facility);

            Assert.Empty(result.Errors.Where(e => e.PropertyName == "Address.AddressLine1"));
        }

        [Fact]
        public async void FacilityContactMissing_ReturnsFailure()
        {
            var facility = GetValidFacility();
            facility.Contact = null;

            var result = await validator.TestValidateAsync(facility);
            result.ShouldHaveValidationErrorFor(f => f.Contact);
        }

        [Fact]
        public async Task FacilityContactMising_OnlyValidatesOnce()
        {
            var facility = GetValidFacility();
            facility.Contact = null;

            var result = await validator.ValidateAsync(facility);

            Assert.Equal(1, result.Errors.Count(e => e.PropertyName == "Contact"));
        }

        [Fact]
        public async void FacilityContactEmpty_ReturnsFailure()
        {
            var facility = GetValidFacility();
            facility.Contact = new Contact();

            var result = await validator.TestValidateAsync(facility);
            result.ShouldHaveValidationErrorFor(f => f.Contact);
        }

        [Fact]
        public async Task FacilityContactEmpty_DoesNotValidateContact()
        {
            var facility = GetValidFacility();
            facility.Contact = new Contact();

            var result = await validator.ValidateAsync(facility);

            Assert.Empty(result.Errors.Where(e => e.PropertyName == "Contact.ContactName"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void BusinessNameMissing_ReturnsFailure(string businessName)
        {
            var facility = GetValidFacility();
            facility.BusinessName = businessName;

            var result = await validator.TestValidateAsync(facility);
            result.ShouldHaveValidationErrorFor(f => f.BusinessName);
        }

        [Fact]
        public async void BusinessTypeMissing_ReturnsFailure()
        {
            var facility = GetValidFacility();
            facility.Type = null;

            var result = await validator.TestValidateAsync(facility);
            result.ShouldHaveValidationErrorFor(f => f.Type);
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