namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Domain.ImportNotification;
    using EA.Iws.Core.Shared;
    using Xunit;

    public class ProducerTests
    {
        private readonly Guid importNotificationId = new Guid("1FE8238D-BD7A-4D3A-8188-704EFB2F62F4");
        private readonly Guid countryId = new Guid("1B685DF3-C190-4359-A81C-537E7AD5BA68");
        private readonly Address address;
        private readonly Contact contact;
        private readonly BusinessType businessType = BusinessType.LimitedCompany;
        private readonly string registrationNumber = "6465654";

        public ProducerTests()
        {
            address = new Address("line1", "line2", "town", "postcode", countryId);
            contact = new Contact("name", new PhoneNumber("123"), new EmailAddress("a@b.com"));
        }

        [Fact]
        public void CanCreateProducer()
        {
            var producer = new Producer(importNotificationId, "business name", address, contact, true, businessType, registrationNumber);

            Assert.IsType<Producer>(producer);
        }

        [Fact]
        public void BusinessNameCantBeNull()
        {
            Action createProducer = () => new Producer(importNotificationId, null, address, contact, true, businessType, registrationNumber);

            Assert.Throws<ArgumentNullException>("businessName", createProducer);
        }

        [Fact]
        public void BusinessNameCantBeEmpty()
        {
            Action createProducer = () => new Producer(importNotificationId, string.Empty, address, contact, true, businessType, registrationNumber);

            Assert.Throws<ArgumentException>("businessName", createProducer);
        }

        [Fact]
        public void ImportNotificationIdCantBeEmpty()
        {
            Action createProducer = () => new Producer(Guid.Empty, "business name", address, contact, true, businessType, registrationNumber);

            Assert.Throws<ArgumentException>("importNotificationId", createProducer);
        }

        [Fact]
        public void AddressCantBeNull()
        {
            Action createProducer = () => new Producer(importNotificationId, "business name", null, contact, true, businessType, registrationNumber);

            Assert.Throws<ArgumentNullException>("address", createProducer);
        }

        [Fact]
        public void ContactCantBeNull()
        {
            Action createProducer = () => new Producer(importNotificationId, "business name", address, null, true, businessType, registrationNumber);

            Assert.Throws<ArgumentNullException>("contact", createProducer);
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