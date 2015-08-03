namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationExporterTests
    {
        [Fact]
        public void CanAddExporter()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = ObjectFactory.CreateEmptyBusiness();
            var address = ObjectFactory.CreateDefaultAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.SetExporter(business, address, contact);

            Assert.True(notification.HasExporter);
        }

        [Fact]
        public void AddSecondExporterReplacesFirst()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = Business.CreateBusiness("first", BusinessType.SoleTrader, "123", "456");
            var address = ObjectFactory.CreateDefaultAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.SetExporter(business, address, contact);

            var newBusiness = Business.CreateBusiness("second", BusinessType.SoleTrader, "123", "456");
            var newAddress = ObjectFactory.CreateDefaultAddress();
            var newContact = ObjectFactory.CreateEmptyContact();

            notification.SetExporter(newBusiness, newAddress, newContact);

            Assert.Equal("second", notification.Exporter.Business.Name);
        }

        [Fact]
        public void HasExporterDefaultToFalse()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Assert.False(notification.HasExporter);
        }

        [Fact]
        public void RemoveExporterSetsToNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = ObjectFactory.CreateEmptyBusiness();
            var address = ObjectFactory.CreateDefaultAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.SetExporter(business, address, contact);

            notification.RemoveExporter();

            Assert.Null(notification.Exporter);
        }

        [Fact]
        public void BusinessCantBeNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action setExporter =
                () =>
                    notification.SetExporter(null, ObjectFactory.CreateDefaultAddress(),
                        ObjectFactory.CreateEmptyContact());

            Assert.Throws<ArgumentNullException>("business", setExporter);
        }

        [Fact]
        public void AddressCantBeNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action setExporter =
                () =>
                    notification.SetExporter(ObjectFactory.CreateEmptyBusiness(), null,
                        ObjectFactory.CreateEmptyContact());

            Assert.Throws<ArgumentNullException>("address", setExporter);
        }

        [Fact]
        public void ContactCantBeNull()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            Action setExporter =
                () =>
                    notification.SetExporter(ObjectFactory.CreateEmptyBusiness(), ObjectFactory.CreateDefaultAddress(),
                        null);

            Assert.Throws<ArgumentNullException>("contact", setExporter);
        }
    }
}