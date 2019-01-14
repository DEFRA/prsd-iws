namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Notification.Audit;
    using DataAccess;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Mapper;
    using RequestHandlers.Notification;
    using Requests.Notification;
    using Xunit;

    public class NotificationAuditGetHandlerTests
    {
        private readonly GetNotificationAuditHandler handler;
        private readonly GetNotificationAudits message;
        private readonly IwsContext context;

        public NotificationAuditGetHandlerTests(IMapper mapper)
        {
            context = new TestIwsContext();

            var repo = A.Fake<INotificationAuditRepository>();

            var notificationUpdateHistory = new List<Audit>()
            {
                new Audit(Guid.NewGuid(), "UserName1", 1, 1, new DateTimeOffset()),
                new Audit(Guid.NewGuid(), "UserName2", 1, 1, new DateTimeOffset())
            };

            A.CallTo(() => repo.GetNotificationAuditsById(Guid.NewGuid())).Returns(notificationUpdateHistory);

            handler = new GetNotificationAuditHandler(context, mapper, repo);
            message = new GetNotificationAudits(Guid.NewGuid());
        }

        [Fact]
        public async Task NotificationUpdateHistoryIsRetrieved()
        {
            var result = await handler.HandleAsync(message);

            Assert.Equal(result.Count(), 2);
        }
    }
}