namespace EA.Iws.RequestHandlers.Tests.Unit.CustomsOffice
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core;
    using Core.CustomsOffice;
    using Cqrs.Tests.Unit.Helpers;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using FakeItEasy;
    using RequestHandlers.CustomsOffice;
    using Requests.CustomsOffice;
    using TestHelpers.Helpers;
    using Xunit;

    public class GetCustomsOfficeRequiredStatusByNotificationIdHandlerTests
    {
        private NotificationApplication notification1;
        private readonly DbSet<NotificationApplication> notifications;
        private readonly IwsContext context;

        public GetCustomsOfficeRequiredStatusByNotificationIdHandlerTests()
        {
            notification1 = new NotificationApplication(Guid.Empty, NotificationType.Recovery, UKCompetentAuthority.England, 500);
            EntityHelper.SetEntityId(notification1, new Guid("295B0511-D0EB-43B4-9D17-938E1A34F0D3"));

            DbContextHelper dbContextHelper = new DbContextHelper();
            notifications = dbContextHelper.GetAsyncEnabledDbSet(new[]
            {
                notification1
            });

            context = A.Fake<IwsContext>();

            A.CallTo(() => context.NotificationApplications).Returns(notifications);
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
