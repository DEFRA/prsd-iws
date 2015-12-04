namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class ProducerValidatorTests
    {
        private readonly ProducerValidator validator;

        public ProducerValidatorTests()
        {
            var countryRepository = A.Fake<Domain.ICountryRepository>();
            var addressValidator = new AddressValidator(countryRepository);
            var contactValidator = new ContactValidator();
            validator = new ProducerValidator(addressValidator, contactValidator);
        }

        [Fact]
        public void ValidProducer_ReturnsSuccess()
        {
            var producer = GetValidProducer();

            var result = validator.Validate(producer);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ProducerAddress_IsValidated()
        {
            validator.ShouldHaveChildValidator(x => x.Address, typeof(AddressValidator));
        }

        [Fact]
        public void ProducerContact_IsValidated()
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

        private Producer GetValidProducer()
        {
            return new Producer
            {
                Address = AddressTestData.GetValidTestAddress(),
                Contact = ContactTestData.GetValidTestContact(),
                BusinessName = "Mike and Eliot Bros.",
                AreMultiple = false
            };
        }
    }
}