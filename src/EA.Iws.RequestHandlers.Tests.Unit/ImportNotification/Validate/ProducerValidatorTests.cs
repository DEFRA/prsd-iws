namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
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

        [Fact]
        public void ProducerAddressMissing_ReturnsFailure()
        {
            var producer = GetValidProducer();
            producer.Address = null;

            validator.ShouldHaveValidationErrorFor(x => x.Address, producer);
        }

        [Fact]
        public void ProducerContactMissing_ReturnsFailure()
        {
            var producer = GetValidProducer();
            producer.Contact = null;

            validator.ShouldHaveValidationErrorFor(x => x.Contact, producer);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void BusinessNameMissing_ReturnsFailure(string businessName)
        {
            var producer = GetValidProducer();
            producer.BusinessName = businessName;

            validator.ShouldHaveValidationErrorFor(x => x.BusinessName, producer);
        }

        private Producer GetValidProducer()
        {
            return new Producer(new Guid("F69C8CF2-ADF7-4065-B008-B57871E85A96"))
            {
                Address = AddressTestData.GetValidTestAddress(),
                Contact = ContactTestData.GetValidTestContact(),
                BusinessName = "Mike and Eliot Bros.",
                AreMultiple = false
            };
        }
    }
}