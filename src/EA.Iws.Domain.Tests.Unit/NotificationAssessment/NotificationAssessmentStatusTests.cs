namespace EA.Iws.Domain.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Linq;
    using Core.NotificationAssessment;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationAssessmentStatusTests
    {
        private readonly Guid notificationId;
        private readonly NotificationAssessment notificationAssessment;
        private readonly INotificationProgressService progressService;
        private DateTime receivedDate = new DateTime(2015, 8, 1);

        public NotificationAssessmentStatusTests()
        {
            notificationId = new Guid("C4C62654-048C-45A2-BF7F-9837EFCF328F");
            notificationAssessment = new NotificationAssessment(notificationId);
            progressService = A.Fake<INotificationProgressService>();
            A.CallTo(() => progressService.IsComplete(notificationId)).Returns(true);
        }

        private void SetNotificationStatusToSubmitted()
        {
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, NotificationStatus.Submitted,
                notificationAssessment);
        }

        [Fact]
        public void DefaultStatusIsNotSubmitted()
        {
            Assert.Equal(NotificationStatus.NotSubmitted, notificationAssessment.Status);
        }

        [Fact]
        public void SubmitChangesStatusToSubmitted()
        {
            notificationAssessment.Submit(progressService);

            Assert.Equal(NotificationStatus.Submitted, notificationAssessment.Status);
        }

        [Fact]
        public void CantSubmitTwice()
        {
            notificationAssessment.Submit(progressService);
            Action submitAgain = () => notificationAssessment.Submit(progressService);

            Assert.Throws<InvalidOperationException>(submitAgain);
        }

        [Fact]
        public void SubmitRaisesStatusChangeEvent()
        {
            notificationAssessment.Submit(progressService);

            Assert.Equal(notificationAssessment,
                notificationAssessment.Events.OfType<NotificationStatusChangeEvent>()
                    .SingleOrDefault()
                    .NotificationAssessment);
        }

        [Fact]
        public void SubmitRaisesNotificationSubmittedEvent()
        {
            notificationAssessment.Submit(progressService);

            Assert.Equal(notificationId,
                notificationAssessment.Events.OfType<NotificationSubmittedEvent>()
                    .SingleOrDefault()
                    .NotificationApplicationId);
        }

        [Fact]
        public void CantSubmitIncompleteApplication()
        {
            A.CallTo(() => progressService.IsComplete(A<Guid>._)).Returns(false);

            Action submit = () => notificationAssessment.Submit(progressService);

            Assert.Throws<InvalidOperationException>(submit);
        }

        [Fact]
        public void SetNotificationReceivedSetsDate()
        {
            SetNotificationStatusToSubmitted();

            notificationAssessment.SetNotificationReceived(receivedDate);

            Assert.Equal(receivedDate, notificationAssessment.Dates.NotificationReceivedDate);
        }

        [Fact]
        public void SetNotificationReceivedChangesStatusToNotificationReceived()
        {
            SetNotificationStatusToSubmitted();

            notificationAssessment.SetNotificationReceived(receivedDate);

            Assert.Equal(NotificationStatus.NotificationReceived, notificationAssessment.Status);
        }

        [Fact]
        public void CantSetNotificationStatusWhenNotSubmitted()
        {
            Action setNotificationReceived = () => notificationAssessment.SetNotificationReceived(receivedDate);

            Assert.Throws<InvalidOperationException>(setNotificationReceived);
        }

        [Fact]
        public void SetNotificationReceivedRaisesStatusChangeEvent()
        {
            SetNotificationStatusToSubmitted();

            notificationAssessment.SetNotificationReceived(receivedDate);

            Assert.Equal(notificationAssessment,
                notificationAssessment.Events.OfType<NotificationStatusChangeEvent>()
                    .SingleOrDefault()
                    .NotificationAssessment);
        }
    }
}