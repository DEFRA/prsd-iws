namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
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
        public void ImporterAddressIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Address, typeof(AddressValidator));
        }

        [Fact]
        public void ImporterContactIsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Contact, typeof(ContactValidator));
        }

        [Fact]
        public void ImporterAddressMissing_ReturnsFailure()
        {
            var importer = GetValidImporter();
            importer.Address = null;

            validator.ShouldHaveValidationErrorFor(x => x.Address, importer);
        }

        [Fact]
        public void ImporterContactMissing_ReturnsFailure()
        {
            var importer = GetValidImporter();
            importer.Contact = null;

            validator.ShouldHaveValidationErrorFor(x => x.Contact, importer);
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