namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotification.Draft;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class ExporterValidatorTests
    {
        private readonly ExporterValidator validator;

        [Obsolete]
        public ExporterValidatorTests()
        {
            var countryRepository = A.Fake<Domain.ICountryRepository>();
            var addressValidator = new AddressValidator(countryRepository);
            var contactValidator = new ContactValidator();
            validator = new ExporterValidator(addressValidator, contactValidator);
        }

        [Fact]
        public async void ValidExporter_ReturnsSuccess()
        {
            var exporter = GetValidExporter();

            var result = await validator.ValidateAsync(exporter);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ExporterAddressNotEmpty_IsValidated()
        {
            var exporter = GetValidExporter();
            exporter.Address.AddressLine1 = null;

            var result = await validator.ValidateAsync(exporter);

            Assert.Equal("Address.AddressLine1", result.Errors.Single().PropertyName);
        }

        [Fact]
        public async Task ExporterContactNotEmpty_IsValidated()
        {
            var exporter = GetValidExporter();
            exporter.Contact.ContactName = null;

            var result = await validator.ValidateAsync(exporter);

            Assert.Equal("Contact.ContactName", result.Errors.Single().PropertyName);
        }

        [Fact]
        public void ExporterAddressMissing_ReturnsFailure()
        {
            var exporter = GetValidExporter();
            exporter.Address = null;
            
            var result = validator.TestValidate(exporter);
            result.ShouldHaveValidationErrorFor(c => c.Address);
        }

        [Fact]
        public void ExporterAddressEmpty_ReturnsFailure()
        {
            var exporter = GetValidExporter();
            exporter.Address = new Address();

            var result = validator.TestValidate(exporter);
            result.ShouldHaveValidationErrorFor(c => c.Address);
        }

        [Fact]
        public async Task ExporterAddressMising_OnlyValidatesOnce()
        {
            var exporter = GetValidExporter();
            exporter.Address = null;

            var result = await validator.ValidateAsync(exporter);

            Assert.Equal(1, result.Errors.Count(e => e.PropertyName == "Address"));
        }

        [Fact]
        public async Task ExporterAddressEmpty_DoesNotValidateAddress()
        {
            var exporter = GetValidExporter();
            exporter.Address = new Address();

            var result = await validator.ValidateAsync(exporter);

            Assert.Empty(result.Errors.Where(e => e.PropertyName == "Address.AddressLine1"));
        }

        [Fact]
        public async void ExporterContactMissing_ReturnsFailure()
        {
            var exporter = GetValidExporter();
            exporter.Contact  = null;

            var result = await validator.TestValidateAsync(exporter);
            result.ShouldHaveValidationErrorFor(c => c.Contact);
        }

        [Fact]
        public async Task ExporterContactMising_OnlyValidatesOnce()
        {
            var exporter = GetValidExporter();
            exporter.Contact = null;

            var result = await validator.ValidateAsync(exporter);

            Assert.Equal(1, result.Errors.Count(e => e.PropertyName == "Contact"));
        }

        [Fact]
        public async void ExporterContactEmpty_ReturnsFailure()
        {
            var exporter = GetValidExporter();
            exporter.Contact = new Contact();

            var result = await validator.TestValidateAsync(exporter);
            result.ShouldHaveValidationErrorFor(c => c.Contact);
        }

        [Fact]
        public async Task ExporterContactEmpty_DoesNotValidateContact()
        {
            var exporter = GetValidExporter();
            exporter.Contact = new Contact();

            var result = await validator.ValidateAsync(exporter);

            Assert.Empty(result.Errors.Where(e => e.PropertyName == "Contact.ContactName"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void BusinessNameMissing_ReturnsFailure(string businessName)
        {
            var exporter = GetValidExporter();
            exporter.BusinessName = businessName;

            var result = await validator.TestValidateAsync(exporter);
            result.ShouldHaveValidationErrorFor(c => c.BusinessName);
        }

        private Exporter GetValidExporter()
        {
            return new Exporter(new Guid("7C898A2C-D940-4F2E-BB89-6301B99B69A3"))
            {
                Address = AddressTestData.GetValidTestAddress(),
                Contact = ContactTestData.GetValidTestContact(),
                BusinessName = "Mike and Eliot Bros."
            };
        }
    }
}