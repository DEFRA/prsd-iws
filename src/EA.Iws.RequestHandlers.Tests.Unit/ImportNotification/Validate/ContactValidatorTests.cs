namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
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
            var contact = GetValidContact();

            var result = validator.Validate(contact);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ContactNameMissing_ReturnsFailure(string contactName)
        {
            var invalidContact = GetValidContact();
            invalidContact.ContactName = contactName;

            var result = validator.Validate(invalidContact);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void EmailMissing_ReturnsFailure(string email)
        {
            var invalidContact = GetValidContact();
            invalidContact.Email = email;

            var result = validator.Validate(invalidContact);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData("test")]
        [InlineData("test@test")]
        public void EmailInvalid_ReturnsFailure(string email)
        {
            var invalidContact = GetValidContact();
            invalidContact.Email = email;

            var result = validator.Validate(invalidContact);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TelephoneMissing_ReturnsFailure(string telephone)
        {
            var invalidContact = GetValidContact();
            invalidContact.Telephone = telephone;

            var result = validator.Validate(invalidContact);

            Assert.False(result.IsValid);
        }

        private Contact GetValidContact()
        {
            return new Contact
            {
                ContactName = "Mike Merry",
                Email = "mike@merry.com",
                Telephone = "01234 567890"
            };
        }
    }
}