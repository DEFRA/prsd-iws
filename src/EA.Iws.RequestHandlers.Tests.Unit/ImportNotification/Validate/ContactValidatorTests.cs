namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using EA.Iws.Core.ImportNotification.Draft;
    using FluentValidation.TestHelper;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class ContactValidatorTests
    {
        private readonly ContactValidator validator;

        public ContactValidatorTests()
        {
            validator = new ContactValidator();
        }

        [Fact]
        public async void ValidContact_ReturnsSuccess()
        {
            var contact = ContactTestData.GetValidTestContact();

            var result = await validator.ValidateAsync(contact);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void ContactNameMissing_ReturnsFailure(string contactName)
        {
            var contact = ContactTestData.GetValidTestContact();
            contact.ContactName = contactName;

            var result = await validator.TestValidateAsync(contact);

            result.ShouldHaveValidationErrorFor(c => c.ContactName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void EmailMissing_ReturnsFailure(string email)
        {
            var model = new Contact { Email = email };
            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("@test.com")]
        public async void EmailInvalid_ReturnsFailure(string email)
        {
            var contact = ContactTestData.GetValidTestContact();
            contact.Email = email;

            var result = await validator.TestValidateAsync(contact);

            result.ShouldHaveValidationErrorFor(c => c.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void TelephoneMissing_ReturnsFailure(string telephone)
        {
            var model = new Contact { Telephone = telephone };
            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(c => c.Telephone);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void TelephonePrefixMissing_ReturnsFailure(string telephonePrefix)
        {
            var model = new Contact { TelephonePrefix = telephonePrefix };
            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(c => c.TelephonePrefix);
        }
    }
}