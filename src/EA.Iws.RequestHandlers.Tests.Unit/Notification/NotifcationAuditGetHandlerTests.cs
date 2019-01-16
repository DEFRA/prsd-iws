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
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationAuditGetHandlerTests
    {
        private readonly GetNotificationAuditHandler handler;
        private readonly GetNotificationAudits message;
        private readonly IwsContext context;
        private readonly IMapper mapper;
        private readonly Audit notificationAudit;
        private readonly List<Audit> notificationAudits;

        public NotificationAuditGetHandlerTests()
        {
            context = new TestIwsContext();
            
            var repo = A.Fake<INotificationAuditRepository>();
            mapper = A.Fake<IMapper>();

            var notificationId = Guid.NewGuid();
            notificationAudit = new Audit(Guid.NewGuid(), "UserGuid1", 1, 1, new DateTimeOffset());
            notificationAudits = new List<Audit>()
            {
                notificationAudit,
                notificationAudit
            };
            var notificationAuditForDisplay = new NotificationAuditForDisplay("UserName1", "Screen1", "AuditType1", new DateTimeOffset());

            A.CallTo(() => repo.GetNotificationAuditsById(notificationId)).Returns(notificationAudits);
            A.CallTo(() => mapper.Map<NotificationAuditForDisplay>(notificationAudit)).Returns(notificationAuditForDisplay);

            handler = new GetNotificationAuditHandler(context, mapper, repo);
            message = new GetNotificationAudits(notificationId);
        }

        [Fact]
        public async Task NotificationUpdateHistoryIsRetrieved()
        {
            var result = await handler.HandleAsync(message);
            
            Assert.Equal(result.Count(), 2);
            // mapper.Map should be called once for each item in notificationAudits
            A.CallTo(() => mapper.Map<NotificationAuditForDisplay>(notificationAudit))
                .MustHaveHappened(Repeated.Exactly.Times(notificationAudits.Count()));
        }
    }
}