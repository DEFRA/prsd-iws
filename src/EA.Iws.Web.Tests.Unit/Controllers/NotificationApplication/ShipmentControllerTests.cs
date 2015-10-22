namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.Shipment;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Xunit;

    public class ShipmentControllerTests
    {
        private readonly ShipmentController shipmentController;

        public ShipmentControllerTests()
        {
            shipmentController = new ShipmentController(A.Fake<IMediator>());
        }

        [Fact]
        public async Task Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new ShipmentInfoViewModel();
            var result = await shipmentController.Index(model, true) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task Post_BackToOverviewFalse_RedirectsToChemicalComposition()
        {
            var model = new ShipmentInfoViewModel();
            var result = await shipmentController.Index(model, false) as RedirectToRouteResult;
            RouteAssert.RoutesTo(result.RouteValues, "Index", "ChemicalComposition");
        }
    }
}
