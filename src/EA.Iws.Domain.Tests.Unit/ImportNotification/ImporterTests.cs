namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Core.Shared;
    using Domain.ImportNotification;
    using Xunit;

    public class ImporterTests
    {
        private readonly Guid importNotificationId = new Guid("1FE8238D-BD7A-4D3A-8188-704EFB2F62F4");
        private readonly Guid countryId = new Guid("1B685DF3-C190-4359-A81C-537E7AD5BA68");
        private readonly Address address;
        private readonly Contact contact;

        public ImporterTests()
        {
            address = new Address("line1", "line2", "town", "postcode", countryId);
            contact = new Contact("name", new PhoneNumber("123"), new EmailAddress("a@b.com"));
        }

        [Fact]
        public void CanCreateImporter()
        {
            var importer = new Importer(importNotificationId, "business name", BusinessType.LimitedCompany, "reg no", address, contact);

            Assert.IsType<Importer>(importer);
        }

        [Fact]
        public void ImportNotificationIdCantBeEmpty()
        {
            Action createImporter = () => new Importer(Guid.Empty, "business name", BusinessType.LimitedCompany, "reg no", address, contact);

            Assert.Throws<ArgumentException>("importNotificationId", createImporter);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void BusinessNameCantBeNullOrEmpty(string input, Type expectedException)
        {
            Action createImporter = () => new Importer(importNotificationId, input, BusinessType.LimitedCompany, "reg no", address, contact);

            Assert.Throws(expectedException, createImporter);
        }

        [Theory]
        [InlineData(null, typeof(ArgumentNullException))]
        [InlineData("", typeof(ArgumentException))]
        public void RegistrationNumberCantBeNullOrEmpty(string input, Type expectedException)
        {
            Action createImporter = () => new Importer(importNotificationId, "business name", BusinessType.LimitedCompany, input, address, contact);

            Assert.Throws(expectedException, createImporter);
        }

        [Fact]
        public void BusinessTypeCantBeDefault()
        {
            Action createImporter = () => new Importer(importNotificationId, "business name", default(BusinessType), "reg no", address, contact);

            Assert.Throws<ArgumentException>("businessType", createImporter);
        }

        [Fact]
        public void AddressCantBeNull()
        {
            Action createImporter = () => new Importer(importNotificationId, "business name", BusinessType.LimitedCompany, "reg no", null, contact);

            Assert.Throws<ArgumentNullException>("address", createImporter);
        }

        [Fact]
        public void ContactCantBeNull()
        {
            Action createImporter = () => new Importer(importNotificationId, "business name", BusinessType.LimitedCompany, "reg no", address, null);

            Assert.Throws<ArgumentNullException>("contact", createImporter);
        }
    }
}
