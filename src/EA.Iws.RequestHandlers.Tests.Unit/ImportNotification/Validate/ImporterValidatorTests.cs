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

    public class ImporterValidatorTests
    {
        private readonly ImporterValidator validator;

        public ImporterValidatorTests()
        {
            var countryRepository = A.Fake<Domain.ICountryRepository>();
            var addressValidator = new AddressValidator(countryRepository);
            var contactValidator = new ContactValidator();
            validator = new ImporterValidator(addressValidator, contactValidator);
        }

        [Fact]
        public void ImporterValid_ReturnsSuccess()
        {
            var importer = GetValidImporter();

            var result = validator.Validate(importer);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ImporterAddressNotEmpty_IsValidated()
        {
            var importer = GetValidImporter();
            importer.Address.AddressLine1 = null;

            var result = await validator.ValidateAsync(importer);

            Assert.Equal("Address.AddressLine1", result.Errors.Single().PropertyName);
        }

        [Fact]
        public async Task ImporterContactNotEmpty_IsValidated()
        {
            var importer = GetValidImporter();
            importer.Contact.ContactName = null;

            var result = await validator.ValidateAsync(importer);

            Assert.Equal("Contact.ContactName", result.Errors.Single().PropertyName);
        }

        [Fact]
        public void ImporterAddressMissing_ReturnsFailure()
        {
            var importer = GetValidImporter();
            importer.Address = null;

            validator.ShouldHaveValidationErrorFor(x => x.Address, importer);
        }

        [Fact]
        public async Task ImporterAddressMising_OnlyValidatesOnce()
        {
            var importer = GetValidImporter();
            importer.Address = null;

            var result = await validator.ValidateAsync(importer);

            Assert.Equal(1, result.Errors.Count(e => e.PropertyName == "Address"));
        }

        [Fact]
        public void ImporterAddressEmpty_ReturnsFailure()
        {
            var importer = GetValidImporter();
            importer.Address = new Address();

            validator.ShouldHaveValidationErrorFor(x => x.Address, importer);
        }

        [Fact]
        public async Task ImporterAddressEmpty_DoesNotValidateAddress()
        {
            var importer = GetValidImporter();
            importer.Address = new Address();

            var result = await validator.ValidateAsync(importer);

            Assert.Empty(result.Errors.Where(e => e.PropertyName == "Address.AddressLine1"));
        }

        [Fact]
        public void ImporterContactMissing_ReturnsFailure()
        {
            var importer = GetValidImporter();
            importer.Contact = null;

            validator.ShouldHaveValidationErrorFor(x => x.Contact, importer);
        }

        [Fact]
        public async Task ImporterContactMising_OnlyValidatesOnce()
        {
            var importer = GetValidImporter();
            importer.Contact = null;

            var result = await validator.ValidateAsync(importer);

            Assert.Equal(1, result.Errors.Count(e => e.PropertyName == "Contact"));
        }

        [Fact]
        public void ImporterContactEmpty_ReturnsFailure()
        {
            var importer = GetValidImporter();
            importer.Contact = new Contact();

            validator.ShouldHaveValidationErrorFor(x => x.Contact, importer);
        }

        [Fact]
        public async Task ImporterContactEmpty_DoesNotValidateContact()
        {
            var importer = GetValidImporter();
            importer.Contact = new Contact();

            var result = await validator.ValidateAsync(importer);

            Assert.Empty(result.Errors.Where(e => e.PropertyName == "Contact.ContactName"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BusinessNameMissing_ReturnsFailure(string businessName)
        {
            var importer = GetValidImporter();
            importer.BusinessName = businessName;

            validator.ShouldHaveValidationErrorFor(x => x.BusinessName, importer);
        }

        [Fact]
        public void BusinessTypeMissing_ReturnsFailure()
        {
            var importer = GetValidImporter();
            importer.Type = null;

            validator.ShouldHaveValidationErrorFor(x => x.Type, importer);
        }

        private Importer GetValidImporter()
        {
            return new Importer(new Guid("0C2A45DE-CEF1-4BD7-92C5-1B400CEDB99C"))
            {
                Address = AddressTestData.GetValidTestAddress(),
                Contact = ContactTestData.GetValidTestContact(),
                BusinessName = "Mike and Eliot Bros.",
                RegistrationNumber = "12987457",
                Type = BusinessType.Other
            };
        }
    }
}