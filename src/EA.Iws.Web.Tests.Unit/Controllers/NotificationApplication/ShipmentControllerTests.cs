namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.Shipment;
    using Core.Notification.Audit;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Web.Infrastructure;
    using Xunit;

    public class ShipmentControllerTests
    {
        private readonly ShipmentController shipmentController;
        private readonly string numberOfShipments = "1";
        private readonly IMediator mediator;
        private readonly IAuditService auditService;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");
        public ShipmentControllerTests()
        {
            this.mediator = A.Fake<IMediator>();
            this.auditService = A.Fake<IAuditService>();
            shipmentController = new ShipmentController(A.Fake<IMediator>(), this.auditService);
            A.CallTo(() => auditService.AddAuditEntry(this.mediator, notificationId, "user", NotificationAuditType.Create, "screen"));
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new ShipmentInfoViewModel();
            model.NumberOfShipments = numberOfShipments;

            var result = await shipmentController.Index(Guid.Empty, model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToChemicalComposition()
        {
            var model = new ShipmentInfoViewModel();
            model.NumberOfShipments = numberOfShipments;

            var result = await shipmentController.Index(Guid.Empty, model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "ChemicalComposition");
        }
    }
}
