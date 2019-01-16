namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using Core.Notification.Audit;
    using DataAccess;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.Notification;
    using Requests.Notification;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class NotificationAuditCreatedHandlerTests
    {
        private readonly CreateNotificationAuditHandler handler;
        private readonly CreateNotificationAudit message;
        private readonly IwsContext context;

        public NotificationAuditCreatedHandlerTests()
        {
            context = new TestIwsContext();

            var repo = A.Fake<INotificationAuditRepository>();

            var audit = A.Fake<Audit>();
            context.NotificationAudit.Add(audit);

            handler = new CreateNotificationAuditHandler(context, repo);
            message = new CreateNotificationAudit()
            {
                NotificationId = audit.NotificationId,
                UserId = audit.UserId,
                DateAdded = audit.DateAdded,
                Screen = audit.Screen,
                Type = (NotificationAuditType)audit.Type
            };
        }

        [Fact]
        public async Task AuditIsAdded()
        {
            await handler.HandleAsync(message);

            Assert.Equal(1, context.NotificationAudit.Count(p => p.NotificationId == message.NotificationId));
        }
    }
}
