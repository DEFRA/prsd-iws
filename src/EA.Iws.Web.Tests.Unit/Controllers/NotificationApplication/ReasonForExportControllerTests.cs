namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.NotificationApplication;
    using Core.Notification.Audit;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Xunit;

    public class ReasonForExportControllerTests
    {
        private readonly ReasonForExportController reasonForExportController;
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");

        public ReasonForExportControllerTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            reasonForExportController = new ReasonForExportController(A.Fake<IMediator>(), this.auditService);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Create, "screen"));
        }

        [Fact]
        public async Task ReasonForExport_Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new ReasonForExportViewModel();

            var result = await reasonForExportController.Index(model, true) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task ReasonForExport_Post_BackToOverviewFalse_RedirectsToCarrierList()
        {
            var model = new ReasonForExportViewModel();

            var result = await reasonForExportController.Index(model, false) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "List", "Carrier");
        }

        [Fact]
        public async Task ReasonForExport_Post_BackToOverviewNull_RedirectsToCarrierList()
        {
            var model = new ReasonForExportViewModel();

            var result = await reasonForExportController.Index(model, null) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "List", "Carrier");
        }
    }
}
