namespace EA.Iws.RequestHandlers.Tests.Unit.CustomsOffice
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using RequestHandlers.CustomsOffice;
    using Requests.CustomsOffice;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetCustomsOfficeRequiredStatusByNotificationIdHandlerTests
    {
        private NotificationApplication notification1;
        private readonly IwsContext context;

        public GetCustomsOfficeRequiredStatusByNotificationIdHandlerTests()
        {
            notification1 = new NotificationApplication(TestIwsContext.UserId, NotificationType.Recovery, UKCompetentAuthority.England, 500);
            EntityHelper.SetEntityId(notification1, new Guid("295B0511-D0EB-43B4-9D17-938E1A34F0D3"));

            var transport = new TransportRoute(notification1.Id);

            context = new TestIwsContext();

            context.NotificationApplications.Add(notification1);
            context.TransportRoutes.Add(transport);
        }

        [Fact]
        public async Task NotificationDoesNotExist_Throws()
        {
            var handler = new GetCustomsCompletionStatusByNotificationIdHandler(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(new GetCustomsCompletionStatusByNotificationId(new Guid("A06E5E68-CB8B-40E3-A6F1-A50C5FFC30AB"))));
        }

        [Fact]
        public async Task NotificationExists_ReturnsACustomsOfficeStatus()
        {
            var handler = new GetCustomsCompletionStatusByNotificationIdHandler(context);

            var result = await handler.HandleAsync(new GetCustomsCompletionStatusByNotificationId(notification1.Id));

            Assert.IsType<CustomsOfficeCompletionStatus>(result);
        }
    }
}
