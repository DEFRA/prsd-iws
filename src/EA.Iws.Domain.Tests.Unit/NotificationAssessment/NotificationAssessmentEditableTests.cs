namespace EA.Iws.Domain.Tests.Unit.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationAssessmentEditableTests
    {
        private readonly NotificationAssessment notificationAssessment;
        private readonly Guid notificationId;

        public NotificationAssessmentEditableTests()
        {
            notificationId = new Guid("C4C62654-048C-45A2-BF7F-9837EFCF328F");
            notificationAssessment = new NotificationAssessment(notificationId);
        }

        private void SetNotificationStatus(NotificationStatus status)
        {
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, status,
                notificationAssessment);
        }

        [Theory]
        [InlineData(NotificationStatus.NotSubmitted)]
        [InlineData(NotificationStatus.Submitted)]
        [InlineData(NotificationStatus.NotificationReceived)]
        [InlineData(NotificationStatus.InAssessment)]
        public void CanEditNotification(NotificationStatus status)
        {
            SetNotificationStatus(status);

            Assert.True(notificationAssessment.CanEditNotification);
        }

        [Theory]
        [InlineData(NotificationStatus.ReadyToTransmit)]
        [InlineData(NotificationStatus.Transmitted)]
        [InlineData(NotificationStatus.DecisionRequiredBy)]
        public void CanNotEditNotification(NotificationStatus status)
        {
            SetNotificationStatus(status);

            Assert.False(notificationAssessment.CanEditNotification);
        }
    }
}