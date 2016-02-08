namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using Core.Notification;
    using Core.Shared;
    using Domain.NotificationApplication;
    using Xunit;

    public class NotificationReasonForExportTests
    {
        private readonly NotificationApplication notification;

        public NotificationReasonForExportTests()
        {
            notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal,
                UKCompetentAuthority.England, 0);
        }

        [Fact]
        public void ReasonForExportCannotBeGreaterThan70Characters()
        {
            Action setReasonForExport = () => notification.ReasonForExport = "value1234567890123456789012345678901234567890123456789012345678901234567890";

            Assert.Throws<InvalidOperationException>(setReasonForExport);
        }

        [Fact]
        public void CanSetReasonForExport()
        {
            notification.ReasonForExport = "reason";

            Assert.Equal("reason", notification.ReasonForExport);
        }
    }
}