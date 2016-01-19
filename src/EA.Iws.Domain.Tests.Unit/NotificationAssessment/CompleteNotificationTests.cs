namespace EA.Iws.Domain.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using TestHelpers.Helpers;
    using Xunit;

    public class CompleteNotificationTests
    {
        private readonly CompleteNotification completeNotification;
        private readonly NotificationAssessment notificationAssessment;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly IFacilityRepository facilityRepository;
        private readonly Guid notificationId = new Guid("7594E739-B64C-4E91-ABFB-513768BE358C");

        public CompleteNotificationTests()
        {
            notificationAssessmentRepository = A.Fake<INotificationAssessmentRepository>();
            facilityRepository = A.Fake<IFacilityRepository>();

            completeNotification = new CompleteNotification(notificationAssessmentRepository, facilityRepository);

            notificationAssessment = new NotificationAssessment(notificationId);

            A.CallTo(() => notificationAssessmentRepository.GetByNotificationId(notificationId))
                .Returns(notificationAssessment);
        }

        private void SetupFacilityCollection(bool? isInterim)
        {
            var facilityCollection = new FacilityCollection(notificationId);

            if (isInterim.HasValue)
            {
                facilityCollection.SetIsInterim(isInterim.Value);
            }

            A.CallTo(() => facilityRepository.GetByNotificationId(notificationId))
                .Returns(facilityCollection);
        }

        private void SetNotificationStatus(NotificationStatus status)
        {
            ObjectInstantiator<NotificationAssessment>.SetProperty(x => x.Status, status,
                notificationAssessment);
        }

        [Fact]
        public async Task IsInterimNotSet_Throws()
        {
            SetupFacilityCollection(null);
            SetNotificationStatus(NotificationStatus.InAssessment);

            await Assert.ThrowsAsync<InvalidOperationException>(() => completeNotification.Complete(notificationId, new DateTime(2016, 1, 15)));
        }

        [Fact]
        public async Task IsInterimSet_SetsCompleteDate()
        {
            SetupFacilityCollection(true);
            SetNotificationStatus(NotificationStatus.InAssessment);

            var completedDate = new DateTime(2016, 1, 15);

            await completeNotification.Complete(notificationId, completedDate);

            Assert.Equal(completedDate, notificationAssessment.Dates.CompleteDate);
        }

        [Fact]
        public async Task CanComplete_IsInterimNotSet_ReturnsFalse()
        {
            SetupFacilityCollection(null);

            var result = await completeNotification.CanComplete(notificationId);

            Assert.False(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CanComplete_IsInterimSet_ReturnsTrue(bool isInterim)
        {
            SetupFacilityCollection(isInterim);

            var result = await completeNotification.CanComplete(notificationId);

            Assert.True(result);
        }
    }
}