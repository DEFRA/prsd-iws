namespace EA.Iws.Web.Tests.Unit.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Core.Notification.Audit;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Notification;
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
            await auditService.AddAuditEntry(this.mediator, notificationId, userId.ToString(), NotificationAuditType.Create, NotificationAuditScreenType.Exporter);

            A.CallTo(() => mediator.SendAsync(A<CreateNotificationAudit>.Ignored)).MustHaveHappened(Repeated.AtLeast.Once);
        }
    }
}
