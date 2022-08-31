﻿namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotification.Draft;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class ProducerValidatorTests
    {
        private readonly ProducerValidator validator;

        [Obsolete]
        public ProducerValidatorTests()
        {
            var countryRepository = A.Fake<Domain.ICountryRepository>();
            var addressValidator = new AddressValidator(countryRepository);
            var contactValidator = new ContactValidator();
            validator = new ProducerValidator(addressValidator, contactValidator);
        }

        [Fact]
        public async void ValidProducer_ReturnsSuccess()
        {
            var producer = GetValidProducer();

            var result = await validator.ValidateAsync(producer);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ProducerAddressNonEmpty_IsValidated()
        {
            var producer = GetValidProducer();
            producer.Address.AddressLine1 = null;

            var result = await validator.ValidateAsync(producer);

            Assert.Equal("Address.AddressLine1", result.Errors.Single().PropertyName);
        }

        [Fact]
        public async Task ProducerContactNonEmpty_IsValidated()
        {
            var producer = GetValidProducer();
            producer.Contact.ContactName = null;

            var result = await validator.ValidateAsync(producer);

            Assert.Equal("Contact.ContactName", result.Errors.Single().PropertyName);
        }

        [Fact]
        public void ProducerAddressMissing_ReturnsFailure()
        {
            var producer = GetValidProducer();
            producer.Address = null;

            var result = validator.TestValidate(producer);
            result.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Fact]
        public async Task ProducerAddressMising_OnlyValidatesOnce()
        {
            var producer = GetValidProducer();
            producer.Address = null;

            var result = await validator.ValidateAsync(producer);

            Assert.Equal(1, result.Errors.Count(e => e.PropertyName == "Address"));
        }

        [Fact]
        public void ProducerAddressEmpty_ReturnsFailure()
        {
            var producer = GetValidProducer();
            producer.Address = new Address();

            var result = validator.TestValidate(producer);
            result.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Fact]
        public async Task ProducerAddressEmpty_DoesNotValidateAddress()
        {
            var producer = GetValidProducer();
            producer.Address = new Address();

            var result = await validator.ValidateAsync(producer);

            Assert.Empty(result.Errors.Where(e => e.PropertyName == "Address.AddressLine1"));
        }

        [Fact]
        public async void ProducerContactMissing_ReturnsFailure()
        {
            var producer = GetValidProducer();
            producer.Contact = null;

            var result = await validator.TestValidateAsync(producer);
            result.ShouldHaveValidationErrorFor(x => x.Contact);
        }

        [Fact]
        public async Task ProducerContactMising_OnlyValidatesOnce()
        {
            var producer = GetValidProducer();
            producer.Contact = null;

            var result = await validator.ValidateAsync(producer);

            Assert.Equal(1, result.Errors.Count(e => e.PropertyName == "Contact"));
        }

        [Fact]
        public async void ProducerContactEmpty_ReturnsFailure()
        {
            var producer = GetValidProducer();
            producer.Contact = new Contact();

            var result = await validator.TestValidateAsync(producer);
            result.ShouldHaveValidationErrorFor(x => x.Contact);
        }

        [Fact]
        public async Task ProducerContactEmpty_DoesNotValidateContact()
        {
            var producer = GetValidProducer();
            producer.Contact = new Contact();

            var result = await validator.ValidateAsync(producer);

            Assert.Empty(result.Errors.Where(e => e.PropertyName == "Contact.ContactName"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void BusinessNameMissing_ReturnsFailure(string businessName)
        {
            var producer = GetValidProducer();
            producer.BusinessName = businessName;

            var result = await validator.TestValidateAsync(producer);
            result.ShouldHaveValidationErrorFor(x => x.BusinessName);
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