namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.WasteGenerationProcess;
    using Core.Notification.Audit;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Xunit;

    public class WasteGenerationProcessControllerTests
    {
        private readonly WasteGenerationProcessController wasteGenerationProcessController;
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");

        public WasteGenerationProcessControllerTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            wasteGenerationProcessController = new WasteGenerationProcessController(A.Fake<IMediator>(), this.auditService);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Create, "screen"));
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new WasteGenerationProcessViewModel();
            var result = await wasteGenerationProcessController.Index(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToPhysicalCharacteristics()
        {
            var model = new WasteGenerationProcessViewModel();
            var result = await wasteGenerationProcessController.Index(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "PhysicalCharacteristics");
        }
    }
}
