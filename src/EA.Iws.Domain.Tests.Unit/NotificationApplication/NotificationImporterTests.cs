namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Importer;
    using TestHelpers.Helpers;
    using Xunit;
    using BusinessType = Domain.NotificationApplication.BusinessType;

    public class NotificationImporterTests
    {
        [Fact]
        public void CanCreateImporter()
        {
            var business = ObjectFactory.CreateEmptyBusiness();
            var address = ObjectFactory.CreateDefaultAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            var importer = new Importer(Guid.NewGuid(), address, business, contact);

            Assert.NotNull(importer);
        }

        [Fact]
        public void UpdateImporterReplacesFirst()
        {
            var business = Business.CreateBusiness("first", BusinessType.SoleTrader, "123", "456");
            var address = ObjectFactory.CreateDefaultAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            var importer = new Importer(Guid.NewGuid(), address, business, contact);

            var newBusiness = Business.CreateBusiness("second", BusinessType.SoleTrader, "123", "456");
            var newAddress = ObjectFactory.CreateDefaultAddress();
            var newContact = ObjectFactory.CreateEmptyContact();

            importer.Update(newAddress, newBusiness, newContact);

            Assert.Equal("second", importer.Business.Name);
        }

        [Fact]
        public void BusinessCantBeNull()
        {
            Action setImporter =
                () =>
                    new Importer(Guid.NewGuid(), ObjectFactory.CreateDefaultAddress(), null,
                        ObjectFactory.CreateEmptyContact());

            Assert.Throws<ArgumentNullException>("business", setImporter);
        }

        [Fact]
        public void AddressCantBeNull()
        {
            Action setImporter =
                () =>
                    new Importer(Guid.NewGuid(), null, ObjectFactory.CreateEmptyBusiness(),
                        ObjectFactory.CreateEmptyContact());

            Assert.Throws<ArgumentNullException>("address", setImporter);
        }

        [Fact]
        public void ContactCantBeNull()
        {
            Action setImporter =
                () =>
                    new Importer(Guid.NewGuid(), ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyBusiness(),
                        null);

            Assert.Throws<ArgumentNullException>("contact", setImporter);
        }
    }
}