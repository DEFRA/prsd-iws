namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Domain.NotificationApplication;
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

            Action setRecoveryPercentageData = () => notification.SetPercentageRecoverable(101);

            Assert.Throws<InvalidOperationException>(setRecoveryPercentageData);
        }

        [Fact]
        public void RecoveryPercentageCannotBeLessThanZero()
        {
            var notification = CreateNotificationApplication();

            Action setRecoveryPercentageData = () => notification.SetPercentageRecoverable(-1);

            Assert.Throws<InvalidOperationException>(setRecoveryPercentageData);
        }

        [Fact]
        public void RecoveryPercentageRoundedUpToTwoDecimalPlaces()
        {
            var notification = CreateNotificationApplication();

            notification.SetPercentageRecoverable(12.9876543M);

            Assert.True(notification.PercentageRecoverable == 12.99M);
        }

        [Fact]
        public void IfRecoveryPercentageIs100ThenMethodOfDisposalIsNull()
        {
            var notification = CreateNotificationApplication();

            notification.SetPercentageRecoverable(100);
            Action setRecoveryPercentageData = () => notification.SetMethodOfDisposal(MethodOfDisposal);

            Assert.Throws<InvalidOperationException>(setRecoveryPercentageData);
        }
    }
}
