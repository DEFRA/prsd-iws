namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using EA.Iws.Core.Notification.AdditionalCharge;
    using EA.Iws.Domain.NotificationApplication;
    using EA.Iws.Domain.NotificationAssessment;
    using EA.Iws.RequestHandlers.Notification;
    using EA.Prsd.Core.Mapper;
    using FakeItEasy;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class AdditionalChargesGetHandlerTests
    {
        private readonly GetNotificationAdditionalChargesHandler handler;
        private readonly IMapper mapper;
        private readonly INotificationAdditionalChargeRepository repository;
        private readonly Guid notificationId;

        public AdditionalChargesGetHandlerTests()
        {
            notificationId = Guid.NewGuid();
            repository = A.Fake<INotificationAdditionalChargeRepository>();
            var context = new TestIwsContext();
            mapper = A.Fake<IMapper>();

            var charges = new List<AdditionalCharge>();
            var chargeItem = new AdditionalCharge(Guid.NewGuid(), notificationId, DateTime.UtcNow, 100, (int)AdditionalChargeType.Export, "Test");

            charges.Add(chargeItem);

            A.CallTo(() => repository.GetPagedNotificationAdditionalChargesById(notificationId)).Returns(charges);

            handler = new GetNotificationAdditionalChargesHandler(mapper, repository);
        }

        [Fact]
        public async Task Get_AdditionalChargesHandler()
        {
            var request = new Requests.Notification.GetNotificationAdditionalCharges(notificationId);

            await handler.HandleAsync(request);

            A.CallTo(() => repository.GetPagedNotificationAdditionalChargesById(notificationId)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task AddNewNotification_ReturnsTrue()
        {
            var request = new Requests.Notification.GetNotificationAdditionalCharges(notificationId);

            var result = await handler.HandleAsync(request);
            Assert.Equal(1, result.Count);
        }
    }
}
