namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using NotificationApplication;
    using Xunit;

    public class NotificationRecoveryPercentageTests
    {
        private const string MethodOfDisposal = "disposal method description";

        private static NotificationApplication CreateNotificationApplication()
        {
            return new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);
        }

        [Fact]
        public void RecoveryPercentageCannotBeGreaterThan100()
        {
            var notification = CreateNotificationApplication();

            Action setRecoveryPercentageData = () => notification.SetRecoveryPercentageData(101, MethodOfDisposal);

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
        public void RecoveryPercentageRoundedTo100IsNotAllowed()
        {
            var notification = CreateNotificationApplication();

            Action setRecoveryPercentageData = () => notification.SetRecoveryPercentageData(99.999999M, MethodOfDisposal);

            Assert.Throws<InvalidOperationException>(setRecoveryPercentageData);
        }

        [Fact]
        public void RecoveryPercentageRoundedUpToTwoDecimalPlaces()
        {
            var notification = CreateNotificationApplication();

            notification.SetRecoveryPercentageData(12.9876543M, MethodOfDisposal);

            Assert.True(notification.PercentageRecoverable == 12.99M);
        }

        [Fact]
        public void IfRecoveryPercentageIs100ThenMethodOfDisposalIsNull()
        {
            var notification = CreateNotificationApplication();

            Action setRecoveryPercentageData = () => notification.SetRecoveryPercentageData(100, MethodOfDisposal);

            Assert.Throws<InvalidOperationException>(setRecoveryPercentageData);
        }
    }
}
