namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Domain.ImportNotification;
    using Xunit;

    public class ContactTests
    {
        [Fact]
        public void CanCreateContact()
        {
            var contact = new Contact("Bob", new PhoneNumber("01234 567890"), new EmailAddress("bob@email.com"));

            Assert.IsType<Contact>(contact);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void NameCantBeNullOrEmpty(string input, Type expectedException)
        {
            Action createContact = () => new Contact(input, new PhoneNumber("01234 567890"), new EmailAddress("bob@email.com"));

            Assert.Throws(expectedException, createContact);
        }

        [Fact]
        public void PhoneNumberCantBeNull()
        {
            Action createContact = () => new Contact("Jeff", null, new EmailAddress("jeff@email.com"));

            Assert.Throws<ArgumentNullException>("phoneNumber", createContact);
        }

        [Fact]
        public void EmailAddressCantBeNull()
        {
            Action createContact = () => new Contact("Jeff", new PhoneNumber("01234 567890"), null);

            Assert.Throws<ArgumentNullException>("emailAddress", createContact);
        }
    }
}