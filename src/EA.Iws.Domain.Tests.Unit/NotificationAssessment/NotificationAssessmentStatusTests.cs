namespace EA.Iws.Domain.Tests.Unit.NotificationAssessment
{
    using System;
    using Core.NotificationAssessment;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Xunit;

    public class NotificationAssessmentStatusTests
    {
        private readonly Guid notificationId;
        private readonly NotificationAssessment notificationAssessment;
        private readonly IDeferredEventDispatcher dispatcher;
        private readonly INotificationProgressCalculator progressCalculator;

        public NotificationAssessmentStatusTests()
        {
            notificationId = new Guid("C4C62654-048C-45A2-BF7F-9837EFCF328F");
            notificationAssessment = new NotificationAssessment(notificationId);
            dispatcher = A.Fake<IDeferredEventDispatcher>();
            progressCalculator = A.Fake<INotificationProgressCalculator>();
            A.CallTo(() => progressCalculator.IsComplete(notificationId)).Returns(true);
            DomainEvents.Dispatcher = dispatcher;
        }

        [Fact]
        public void DefaultStatusIsNotSubmitted()
        {
            Assert.Equal(NotificationStatus.NotSubmitted, notificationAssessment.Status);
        }

        [Fact]
        public void SubmitChangesStatusToSubmitted()
        {
            notificationAssessment.Submit(progressCalculator);

            Assert.Equal(NotificationStatus.Submitted, notificationAssessment.Status);
        }

        [Fact]
        public void CanSubmitTwice()
        {
            notificationAssessment.Submit(progressCalculator);
            notificationAssessment.Submit(progressCalculator);

            Assert.Equal(NotificationStatus.Submitted, notificationAssessment.Status);
        }

        [Fact]
        public void SubmitRaisesStatusChangeEvent()
        {
            notificationAssessment.Submit(progressCalculator);

            A.CallTo(() => dispatcher.Dispatch(A<NotificationStatusChangeEvent>.That.Matches(p => Equals(p.NotificationAssessment, notificationAssessment)
                && p.TargetStatus == NotificationStatus.Submitted))).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void SubmitRaisesNotificationSubmittedEvent()
        {
            notificationAssessment.Submit(progressCalculator);

            A.CallTo(() => dispatcher.Dispatch(A<NotificationSubmittedEvent>.That.Matches(p => p.NotificationApplicationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public void CantSubmitIncompleteApplication()
        {
            A.CallTo(() => progressCalculator.IsComplete(A<Guid>._)).Returns(false);

            Action submit = () => notificationAssessment.Submit(progressCalculator);

            Assert.Throws<InvalidOperationException>(submit);
        }
    }
}