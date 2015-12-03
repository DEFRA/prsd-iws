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
                Address = new Address
                {
                    AddressLine1 = "Eliot House",
                    AddressLine2 = "Eliot Lane",
                    CountryId = new System.Guid("0A323947-78A0-40FE-BDDD-9A58803559BC"),
                    PostalCode = "EL10TJ",
                    TownOrCity = "Eliotsville"
                },
                Contact = new Contact
                {
                    ContactName = "Mike Merry",
                    Email = "mike@merry.com",
                    Telephone = "01234 567890"
                },
                BusinessName = "Mike and Eliot Bros."
            };
        }
    }
}