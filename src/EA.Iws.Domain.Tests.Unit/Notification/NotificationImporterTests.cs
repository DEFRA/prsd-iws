namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using Domain.Notification;
    using Helpers;
    using Xunit;

    public class NotificationImporterTests
    {
        [Fact]
        public void CanAddImporter()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = ObjectFactory.CreateEmptyBusiness();
            var address = ObjectFactory.CreateEmptyAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.AddImporter(business, address, contact);

            Assert.True(notification.HasImporter);
        }

        [Fact]
        public void CantAddMultipleImporters()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = ObjectFactory.CreateEmptyBusiness();
            var address = ObjectFactory.CreateEmptyAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.AddImporter(business, address, contact);

            Action addSecondImporter = () => notification.AddImporter(business, address, contact);

            Assert.Throws<InvalidOperationException>(addSecondImporter);
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
            var address = ObjectFactory.CreateEmptyAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.AddImporter(business, address, contact);

            notification.RemoveImporter();

            Assert.Null(notification.Importer);
        }
    }
}