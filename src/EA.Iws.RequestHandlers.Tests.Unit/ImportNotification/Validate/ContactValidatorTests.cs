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
        public void ValidContact_ReturnsSuccess()
        {
            var contact = ContactTestData.GetValidTestContact();

            var result = validator.Validate(contact);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ContactNameMissing_ReturnsFailure(string contactName)
        {
            var model = new Contact { ContactName = contactName };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c.ContactName);

            //validator.ShouldHaveValidationErrorFor(x => x.ContactName, contactName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void EmailMissing_ReturnsFailure(string email)
        {
            var model = new Contact { Email = email };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c.Email);

            //validator.ShouldHaveValidationErrorFor(x => x.Email, email);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test@test")]
        public void EmailInvalid_ReturnsFailure(string email)
        {
            var model = new Contact { Email = email };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c.Email);

            //validator.ShouldHaveValidationErrorFor(x => x.Email, email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TelephoneMissing_ReturnsFailure(string telephone)
        {
            var model = new Contact { Telephone = telephone };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c.Telephone);

            //validator.ShouldHaveValidationErrorFor(x => x.Telephone, telephone);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TelephonePrefixMissing_ReturnsFailure(string telephonePrefix)
        {
            var model = new Contact { TelephonePrefix = telephonePrefix };
            var result = validator.TestValidate(model);

            result.ShouldHaveValidationErrorFor(c => c.TelephonePrefix);

            //validator.ShouldHaveValidationErrorFor(x => x.TelephonePrefix, telephonePrefix);
        }
    }
}