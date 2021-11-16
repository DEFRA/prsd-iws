namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Domain.ImportNotification;
    using EA.Iws.Core.Shared;
    using Xunit;

    public class ExporterTests
    {
        private readonly Guid importNotificationId = new Guid("1FE8238D-BD7A-4D3A-8188-704EFB2F62F4");
        private readonly Guid countryId = new Guid("1B685DF3-C190-4359-A81C-537E7AD5BA68");
        private readonly Address address;
        private readonly Contact contact;
        private readonly BusinessType businessType = BusinessType.LimitedCompany;
        private readonly string registrationNumber = "1112233";

        public ExporterTests()
        {
            address = new Address("line1", "line2", "town", "postcode", countryId);
            contact = new Contact("name", new PhoneNumber("123"), new EmailAddress("a@b.com"));
        }

        [Fact]
        public void CanCreateExporter()
        {
            var exporter = new Exporter(importNotificationId, "business name", address, contact, businessType, registrationNumber);

            Assert.IsType<Exporter>(exporter);
        }

        [Fact]
        public void BusinessNameCantBeNull()
        {
            Action createExporter = () => new Exporter(importNotificationId, null, address, contact, businessType, registrationNumber);

            Assert.Throws<ArgumentNullException>("businessName", createExporter);
        }

        [Fact]
        public void BusinessNameCantBeEmpty()
        {
            Action createExporter = () => new Exporter(importNotificationId, string.Empty, address, contact, businessType, registrationNumber);

            Assert.Throws<ArgumentException>("businessName", createExporter);
        }

        [Fact]
        public void ImportNotificationIdCantBeEmpty()
        {
            Action createExporter = () => new Exporter(Guid.Empty, "business name", address, contact, businessType, registrationNumber);

            Assert.Throws<ArgumentException>("importNotificationId", createExporter);
        }

        [Fact]
        public void AddressCantBeNull()
        {
            Action createExporter = () => new Exporter(importNotificationId, "business name", null, contact, businessType, registrationNumber);

            Assert.Throws<ArgumentNullException>("address", createExporter);
        }

        [Fact]
        public void ContactCantBeNull()
        {
            Action createExporter = () => new Exporter(importNotificationId, "business name", address, null, businessType, registrationNumber);

            Assert.Throws<ArgumentNullException>("contact", createExporter);
        }

        [Fact]
        public void BusinessTypeCantBeDefault()
        {
            Action createImporter = () => new Importer(importNotificationId, "business name", default(BusinessType), "reg no", address, contact);

            Assert.Throws<ArgumentException>("businessType", createImporter);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RegistrationNumberCanBeNullOrEmpty(string regNumber)
        {
            var importer = new Importer(importNotificationId, "business name", businessType, regNumber, address, contact);

            Assert.IsType<Importer>(importer);
        }
    }
}