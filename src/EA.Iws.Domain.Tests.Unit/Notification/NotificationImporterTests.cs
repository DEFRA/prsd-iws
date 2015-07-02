namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using Domain.Notification;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationImporterTests
    {
        [Fact]
        public void CanAddImporter()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = ObjectFactory.CreateEmptyBusiness();
            var address = ObjectFactory.CreateDefaultAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.SetImporter(business, address, contact);

            Assert.True(notification.HasImporter);
        }

        [Fact]
        public void AddSecondImporterReplacesFirst()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = Business.CreateBusiness("first", BusinessType.SoleTrader, "123", "456");
            var address = ObjectFactory.CreateDefaultAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.SetImporter(business, address, contact);

            var newBusiness = Business.CreateBusiness("second", BusinessType.SoleTrader, "123", "456");
            var newAddress = ObjectFactory.CreateDefaultAddress();
            var newContact = ObjectFactory.CreateEmptyContact();

            notification.SetImporter(newBusiness, newAddress, newContact);

            Assert.Equal("second", notification.Importer.Business.Name);
        }

        [Fact]
        public void HasImporterDefaultToFalse()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Assert.False(notification.HasImporter);
        }

        [Fact]
        public void RemoveImporterSetsToNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = ObjectFactory.CreateEmptyBusiness();
            var address = ObjectFactory.CreateDefaultAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.SetImporter(business, address, contact);

            notification.RemoveImporter();

            Assert.Null(notification.Importer);
        }

        [Fact]
        public void BusinessCantBeNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action setImporter =
                () =>
                    notification.SetImporter(null, ObjectFactory.CreateDefaultAddress(),
                        ObjectFactory.CreateEmptyContact());

            Assert.Throws<ArgumentNullException>("business", setImporter);
        }

        [Fact]
        public void AddressCantBeNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action setImporter =
                () =>
                    notification.SetImporter(ObjectFactory.CreateEmptyBusiness(), null,
                        ObjectFactory.CreateEmptyContact());

            Assert.Throws<ArgumentNullException>("address", setImporter);
        }

        [Fact]
        public void ContactCantBeNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action setImporter =
                () =>
                    notification.SetImporter(ObjectFactory.CreateEmptyBusiness(), ObjectFactory.CreateDefaultAddress(),
                        null);

            Assert.Throws<ArgumentNullException>("contact", setImporter);
        }
    }
}