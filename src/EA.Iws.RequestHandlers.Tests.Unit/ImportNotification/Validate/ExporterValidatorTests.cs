namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class ExporterValidatorTests
    {
        private readonly ExporterValidator validator;

        public ExporterValidatorTests()
        {
            var countryRepository = A.Fake<Domain.ICountryRepository>();
            var addressValidator = new AddressValidator(countryRepository);
            var contactValidator = new ContactValidator();
            validator = new ExporterValidator(addressValidator, contactValidator);
        }

        [Fact]
        public void ValidExporter_ReturnsSuccess()
        {
            var exporter = GetValidExporter();

            var result = validator.Validate(exporter);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ExporterAddress_IsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Address, typeof(AddressValidator));
        }

        [Fact]
        public void ExporterContact_IsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Contact, typeof(ContactValidator));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BusinessNameMissing_ReturnsFailure(string businessName)
        {
            validator.ShouldHaveValidationErrorFor(x => x.BusinessName, businessName);
        }

        private Exporter GetValidExporter()
        {
            return new Exporter
            {
                Address = AddressTestData.GetValidTestAddress(),
                Contact = ContactTestData.GetValidTestContact(),
                BusinessName = "Mike and Eliot Bros."
            };
        }
    }
}