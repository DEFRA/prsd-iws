namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.WasteRecovery;
    using Core.Notification.Audit;
    using Core.Shared;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.WasteRecovery;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Xunit;

    public class WasteRecoveryControllerTests
    {
        private static readonly Guid AnyGuid = Guid.NewGuid();

        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly WasteRecoveryController controller;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");

        public WasteRecoveryControllerTests()
        {
            mediator = A.Fake<IMediator>();
            auditService = A.Fake<IAuditService>();
            controller = new WasteRecoveryController(mediator, auditService);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Added, NotificationAuditScreenType.WasteRecovery));
        }

        [Fact]
        public async Task RedirectsToOverview_WhenProvidedByImporter()
        {
            var result = await controller.Index(AnyGuid, new WasteRecoveryViewModel(ProvidedBy.Importer));

            var routeResult = Assert.IsType<RedirectToRouteResult>(result);

            RouteAssert.RoutesTo(routeResult.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task RedirectsToCorrectScreen_WhenProvidedByNotifier()
        {
            var result = await controller.Index(AnyGuid, new WasteRecoveryViewModel(ProvidedBy.Notifier));

            var routeResult = Assert.IsType<RedirectToRouteResult>(result);

            RouteAssert.RoutesTo(routeResult.RouteValues, "Percentage", "WasteRecovery");
        }
    }
}
