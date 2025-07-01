namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Iws.RequestHandlers.Facilities;
    using EA.Iws.Requests.Facilities;
    using FakeItEasy;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class MarkInterimStatusToNotificationHandlerTests
    {
        private readonly MarkInterimStatusToNotificationHandler handler;
        private readonly IFacilityRepository facilityRepository;
        private readonly Guid notificationId;
        private readonly NotificationInterim notificationInterim;

        public MarkInterimStatusToNotificationHandlerTests()
        {
            var context = new TestIwsContext();
            facilityRepository = A.Fake<IFacilityRepository>();
            notificationInterim = A.Fake<NotificationInterim>();
            notificationId = Guid.NewGuid();

            context.Facilities.Add(new FacilityCollection(notificationId));

            handler = new MarkInterimStatusToNotificationHandler(notificationInterim, context);
        }

        [Fact]
        public async Task UpdateInterimStatusHandler_AsTrue_And_ReturnsTrue()
        {
            var facilityCollection = new FacilityCollection(notificationId);

            A.CallTo(() => facilityRepository.GetByNotificationId(notificationId)).Returns(facilityCollection);

            var result = await handler.HandleAsync(new MarkInterimStatusToNotification(notificationId, true));

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateInterimStatusHandler_AsFalse_And_ReturnsTrue()
        {
            A.CallTo(() => facilityRepository.GetByNotificationId(notificationId)).Returns((FacilityCollection)null);

            var result = await handler.HandleAsync(new MarkInterimStatusToNotification(notificationId, false));

            Assert.True(result);
        }
    }
}
