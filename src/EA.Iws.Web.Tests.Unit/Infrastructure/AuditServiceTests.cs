namespace EA.Iws.Web.Tests.Unit.Infrastructure
{
    using Core.Notification.Audit;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Web.Infrastructure;
    using Xunit;

    public class AuditServiceTests
    {
        private readonly IMediator mediator;
        private readonly IAuditService auditService;

        private readonly Guid notificationId = new Guid("81CBBCEE-34C0-4628-B054-E0D8135A7947");
        private readonly Guid userId = new Guid("81CBBCEE-34C0-4628-B054-E0D8135A7947");

        public AuditServiceTests()
        {
            mediator = A.Fake<IMediator>();
            auditService = new AuditService();
        }

        [Fact]
        public async Task Exporter_AddExporter_AuditMustBeCalled()
        {
            var screens = A.CollectionOfFake<NotificationAuditScreen>(1);
            A.CallTo(() => mediator.SendAsync(A<GetNotificationAuditScreens>.Ignored)).Returns(screens);

            await this.auditService.AddAuditEntry(this.mediator, notificationId, userId.ToString(), true, screens.FirstOrDefault().ScreenName);

            A.CallTo(() => mediator.SendAsync(A<GetNotificationAuditScreens>.Ignored)).MustHaveHappened(Repeated.AtLeast.Once);

            A.CallTo(() => mediator.SendAsync(A<CreateNotificationAudit>.Ignored)).MustHaveHappened(Repeated.AtLeast.Once);
        }
    }
}
