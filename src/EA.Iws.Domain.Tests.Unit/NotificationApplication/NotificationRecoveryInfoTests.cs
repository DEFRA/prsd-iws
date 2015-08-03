namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Core.RecoveryInfo;
    using Domain.NotificationApplication;
    using Xunit;

    public class NotificationRecoveryInfoTests
    {
        private static NotificationApplication CreateNotificationApplication()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
            return notification;
        }

        [Fact]
        public void CanAddRecoveryInfoValues()
        {
            var notification = CreateNotificationApplication();
            notification.SetRecoveryInfoValues(RecoveryInfoUnits.Kilogram, 10, RecoveryInfoUnits.Tonne, 20, RecoveryInfoUnits.Kilogram, 30);
            Assert.True(notification.HasRecoveryInfo);
        }

        [Fact]
        public void CanAddRecoveryInfoValues_WithoutDisposal()
        {
            var notification = CreateNotificationApplication();
            notification.SetRecoveryInfoValues(RecoveryInfoUnits.Kilogram, 10, RecoveryInfoUnits.Tonne, 20, null, null);
            Assert.True(notification.HasRecoveryInfo);
        }
    }
}
