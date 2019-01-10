namespace EA.Iws.RequestHandlers.Tests.Unit.Notification
{
    using DataAccess;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.Notification;
    using Requests.Notification;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class NotificationAuditScreenGetHandlerTests
    {
        private readonly GetNotificationAuditScreensHandler handler;
        private readonly GetNotificationAuditScreens message;
        private readonly IwsContext context;

        public NotificationAuditScreenGetHandlerTests()
        {
            context = new TestIwsContext();

            var repo = A.Fake<INotificationAuditScreenRepository>();

            List<AuditScreen> auditScreens = new List<AuditScreen>()
            {
                new AuditScreen("Screen1"),
                new AuditScreen("Screen2")
            };

            A.CallTo(() => repo.GetNotificationAuditScreens()).Returns(auditScreens);

            handler = new GetNotificationAuditScreensHandler(context, repo);
            message = new GetNotificationAuditScreens();
        }

        [Fact]
        public async Task NotificationScreensAreRetrieved()
        {
            var result = await handler.HandleAsync(message);

            Assert.Equal(result.Count, 2);
        }
    }
}
