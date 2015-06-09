namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using Domain.Notification;
    using Helpers;
    using Xunit;

    public class NotificationExporterTests
    {
        [Fact]
        public void CanAddExporter()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = ObjectFactory.CreateEmptyBusiness();
            var address = ObjectFactory.CreateEmptyAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.AddExporter(business, address, contact);

            Assert.True(notification.HasExporter);
        }

        [Fact]
        public void CantAddMultipleExporters()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);

            var business = ObjectFactory.CreateEmptyBusiness();
            var address = ObjectFactory.CreateEmptyAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.AddExporter(business, address, contact);

            Action addSecondExporter = () => notification.AddExporter(business, address, contact);

            Assert.Throws<InvalidOperationException>(addSecondExporter);
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
            var address = ObjectFactory.CreateEmptyAddress();
            var contact = ObjectFactory.CreateEmptyContact();

            notification.AddExporter(business, address, contact);

            notification.RemoveExporter();

            Assert.Null(notification.Exporter);
        }
    }
}