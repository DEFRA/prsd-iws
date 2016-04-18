namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Core.Notification;
    using Core.Shared;
    using Domain.ImportNotification;
    using Xunit;

    public class ImportNotificationTests
    {
        [Fact]
        public void CanCreateImportNotification()
        {
            var import = new ImportNotification(NotificationType.Recovery, UKCompetentAuthority.England, "DE 001 12345");

            Assert.NotNull(import);
        }

        [Fact]
        public void NotificationTypeCantBeDefault()
        {
            Assert.Throws<ArgumentException>("notificationType",
                () => new ImportNotification(default(NotificationType), UKCompetentAuthority.England, "DE 001 12345"));
        }

        [Fact]
        public void NotificationNumberCantBeNull()
        {
            Assert.Throws<ArgumentNullException>("notificationNumber",
                () => new ImportNotification(NotificationType.Recovery, UKCompetentAuthority.England, null));
        }

        [Fact]
        public void NotificationNumberCantBeEmpty()
        {
            Assert.Throws<ArgumentException>("notificationNumber",
                () => new ImportNotification(NotificationType.Recovery, UKCompetentAuthority.England, string.Empty));
        }
    }
}