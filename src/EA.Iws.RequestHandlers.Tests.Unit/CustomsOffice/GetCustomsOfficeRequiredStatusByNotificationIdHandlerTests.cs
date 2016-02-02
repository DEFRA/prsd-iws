namespace EA.Iws.RequestHandlers.Tests.Unit.CustomsOffice
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using FakeItEasy;
    using RequestHandlers.CustomsOffice;
    using Requests.CustomsOffice;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationApplicationFactory = TestHelpers.Helpers.NotificationApplicationFactory;

    public class GetCustomsOfficeRequiredStatusByNotificationIdHandlerTests
    {
        private NotificationApplication notification1;
        private readonly IwsContext context;
        private GetCustomsCompletionStatusByNotificationIdHandler handler;
        private readonly Guid notificationId;

        public GetCustomsOfficeRequiredStatusByNotificationIdHandlerTests()
        {
            notification1 = NotificationApplicationFactory.Create(TestIwsContext.UserId, NotificationType.Recovery, UKCompetentAuthority.England, 500);
            notificationId = new Guid("295B0511-D0EB-43B4-9D17-938E1A34F0D3");

            EntityHelper.SetEntityId(notification1, notificationId);

            var transport = new TransportRoute(notificationId);

            context = new TestIwsContext();

            context.NotificationApplications.Add(notification1);
            context.TransportRoutes.Add(transport);

            var repository = A.Fake<ITransportRouteRepository>();
            A.CallTo(() => repository.GetByNotificationId(notificationId)).Returns(transport);

            handler = new GetCustomsCompletionStatusByNotificationIdHandler(repository);
        }

        [Fact]
        public async Task NotificationExists_ReturnsACustomsOfficeStatus()
        {
            var result = await handler.HandleAsync(new GetCustomsCompletionStatusByNotificationId(notification1.Id));

            Assert.IsType<CustomsOfficeCompletionStatus>(result);
        }
    }
}
