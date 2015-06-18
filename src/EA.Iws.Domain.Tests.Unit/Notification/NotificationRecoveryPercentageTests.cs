namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using Domain.Notification;
    using Xunit;

    public class NotificationRecoveryPercentageTests
    {
        private static NotificationApplication CreateNotificationApplication()
        {
            return new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);
        }

        [Fact]
        public void RecoveryPercentageCannotBeGreaterThan100()
        {
            var notification = CreateNotificationApplication();

            Action setRecoveryPercentageData = () => notification.SetRecoveryPercentageData(101, "Some Text");

            Assert.Throws<InvalidOperationException>(setRecoveryPercentageData);
        }

        [Fact]
        public void RecoveryPercentageCannotBeGreaterLessThanZero()
        {
            var notification = CreateNotificationApplication();

            Action setRecoveryPercentageData = () => notification.SetRecoveryPercentageData(-1, null);

            Assert.Throws<InvalidOperationException>(setRecoveryPercentageData);
        }

        [Fact]
        public void MethodOfDisposalCannotBeNull()
        {
            var notification = CreateNotificationApplication();

            Action setRecoveryPercentageData = () => notification.SetRecoveryPercentageData(50, null);

            Assert.Throws<InvalidOperationException>(setRecoveryPercentageData);
        }

        [Fact]
        public void IfRecoveryPercentageIs100ThenMethodOfDisposalIsNull()
        {
            var notification = CreateNotificationApplication();

            Action setRecoveryPercentageData = () => notification.SetRecoveryPercentageData(100, "Some Text");

            Assert.Throws<InvalidOperationException>(setRecoveryPercentageData);
        }
    }
}
